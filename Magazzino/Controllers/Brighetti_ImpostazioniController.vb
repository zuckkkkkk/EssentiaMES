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
    ''' Pannello di amministrazione delle impostazioni e degli interruttori (feature toggle)
    ''' degli automatismi. Permette di attivare gli automatismi UNO ALLA VOLTA.
    ''' </summary>
    <Authorize(Roles:="Admin")>
    Public Class Brighetti_ImpostazioniController
        Inherits System.Web.Mvc.Controller

        Private db As New BrighettiModels

        ' GET: Brighetti_Impostazioni
        Function Index() As ActionResult
            ' Crea le impostazioni mancanti senza sovrascrivere quelle già presenti.
            ImpostazioniService.AssicuraDefault()
            Dim lista = db.Brighetti_Impostazioni _
                .OrderBy(Function(x) x.Categoria) _
                .ThenBy(Function(x) x.Chiave) _
                .ToList()
            ' Raggruppo per categoria nel controller (System.Linq non è disponibile nelle view).
            Dim gruppi = lista _
                .GroupBy(Function(x) x.Categoria) _
                .Select(Function(g) New ImpostazioniCategoriaViewModel With {
                    .Categoria = g.Key,
                    .Impostazioni = g.ToList()
                }) _
                .ToList()
            Return View(gruppi)
        End Function

        ' POST: salva i valori testuali/numerici dal form.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Salva() As ActionResult
            Dim opId = User.Identity.GetUserId()
            Dim opName = User.Identity.GetUserName()
            ' Gli interruttori (Booleano) sono gestiti istantaneamente da Toggle: qui
            ' salviamo solo i parametri testuali/numerici.
            For Each imp In db.Brighetti_Impostazioni.Where(Function(x) x.Tipo <> TipoImpostazione.Booleano).ToList()
                Dim raw = Request.Form("imp_" & imp.Id.ToString())
                If raw IsNot Nothing Then
                    imp.Valore = raw
                    imp.UltimaModifica = New TipoUltimaModifica With {
                        .Data = DateTime.Now, .OperatoreID = opId, .Operatore = opName
                    }
                End If
            Next
            db.SaveChanges()
            TempData("Messaggio") = "Impostazioni salvate."
            Return RedirectToAction("Index")
        End Function

        ' POST AJAX: attiva/disattiva un singolo interruttore (un automatismo alla volta).
        <HttpPost()>
        Function Toggle(ByVal id As Integer) As JsonResult
            Dim imp = db.Brighetti_Impostazioni.Find(id)
            If imp Is Nothing Then
                Return Json(New With {.ok = False, .messaggio = "Impostazione non trovata"})
            End If
            Dim attuale = ImpostazioniService.LeggiBool(imp.Chiave)
            imp.Valore = If(attuale, "false", "true")
            imp.UltimaModifica = New TipoUltimaModifica With {
                .Data = DateTime.Now,
                .OperatoreID = User.Identity.GetUserId(),
                .Operatore = User.Identity.GetUserName()
            }
            db.SaveChanges()
            Return Json(New With {.ok = True, .nuovoStato = Not attuale})
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace
