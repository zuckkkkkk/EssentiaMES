Imports System
Imports System.Web
Imports System.Web.Mvc
Imports Microsoft.AspNet.Identity
Imports Brighetti

Namespace Controllers
    ''' <summary>
    ''' Sincronizzazione giacenze con il gestionale Mexal: stato, esecuzione manuale
    ''' e import manuale di un file CSV.
    ''' </summary>
    <Authorize(Roles:="Admin")>
    Public Class Brighetti_SyncController
        Inherits System.Web.Mvc.Controller

        ' GET: stato e comandi.
        Function Index() As ActionResult
            ViewBag.Abilitato = ImpostazioniService.LeggiBool(ChiaviImpostazioni.SyncMexalAbilitato)
            ViewBag.Percorso = ImpostazioniService.LeggiTesto(ChiaviImpostazioni.SyncMexalPercorsoFile, "")
            ViewBag.Ora = ImpostazioniService.LeggiTesto(ChiaviImpostazioni.SchedulerOraSyncGiornaliera, "02:00")
            Return View()
        End Function

        ' POST: esegue subito la sincronizzazione dal file configurato.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function EseguiOra() As ActionResult
            Dim esito = MexalSyncService.SincronizzaDaConfigurazione(
                User.Identity.GetUserId(), User.Identity.GetUserName(), rispettaToggle:=False)
            TempData("Messaggio") = esito.Messaggio
            Return RedirectToAction("Index")
        End Function

        ' POST: importa un file CSV caricato manualmente.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Importa(ByVal file As HttpPostedFileBase) As ActionResult
            If file Is Nothing OrElse file.ContentLength = 0 Then
                TempData("Messaggio") = "Nessun file selezionato."
                Return RedirectToAction("Index")
            End If
            Try
                Dim separatore = ImpostazioniService.LeggiTesto(ChiaviImpostazioni.SyncMexalSeparatore, ";")
                Dim righe = MexalCsvSource.Leggi(file.InputStream, separatore)
                Dim esito = MexalSyncService.Sincronizza(
                    New MexalListaSource(righe), User.Identity.GetUserId(), User.Identity.GetUserName())
                TempData("Messaggio") = esito.Messaggio
            Catch ex As Exception
                TempData("Messaggio") = "Errore import: " & ex.Message
            End Try
            Return RedirectToAction("Index")
        End Function
    End Class
End Namespace
