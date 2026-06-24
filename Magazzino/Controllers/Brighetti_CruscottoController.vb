Imports System
Imports System.Collections.Generic
Imports System.Data.Entity
Imports System.Linq
Imports System.Web.Mvc
Imports Brighetti

Namespace Controllers
    ''' <summary>
    ''' Centro di Controllo: vista d'insieme di ciò che richiede attenzione o è fermo
    ''' (proposte di riordino, ordini in arrivo, attività sospese/bloccate, ODP fermi,
    ''' lotti non ritornati, giacenze sotto scorta senza ordine). Serve a non lasciare
    ''' nulla indietro.
    ''' </summary>
    <Authorize(Roles:="Admin")>
    Public Class Brighetti_CruscottoController
        Inherits System.Web.Mvc.Controller

        Private db As New BrighettiModels

        Function Index() As ActionResult
            Dim vm As New CruscottoViewModel()

            ' --- Attività ancora aperte (escludo concluse/annullate) ---
            Dim openAtt = db.Brighetti_Attività.Where(
                Function(a) a.StatoAttività <> TipoStatoAttività.Completato _
                        AndAlso a.StatoAttività <> TipoStatoAttività.Annullato).ToList()

            vm.AttivitaStandBy = openAtt.Count(Function(a) a.StatoAttività = TipoStatoAttività.StandBy)
            vm.AttivitaBloccate = openAtt.Count(Function(a) a.StatoAttività = TipoStatoAttività.BloccoDaAttivitàPrecedente)

            ' ODP fermi: hanno fasi bloccate ma nessuna fase avviabile/in corso.
            Dim stuckOdp = openAtt _
                .GroupBy(Function(a) a.OrdineDiProduzione) _
                .Where(Function(g) g.Any(Function(a) a.StatoAttività = TipoStatoAttività.BloccoDaAttivitàPrecedente) _
                                AndAlso Not g.Any(Function(a) Attiva(a.StatoAttività))) _
                .Select(Function(g) g.Key) _
                .ToList()
            vm.OdpFermi = stuckOdp.Count

            ' Tabella attività ferme: sospese (StandBy) o bloccate dentro un ODP fermo.
            Dim stuckSet As New HashSet(Of String)(stuckOdp.Where(Function(k) k IsNot Nothing))
            vm.AttivitaFerme = openAtt _
                .Where(Function(a) a.StatoAttività = TipoStatoAttività.StandBy _
                                OrElse (a.StatoAttività = TipoStatoAttività.BloccoDaAttivitàPrecedente _
                                        AndAlso a.OrdineDiProduzione IsNot Nothing _
                                        AndAlso stuckSet.Contains(a.OrdineDiProduzione))) _
                .OrderBy(Function(a) a.UltimaModifica.Data) _
                .Take(100).ToList()

            ' --- Flusso di produzione: attività attive per reparto ---
            Dim reparti = db.Brighetti_Reparti.ToList()
            Dim attive = openAtt.Where(Function(a) Attiva(a.StatoAttività)).ToList()
            vm.Flusso = New List(Of CruscottoRepartoViewModel)
            For Each r In reparti.OrderBy(Function(x) x.IdReparto)
                vm.Flusso.Add(New CruscottoRepartoViewModel With {
                    .NomeReparto = r.NomeReparto,
                    .Conteggio = attive.Count(Function(a) a.idReparto = r.IdReparto)
                })
            Next
            vm.FlussoMax = If(vm.Flusso.Count > 0, Math.Max(1, vm.Flusso.Max(Function(x) x.Conteggio)), 1)

            ' --- Ordini automatici ---
            vm.ProposteAperte = db.Brighetti_OrdiniAutomatici.Count(Function(o) o.Stato = StatoOrdineAutomatico.Proposto)
            vm.OrdiniConfermati = db.Brighetti_OrdiniAutomatici.Count(Function(o) o.Stato = StatoOrdineAutomatico.Confermato)

            ' Chiavi articolo|magazzino con un ordine già in corso (non terminato).
            Dim ordiniInCorso = db.Brighetti_OrdiniAutomatici.Where(
                Function(o) o.Stato = StatoOrdineAutomatico.Proposto _
                        OrElse o.Stato = StatoOrdineAutomatico.Confermato _
                        OrElse o.Stato = StatoOrdineAutomatico.Inviato).ToList()
            Dim coperti As New HashSet(Of String)
            For Each o In ordiniInCorso
                coperti.Add(Chiave(o.CodiceArticolo, o.CodiceMagazzino))
            Next

            ' --- Giacenze sotto scorta SENZA ordine in corso (le vere scoperte) ---
            Dim giac = db.Brighetti_Giacenze.Where(Function(x) x.QuantitàSottoscorta > 0).ToList()
            vm.Scoperte = giac _
                .Where(Function(x) (x.QuantitàGiacenza + x.InPrevisioneEntrata) <= x.QuantitàSottoscorta _
                                AndAlso Not coperti.Contains(Chiave(x.CodiceArticolo, x.CodiceMagazzino))) _
                .OrderBy(Function(x) x.QuantitàGiacenza) _
                .Take(100).ToList()
            vm.GiacenzeScoperte = vm.Scoperte.Count

            ' --- Lotti non ancora ritornati ---
            vm.LottiAperti = db.Brighetti_Lotti _
                .Where(Function(l) l.StatoLotto = StatoLotto.In_Attesa OrElse l.StatoLotto = StatoLotto.Inviato) _
                .OrderByDescending(Function(l) l.UltimaModifica.Data) _
                .Take(50).ToList()
            vm.LottiNonRitornati = db.Brighetti_Lotti.Count(Function(l) l.StatoLotto = StatoLotto.In_Attesa OrElse l.StatoLotto = StatoLotto.Inviato)

            Return View(vm)
        End Function

        Private Shared Function Attiva(s As TipoStatoAttività) As Boolean
            Return s = TipoStatoAttività.In_attesa _
                OrElse s = TipoStatoAttività.In_Attrezzaggio _
                OrElse s = TipoStatoAttività.In_Lavorazione _
                OrElse s = TipoStatoAttività.StandBy
        End Function

        Private Shared Function Chiave(codiceArticolo As String, codiceMagazzino As String) As String
            Return If(codiceArticolo, "") & "|" & If(codiceMagazzino, "")
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace
