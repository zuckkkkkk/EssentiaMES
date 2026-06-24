Imports System
Imports System.Data.Entity
Imports System.Linq

''' <summary>
''' Aggiorna le giacenze interne (Brighetti_Giacenze) a partire da una sorgente
''' Mexal. Le giacenze esistenti vengono aggiornate, quelle nuove create.
''' Pensato per la sincronizzazione giornaliera, ma usabile anche on-demand.
''' </summary>
Public Class MexalSyncService

    Public Class Esito
        Public Property Aggiornate As Integer
        Public Property Create As Integer
        Public Property Lette As Integer
        Public Property Messaggio As String
        Public Property Ok As Boolean
    End Class

    ''' <summary>Sincronizza usando la configurazione del pannello (file CSV).</summary>
    Public Shared Function SincronizzaDaConfigurazione(operatoreId As String, operatoreNome As String,
                                                       Optional rispettaToggle As Boolean = False) As Esito
        If rispettaToggle AndAlso Not ImpostazioniService.LeggiBool(ChiaviImpostazioni.SyncMexalAbilitato) Then
            Return New Esito With {.Ok = False, .Messaggio = "Sincronizzazione Mexal disattivata."}
        End If
        Dim percorso = ImpostazioniService.LeggiTesto(ChiaviImpostazioni.SyncMexalPercorsoFile, "")
        Dim separatore = ImpostazioniService.LeggiTesto(ChiaviImpostazioni.SyncMexalSeparatore, ";")
        Return Sincronizza(New MexalCsvSource(percorso, separatore), operatoreId, operatoreNome)
    End Function

    ''' <summary>Sincronizza da una sorgente qualsiasi (CSV, upload, in futuro API).</summary>
    Public Shared Function Sincronizza(sorgente As IMexalSource, operatoreId As String, operatoreNome As String) As Esito
        Dim esito As New Esito With {.Aggiornate = 0, .Create = 0, .Lette = 0, .Ok = False}
        Try
            Dim righe = sorgente.LeggiGiacenze()
            esito.Lette = righe.Count
            Using db As New BrighettiModels
                For Each dto In righe
                    Dim g = db.Brighetti_Giacenze.FirstOrDefault(
                        Function(x) x.CodiceArticolo = dto.CodiceArticolo AndAlso x.CodiceMagazzino = dto.CodiceMagazzino)
                    If g Is Nothing Then
                        db.Brighetti_Giacenze.Add(New Brighetti_Giacenza With {
                            .CodiceArticolo = dto.CodiceArticolo,
                            .CodiceMagazzino = dto.CodiceMagazzino,
                            .QuantitàGiacenza = dto.Quantita,
                            .UltimaModifica = New TipoUltimaModifica With {
                                .Data = DateTime.Now, .OperatoreID = operatoreId, .Operatore = operatoreNome
                            }
                        })
                        esito.Create += 1
                    Else
                        g.QuantitàGiacenza = dto.Quantita
                        g.UltimaModifica = New TipoUltimaModifica With {
                            .Data = DateTime.Now, .OperatoreID = operatoreId, .Operatore = operatoreNome
                        }
                        esito.Aggiornate += 1
                    End If
                Next
                db.SaveChanges()

                db.Log.Add(New Log With {
                    .Livello = TipoLogLivello.Info,
                    .Indirizzo = "MexalSyncService/Sincronizza",
                    .Messaggio = "Sync Mexal: lette " & esito.Lette & ", aggiornate " & esito.Aggiornate & ", create " & esito.Create,
                    .Dati = "",
                    .UltimaModifica = New TipoUltimaModifica With {
                        .Data = DateTime.Now, .OperatoreID = operatoreId, .Operatore = operatoreNome
                    }
                })
                db.SaveChanges()
            End Using
            esito.Ok = True
            esito.Messaggio = "Sincronizzazione completata: " & esito.Aggiornate & " aggiornate, " & esito.Create & " create (su " & esito.Lette & " righe lette)."
        Catch ex As Exception
            esito.Ok = False
            esito.Messaggio = "Errore sincronizzazione Mexal: " & ex.Message
            Try
                Using db As New BrighettiModels
                    db.Log.Add(New Log With {
                        .Livello = TipoLogLivello.Errors,
                        .Indirizzo = "MexalSyncService/Sincronizza",
                        .Messaggio = esito.Messaggio,
                        .Dati = "",
                        .UltimaModifica = New TipoUltimaModifica With {
                            .Data = DateTime.Now, .OperatoreID = operatoreId, .Operatore = operatoreNome
                        }
                    })
                    db.SaveChanges()
                End Using
            Catch
            End Try
        End Try
        Return esito
    End Function
End Class
