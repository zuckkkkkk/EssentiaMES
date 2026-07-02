Imports System
Imports System.Collections.Generic
Imports System.Data.Entity
Imports System.Linq
Imports System.Net
Imports System.Web
Imports System.Web.Mvc
Imports Microsoft.AspNet.Identity
Imports Brighetti

Namespace Controllers
    ''' <summary>
    ''' Gestione delle proposte di riordino automatico: elenco, generazione manuale,
    ''' conferma e annullamento. La generazione pianificata è gestita dallo scheduler.
    ''' </summary>
    <Authorize(Roles:="Admin")>
    Public Class Brighetti_OrdiniAutomaticiController
        Inherits System.Web.Mvc.Controller

        Private db As New BrighettiModels

        ' GET: Brighetti_OrdiniAutomatici
        Function Index() As ActionResult
            Dim lista = db.Brighetti_OrdiniAutomatici _
                .OrderByDescending(Function(x) x.DataGenerazione) _
                .ToList()
            ViewBag.OrdiniAutomaticiAbilitato = ImpostazioniService.LeggiBool(ChiaviImpostazioni.OrdiniAutomaticiAbilitato)
            ViewBag.Prefissi = ImpostazioniService.LeggiTesto(ChiaviImpostazioni.OrdiniAutomaticiPrefissi, "")
            Return View(lista)
        End Function

        ' POST: genera subito le proposte (esecuzione manuale dell'admin).
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Genera() As ActionResult
            Dim opID = User.Identity.GetUserId()
            Dim opName = User.Identity.GetUserName()
            Try
                Dim esito = OrdiniAutomaticiService.GeneraProposte(
                    opID, opName,
                    rispettaToggle:=False, origine:="Manuale")
                TempData("Messaggio") = esito.Messaggio
                db.Audit.Add(New Audit With {
                    .Livello = TipoAuditLivello.Info,
                    .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                    .Messaggio = "Generazione manuale proposte di riordino: " & esito.Messaggio,
                    .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(esito),
                    .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = opID, .Operatore = opName, .Data = DateTime.Now}
                })
                db.SaveChanges()
            Catch ex As Exception
                db.Log.Add(New Log With {
                    .Livello = TipoLogLivello.Errors,
                    .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                    .Messaggio = "Errore generazione manuale proposte di riordino: " & ex.Message,
                    .Dati = "",
                    .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = opID, .Operatore = opName, .Data = DateTime.Now}
                })
                db.SaveChanges()
                TempData("Messaggio") = "Errore durante la generazione delle proposte."
            End Try
            Return RedirectToAction("Index")
        End Function

        ' POST: conferma una proposta.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Conferma(ByVal id As Integer) As ActionResult
            CambiaStato(id, StatoOrdineAutomatico.Confermato, "Proposta di riordino confermata")
            Return RedirectToAction("Index")
        End Function

        ' POST: annulla una proposta.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Annulla(ByVal id As Integer) As ActionResult
            CambiaStato(id, StatoOrdineAutomatico.Annullato, "Proposta di riordino annullata")
            Return RedirectToAction("Index")
        End Function

        ' POST: segna un ordine come evaso (merce arrivata). Chiude il ciclo e
        ' consente di riproporre l'articolo se in futuro tornerà sotto scorta.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Evaso(ByVal id As Integer) As ActionResult
            CambiaStato(id, StatoOrdineAutomatico.Evaso, "Ordine di riordino segnato come evaso")
            Return RedirectToAction("Index")
        End Function

        Private Sub CambiaStato(id As Integer, nuovoStato As StatoOrdineAutomatico, messaggioAudit As String)
            Dim opID = User.Identity.GetUserId()
            Dim opName = User.Identity.GetUserName()
            Try
                Dim ordine = db.Brighetti_OrdiniAutomatici.Find(id)
                If ordine Is Nothing Then
                    TempData("Messaggio") = "Proposta di riordino non trovata (Id " & id & ")."
                    Return
                End If
                Dim statoPrecedente = ordine.Stato
                ordine.Stato = nuovoStato
                ordine.UltimaModifica = New TipoUltimaModifica With {
                    .Data = DateTime.Now,
                    .OperatoreID = opID,
                    .Operatore = opName
                }
                db.SaveChanges()
                db.Audit.Add(New Audit With {
                    .Livello = TipoAuditLivello.Info,
                    .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                    .Messaggio = messaggioAudit & " (Id " & id & ", articolo " & ordine.CodiceArticolo & ")",
                    .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {.Id = id, .StatoPrecedente = statoPrecedente, .StatoNuovo = nuovoStato}),
                    .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = opID, .Operatore = opName, .Data = DateTime.Now}
                })
                db.SaveChanges()
            Catch ex As Exception
                db.Log.Add(New Log With {
                    .Livello = TipoLogLivello.Errors,
                    .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                    .Messaggio = "Errore cambio stato ordine automatico (Id " & id & "): " & ex.Message,
                    .Dati = "",
                    .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = opID, .Operatore = opName, .Data = DateTime.Now}
                })
                db.SaveChanges()
                TempData("Messaggio") = "Errore durante l'aggiornamento della proposta di riordino."
            End Try
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace
