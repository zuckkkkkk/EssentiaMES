Imports System
Imports Hangfire

''' <summary>
''' Configurazione dello scheduler (Hangfire) per gli automatismi pianificati.
''' Viene avviato da Global.asax (Application_Start) in modo protetto: se il DB
''' non è ancora pronto o Hangfire non parte, l'applicazione web continua a
''' funzionare normalmente (gli automatismi restano comunque attivabili a mano).
'''
''' I job leggono gli interruttori a ogni esecuzione, quindi restano inerti
''' finché l'automatismo corrispondente non viene attivato dal pannello.
''' </summary>
Public Class SchedulerAutomazioni

    Private Shared _server As BackgroundJobServer

    ''' <summary>Inizializza storage, server e job ricorrenti. Idempotente e protetta.</summary>
    Public Shared Sub Configura()
        Try
            ' Riusa la connection string dell'applicazione ("BrighettiModels").
            GlobalConfiguration.Configuration.UseSqlServerStorage("BrighettiModels")
            _server = New BackgroundJobServer()

            ' Job: generazione proposte di riordino (rispetta l'interruttore dedicato).
            RecurringJob.AddOrUpdate("ordini-automatici",
                                     Sub() SchedulerAutomazioni.JobOrdiniAutomatici(),
                                     CronOrdiniAutomatici())
        Catch ex As Exception
            Try
                Using db As New BrighettiModels
                    db.Log.Add(New Log With {
                        .Livello = TipoLogLivello.Warning,
                        .Indirizzo = "SchedulerAutomazioni/Configura",
                        .Messaggio = "Hangfire non inizializzato: " & ex.Message,
                        .Dati = "",
                        .UltimaModifica = New TipoUltimaModifica With {.Data = DateTime.Now, .Operatore = "Sistema"}
                    })
                    db.SaveChanges()
                End Using
            Catch
                ' Ignora: in fase di avvio il DB potrebbe non essere disponibile.
            End Try
        End Try
    End Sub

    ''' <summary>Job ricorrente: genera le proposte di riordino se l'automatismo è attivo.</summary>
    Public Shared Sub JobOrdiniAutomatici()
        OrdiniAutomaticiService.GeneraProposte("", "Scheduler", rispettaToggle:=True, origine:="Pianificato")
    End Sub

    ''' <summary>Espressione cron per il controllo scorte (default ogni 60 minuti).</summary>
    Private Shared Function CronOrdiniAutomatici() As String
        Dim minuti As Integer = 60
        Try
            minuti = CInt(ImpostazioniService.LeggiNumero(ChiaviImpostazioni.SchedulerMinutiIntervalloOrdini, 60))
        Catch
            minuti = 60
        End Try
        If minuti < 1 Then minuti = 60
        If minuti >= 60 Then Return "0 * * * *" ' ogni ora allo scoccare
        Return "*/" & minuti.ToString() & " * * * *"
    End Function
End Class
