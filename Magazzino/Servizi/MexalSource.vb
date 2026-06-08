Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports Microsoft.VisualBasic.FileIO

''' <summary>Riga di giacenza letta dal gestionale Mexal.</summary>
Public Class MexalGiacenzaDto
    Public Property CodiceArticolo As String
    Public Property CodiceMagazzino As String
    Public Property Quantita As Decimal
End Class

''' <summary>
''' Astrazione della sorgente dati Mexal. Oggi è implementata via file CSV; quando
''' sarà disponibile l'integrazione definitiva (es. API REST o lettura diretta del
''' DB Mexal) basterà aggiungere una nuova implementazione di questa interfaccia
''' senza toccare il resto del codice (MexalSyncService).
''' </summary>
Public Interface IMexalSource
    Function LeggiGiacenze() As List(Of MexalGiacenzaDto)
End Interface

''' <summary>Sorgente in memoria: avvolge righe già lette (es. da upload manuale).</summary>
Public Class MexalListaSource
    Implements IMexalSource

    Private ReadOnly _righe As List(Of MexalGiacenzaDto)

    Public Sub New(righe As List(Of MexalGiacenzaDto))
        _righe = righe
    End Sub

    Public Function LeggiGiacenze() As List(Of MexalGiacenzaDto) Implements IMexalSource.LeggiGiacenze
        Return _righe
    End Function
End Class

''' <summary>
''' Sorgente Mexal basata su file CSV (es. export notturno del gestionale).
''' Le colonne sono individuate dall'intestazione in modo tollerante:
''' articolo, magazzino, giacenza/quantità.
''' </summary>
Public Class MexalCsvSource
    Implements IMexalSource

    Private ReadOnly _percorso As String
    Private ReadOnly _separatore As String

    Public Sub New(percorso As String, separatore As String)
        _percorso = percorso
        _separatore = If(String.IsNullOrEmpty(separatore), ";", separatore)
    End Sub

    Public Function LeggiGiacenze() As List(Of MexalGiacenzaDto) Implements IMexalSource.LeggiGiacenze
        If String.IsNullOrWhiteSpace(_percorso) Then
            Throw New Exception("Percorso del file CSV Mexal non configurato.")
        End If
        If Not File.Exists(_percorso) Then
            Throw New Exception("File CSV Mexal non trovato: " & _percorso)
        End If
        Using stream As New FileStream(_percorso, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Return Leggi(stream)
        End Using
    End Function

    ''' <summary>Parsing condiviso (usato anche dall'upload manuale del file).</summary>
    Public Shared Function Leggi(stream As Stream, Optional separatore As String = ";") As List(Of MexalGiacenzaDto)
        Dim risultato As New List(Of MexalGiacenzaDto)
        Using parser As New TextFieldParser(stream)
            parser.TextFieldType = FieldType.Delimited
            parser.SetDelimiters(If(String.IsNullOrEmpty(separatore), ";", separatore))
            parser.HasFieldsEnclosedInQuotes = True
            parser.TrimWhiteSpace = True

            If parser.EndOfData Then Return risultato
            Dim header = parser.ReadFields()
            Dim idxArt = TrovaColonna(header, {"codicearticolo", "articolo", "codart", "cod_articolo", "codice articolo"})
            Dim idxMag = TrovaColonna(header, {"codicemagazzino", "magazzino", "codmag", "cod_magazzino", "codice magazzino", "deposito"})
            Dim idxQta = TrovaColonna(header, {"giacenza", "quantita", "quantità", "qta", "qty", "esistenza", "quantitagiacenza"})

            If idxArt < 0 OrElse idxQta < 0 Then
                Throw New Exception("Intestazione CSV non riconosciuta: servono almeno le colonne 'Articolo' e 'Giacenza/Quantità'.")
            End If

            While Not parser.EndOfData
                Dim campi = parser.ReadFields()
                If campi Is Nothing OrElse campi.Length = 0 Then Continue While
                If idxArt >= campi.Length Then Continue While

                Dim dto As New MexalGiacenzaDto With {
                    .CodiceArticolo = campi(idxArt),
                    .CodiceMagazzino = If(idxMag >= 0 AndAlso idxMag < campi.Length, campi(idxMag), ""),
                    .Quantita = ParseDecimal(If(idxQta < campi.Length, campi(idxQta), "0"))
                }
                If Not String.IsNullOrWhiteSpace(dto.CodiceArticolo) Then risultato.Add(dto)
            End While
        End Using
        Return risultato
    End Function

    Private Shared Function TrovaColonna(header As String(), candidati As String()) As Integer
        For i = 0 To header.Length - 1
            Dim h = If(header(i), "").Trim().ToLowerInvariant().Replace(" ", "")
            For Each c In candidati
                If h = c.Replace(" ", "") Then Return i
            Next
        Next
        Return -1
    End Function

    Private Shared Function ParseDecimal(valore As String) As Decimal
        If String.IsNullOrWhiteSpace(valore) Then Return 0
        Dim r As Decimal
        If Decimal.TryParse(valore, NumberStyles.Any, CultureInfo.InvariantCulture, r) Then Return r
        If Decimal.TryParse(valore.Replace(".", "").Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, r) Then Return r
        Return 0
    End Function
End Class
