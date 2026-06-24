Imports System
Imports System.Collections.Generic
Imports System.Data.Entity
Imports System.Linq

''' <summary>
''' Automatismi sui lotti: creazione automatica del lotto alla chiusura di una fase
''' e sblocco della fase successiva quando un lotto torna ("Ritornato").
''' Entrambi sono governati dai rispettivi interruttori del pannello Automazioni.
''' </summary>
Public Class LottiAutomaticiService

    ''' <summary>
    ''' Crea un lotto (stato "In attesa") per l'articolo/quantità appena versato.
    ''' Ritorna l'IdLotto creato, oppure 0 se l'automatismo è spento.
    ''' </summary>
    Public Shared Function CreaLottoDaVersamento(codiceArticolo As String, quantita As Decimal,
                                                 operatoreId As String, operatoreNome As String) As Integer
        If Not ImpostazioniService.LeggiBool(ChiaviImpostazioni.LottiCreazioneAutomaticaAbilitato) Then Return 0
        If String.IsNullOrWhiteSpace(codiceArticolo) Then Return 0

        Using db As New BrighettiModels
            Dim art = db.Brighetti_Articoli.FirstOrDefault(Function(a) a.CodiceArticolo = codiceArticolo)
            Dim idArticolo As Integer = If(art IsNot Nothing, art.Id, 0)

            Dim lotto As New Brighetti_Lotti With {
                .NomeLotto = "AUTO " & codiceArticolo & " " & DateTime.Now.ToString("yyyyMMdd_HHmm"),
                .DescrizioneLotto = "Lotto generato automaticamente alla chiusura della fase.",
                .StatoLotto = StatoLotto.In_Attesa,
                .TipologiaLotto = "Automatico",
                .CodiceADLACL = "",
                .UltimaModifica = New TipoUltimaModifica With {
                    .Data = DateTime.Now, .OperatoreID = operatoreId, .Operatore = operatoreNome
                }
            }
            db.Brighetti_Lotti.Add(lotto)
            db.SaveChanges()

            db.Brighetti_Lotti_Articoli.Add(New Brighetti_Lotti_Articoli With {
                .IdLotto = lotto.IdLotto,
                .IdArticolo = idArticolo,
                .NomeArticolo = codiceArticolo,
                .QuantitàArticolo = quantita.ToString(),
                .NoteArticolo = "",
                .StatoArticoloLotto = StatoArticoloLotto.In_Attesa,
                .UltimaModifica = New TipoUltimaModifica With {
                    .Data = DateTime.Now, .OperatoreID = operatoreId, .Operatore = operatoreNome
                }
            })
            db.SaveChanges()

            db.Log.Add(New Log With {
                .Livello = TipoLogLivello.Info,
                .Indirizzo = "LottiAutomaticiService/CreaLottoDaVersamento",
                .Messaggio = "Lotto automatico creato: " & lotto.NomeLotto & " (id " & lotto.IdLotto & ")",
                .Dati = "",
                .UltimaModifica = New TipoUltimaModifica With {
                    .Data = DateTime.Now, .OperatoreID = operatoreId, .Operatore = operatoreNome
                }
            })
            db.SaveChanges()

            Return lotto.IdLotto
        End Using
    End Function

    ''' <summary>
    ''' Quando un lotto passa a "Ritornato", sblocca la fase successiva per gli articoli
    ''' del lotto. Per sicurezza agisce solo quando il blocco è univoco (una sola attività
    ''' bloccata per quel codice articolo): in caso di ambiguità non fa nulla e lo annota a log.
    ''' Ritorna il numero di attività sbloccate.
    ''' </summary>
    Public Shared Function SbloccaFaseDaLottoRitornato(idLotto As Integer,
                                                       operatoreId As String, operatoreNome As String) As Integer
        If Not ImpostazioniService.LeggiBool(ChiaviImpostazioni.LottiRitornatoSbloccaFase) Then Return 0
        Dim sbloccate As Integer = 0

        Using db As New BrighettiModels
            Dim articoli = db.Brighetti_Lotti_Articoli.Where(Function(x) x.IdLotto = idLotto).ToList()
            For Each la In articoli
                ' Ricava il codice articolo (preferendo l'anagrafica, fallback sul nome salvato).
                Dim codice As String = la.NomeArticolo
                Dim art = db.Brighetti_Articoli.Find(la.IdArticolo)
                If art IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(art.CodiceArticolo) Then codice = art.CodiceArticolo
                If String.IsNullOrWhiteSpace(codice) Then Continue For

                Dim bloccate = db.Brighetti_Attività.Where(
                    Function(x) x.CodiceArticolo = codice _
                            AndAlso x.StatoAttività = TipoStatoAttività.BloccoDaAttivitàPrecedente).ToList()

                If bloccate.Count = 1 Then
                    bloccate(0).StatoAttività = TipoStatoAttività.In_attesa
                    bloccate(0).UltimaModifica = New TipoUltimaModifica With {
                        .Data = DateTime.Now, .OperatoreID = operatoreId, .Operatore = operatoreNome
                    }
                    sbloccate += 1
                ElseIf bloccate.Count > 1 Then
                    db.Log.Add(New Log With {
                        .Livello = TipoLogLivello.Warning,
                        .Indirizzo = "LottiAutomaticiService/SbloccaFaseDaLottoRitornato",
                        .Messaggio = "Sblocco non univoco per articolo " & codice & " (lotto " & idLotto & "): " & bloccate.Count & " attività bloccate. Nessuno sblocco automatico.",
                        .Dati = "",
                        .UltimaModifica = New TipoUltimaModifica With {.Data = DateTime.Now, .OperatoreID = operatoreId, .Operatore = operatoreNome}
                    })
                End If
            Next
            db.SaveChanges()
        End Using

        Return sbloccate
    End Function
End Class
