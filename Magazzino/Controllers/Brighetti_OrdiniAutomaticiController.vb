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
            Dim esito = OrdiniAutomaticiService.GeneraProposte(
                User.Identity.GetUserId(), User.Identity.GetUserName(),
                rispettaToggle:=False, origine:="Manuale")
            TempData("Messaggio") = esito.Messaggio
            Return RedirectToAction("Index")
        End Function

        ' POST: conferma una proposta.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Conferma(ByVal id As Integer) As ActionResult
            CambiaStato(id, StatoOrdineAutomatico.Confermato)
            Return RedirectToAction("Index")
        End Function

        ' POST: annulla una proposta.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Annulla(ByVal id As Integer) As ActionResult
            CambiaStato(id, StatoOrdineAutomatico.Annullato)
            Return RedirectToAction("Index")
        End Function

        Private Sub CambiaStato(id As Integer, nuovoStato As StatoOrdineAutomatico)
            Dim ordine = db.Brighetti_OrdiniAutomatici.Find(id)
            If ordine Is Nothing Then Return
            ordine.Stato = nuovoStato
            ordine.UltimaModifica = New TipoUltimaModifica With {
                .Data = DateTime.Now,
                .OperatoreID = User.Identity.GetUserId(),
                .Operatore = User.Identity.GetUserName()
            }
            db.SaveChanges()
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace
