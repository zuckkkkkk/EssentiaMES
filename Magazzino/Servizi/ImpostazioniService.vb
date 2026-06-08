Imports System
Imports System.Collections.Generic
Imports System.Data.Entity
Imports System.Globalization
Imports System.Linq

''' <summary>
''' Chiavi canoniche delle impostazioni / interruttori (feature toggle) degli automatismi.
''' Centralizzare le chiavi qui evita stringhe "magiche" sparse nel codice.
''' </summary>
Public Module ChiaviImpostazioni
    ' --- Ordini automatici (riordino su sottoscorta) ---
    Public Const OrdiniAutomaticiAbilitato As String = "ordini_automatici.abilitato"
    Public Const OrdiniAutomaticiPrefissi As String = "ordini_automatici.prefissi_codice"
    Public Const OrdiniAutomaticiMoltiplicatore As String = "ordini_automatici.moltiplicatore_scorta_minima"
    Public Const OrdiniAutomaticiIncludiSemilavorati As String = "ordini_automatici.includi_semilavorati"

    ' --- Sincronizzazione gestionale Mexal ---
    Public Const SyncMexalAbilitato As String = "sync_mexal.abilitato"
    Public Const SyncMexalPercorsoFile As String = "sync_mexal.percorso_file_csv"
    Public Const SyncMexalSeparatore As String = "sync_mexal.separatore_csv"

    ' --- Gestione lotti automatica ---
    Public Const LottiCreazioneAutomaticaAbilitato As String = "lotti.creazione_automatica_abilitato"
    Public Const LottiRitornatoSbloccaFase As String = "lotti.ritornato_sblocca_fase_abilitato"

    ' --- Pianificazione (scheduler Hangfire) ---
    Public Const SchedulerOraSyncGiornaliera As String = "scheduler.ora_sync_giornaliera"
    Public Const SchedulerMinutiIntervalloOrdini As String = "scheduler.minuti_intervallo_ordini"
End Module

''' <summary>
''' Lettura/scrittura tipizzata delle impostazioni e degli interruttori degli automatismi.
''' Ogni metodo apre un contesto a vita breve così da poter essere chiamato anche
''' dai job in background senza condividere il DbContext del controller.
''' </summary>
Public Class ImpostazioniService

    ''' <summary>Legge un interruttore (true/false). Ritorna defaultValue se assente o non valido.</summary>
    Public Shared Function LeggiBool(chiave As String, Optional defaultValue As Boolean = False) As Boolean
        Dim valore = LeggiValoreGrezzo(chiave)
        If String.IsNullOrWhiteSpace(valore) Then Return defaultValue
        Dim r As Boolean
        If Boolean.TryParse(valore, r) Then Return r
        valore = valore.Trim().ToLowerInvariant()
        Return valore = "1" OrElse valore = "true" OrElse valore = "on" OrElse valore = "si" OrElse valore = "sì"
    End Function

    ''' <summary>Legge un valore testuale. Ritorna defaultValue se assente.</summary>
    Public Shared Function LeggiTesto(chiave As String, Optional defaultValue As String = "") As String
        Dim valore = LeggiValoreGrezzo(chiave)
        If valore Is Nothing Then Return defaultValue
        Return valore
    End Function

    ''' <summary>Legge un valore numerico (cultura invariante). Ritorna defaultValue se assente o non valido.</summary>
    Public Shared Function LeggiNumero(chiave As String, Optional defaultValue As Double = 0) As Double
        Dim valore = LeggiValoreGrezzo(chiave)
        If String.IsNullOrWhiteSpace(valore) Then Return defaultValue
        Dim r As Double
        If Double.TryParse(valore, NumberStyles.Any, CultureInfo.InvariantCulture, r) Then Return r
        If Double.TryParse(valore.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, r) Then Return r
        Return defaultValue
    End Function

    Private Shared Function LeggiValoreGrezzo(chiave As String) As String
        Using db As New BrighettiModels
            Dim imp = db.Brighetti_Impostazioni.FirstOrDefault(Function(x) x.Chiave = chiave)
            If imp Is Nothing Then Return Nothing
            Return imp.Valore
        End Using
    End Function

    ''' <summary>Inserisce o aggiorna il valore di un'impostazione.</summary>
    Public Shared Sub Scrivi(chiave As String, valore As String, operatoreId As String, operatoreNome As String)
        Using db As New BrighettiModels
            Dim imp = db.Brighetti_Impostazioni.FirstOrDefault(Function(x) x.Chiave = chiave)
            If imp Is Nothing Then
                imp = New Brighetti_Impostazione With {
                    .Chiave = chiave,
                    .Tipo = TipoImpostazione.Testo,
                    .Categoria = "Generale"
                }
                db.Brighetti_Impostazioni.Add(imp)
            End If
            imp.Valore = valore
            imp.UltimaModifica = New TipoUltimaModifica With {
                .Data = DateTime.Now,
                .OperatoreID = operatoreId,
                .Operatore = operatoreNome
            }
            db.SaveChanges()
        End Using
    End Sub

    ''' <summary>
    ''' Crea le impostazioni di default mancanti (idempotente). Tutti gli automatismi
    ''' nascono SPENTI: si attivano poi uno alla volta dal pannello Impostazioni.
    ''' </summary>
    Public Shared Sub AssicuraDefault()
        Using db As New BrighettiModels
            Dim esistenti = db.Brighetti_Impostazioni.Select(Function(x) x.Chiave).ToList()
            Dim aggiunte = False
            For Each def In ElencoDefault()
                If Not esistenti.Contains(def.Chiave) Then
                    db.Brighetti_Impostazioni.Add(def)
                    aggiunte = True
                End If
            Next
            If aggiunte Then db.SaveChanges()
        End Using
    End Sub

    ''' <summary>Elenco canonico delle impostazioni con valori di default (automatismi spenti).</summary>
    Public Shared Function ElencoDefault() As List(Of Brighetti_Impostazione)
        Return New List(Of Brighetti_Impostazione) From {
            Nuova(ChiaviImpostazioni.OrdiniAutomaticiAbilitato, "false", TipoImpostazione.Booleano, "Ordini Automatici",
                  "Abilita la generazione automatica delle proposte di riordino al raggiungimento della scorta minima."),
            Nuova(ChiaviImpostazioni.OrdiniAutomaticiPrefissi, "G8", TipoImpostazione.Testo, "Ordini Automatici",
                  "Prefissi dei codici articolo coinvolti, separati da virgola (es. G8,G12). Lasciare vuoto per tutti i codici."),
            Nuova(ChiaviImpostazioni.OrdiniAutomaticiMoltiplicatore, "2", TipoImpostazione.Numero, "Ordini Automatici",
                  "Se l'articolo non ha una scorta massima, riordina fino a questo multiplo della scorta minima (es. 2 = doppio della scorta minima)."),
            Nuova(ChiaviImpostazioni.OrdiniAutomaticiIncludiSemilavorati, "true", TipoImpostazione.Booleano, "Ordini Automatici",
                  "Genera proposte di riordino anche per i semilavorati, non solo per i prodotti finiti."),
            Nuova(ChiaviImpostazioni.SyncMexalAbilitato, "false", TipoImpostazione.Booleano, "Sincronizzazione Mexal",
                  "Abilita l'aggiornamento giornaliero delle giacenze dal gestionale Mexal."),
            Nuova(ChiaviImpostazioni.SyncMexalPercorsoFile, "", TipoImpostazione.Testo, "Sincronizzazione Mexal",
                  "Percorso del file CSV esportato da Mexal con le giacenze (es. \\server\export\giacenze.csv)."),
            Nuova(ChiaviImpostazioni.SyncMexalSeparatore, ";", TipoImpostazione.Testo, "Sincronizzazione Mexal",
                  "Carattere separatore delle colonne nel file CSV (di norma ; oppure ,)."),
            Nuova(ChiaviImpostazioni.LottiCreazioneAutomaticaAbilitato, "false", TipoImpostazione.Booleano, "Gestione Lotti",
                  "Crea automaticamente un lotto alla chiusura di una fase di versamento."),
            Nuova(ChiaviImpostazioni.LottiRitornatoSbloccaFase, "false", TipoImpostazione.Booleano, "Gestione Lotti",
                  "Quando un lotto passa allo stato 'Ritornato', sblocca automaticamente la fase di lavorazione successiva."),
            Nuova(ChiaviImpostazioni.SchedulerOraSyncGiornaliera, "02:00", TipoImpostazione.Testo, "Pianificazione",
                  "Ora (HH:mm) della sincronizzazione giornaliera con Mexal."),
            Nuova(ChiaviImpostazioni.SchedulerMinutiIntervalloOrdini, "60", TipoImpostazione.Numero, "Pianificazione",
                  "Ogni quanti minuti il sistema controlla le scorte per generare le proposte di riordino.")
        }
    End Function

    Private Shared Function Nuova(chiave As String, valore As String, tipo As TipoImpostazione, categoria As String, descrizione As String) As Brighetti_Impostazione
        Return New Brighetti_Impostazione With {
            .Chiave = chiave,
            .Valore = valore,
            .Tipo = tipo,
            .Categoria = categoria,
            .Descrizione = descrizione,
            .UltimaModifica = New TipoUltimaModifica With {.Data = DateTime.Now, .Operatore = "Sistema", .OperatoreID = ""}
        }
    End Function
End Class
