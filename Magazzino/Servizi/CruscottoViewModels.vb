Imports System.Collections.Generic

''' <summary>Dati del Centro di Controllo (cruscotto): cosa richiede attenzione o è fermo.</summary>
Public Class CruscottoViewModel
    ' KPI "da tenere d'occhio"
    Public Property ProposteAperte As Integer
    Public Property OrdiniConfermati As Integer
    Public Property AttivitaStandBy As Integer
    Public Property AttivitaBloccate As Integer
    Public Property LottiNonRitornati As Integer
    Public Property GiacenzeScoperte As Integer
    Public Property OdpFermi As Integer

    ' Flusso di produzione: attività attive per reparto
    Public Property Flusso As List(Of CruscottoRepartoViewModel)
    Public Property FlussoMax As Integer

    ' Tabelle azionabili
    Public Property AttivitaFerme As List(Of Brighetti_Attività)
    Public Property Scoperte As List(Of Brighetti_Giacenza)
    Public Property LottiAperti As List(Of Brighetti_Lotti)
End Class

''' <summary>Una tappa del flusso (reparto) col numero di attività attive.</summary>
Public Class CruscottoRepartoViewModel
    Public Property NomeReparto As String
    Public Property Conteggio As Integer
End Class
