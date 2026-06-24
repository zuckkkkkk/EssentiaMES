Imports System
Imports System.Collections.Generic
Imports System.Data.Entity
Imports System.Linq

''' <summary>
''' Motore di generazione delle PROPOSTE di riordino automatico al raggiungimento
''' della scorta minima. Le proposte non inviano nulla: l'amministratore le rivede,
''' conferma o annulla. Tutto è governato dagli interruttori del pannello Automazioni.
''' </summary>
Public Class OrdiniAutomaticiService

    ''' <summary>Esito di una generazione di proposte.</summary>
    Public Class Esito
        Public Property Generate As Integer
        Public Property Saltate As Integer
        Public Property Messaggio As String
    End Class

    ''' <summary>
    ''' Analizza le giacenze e crea proposte di riordino per gli articoli sotto scorta minima.
    ''' </summary>
    ''' <param name="rispettaToggle">
    ''' Se True (job pianificato) la generazione avviene solo quando l'interruttore
    ''' "ordini_automatici.abilitato" è attivo. Se False (esecuzione manuale dall'admin)
    ''' viene eseguita comunque.
    ''' </param>
    Public Shared Function GeneraProposte(operatoreId As String, operatoreNome As String,
                                          Optional rispettaToggle As Boolean = False,
                                          Optional origine As String = "Manuale") As Esito
        Dim esito As New Esito With {.Generate = 0, .Saltate = 0}

        If rispettaToggle AndAlso Not ImpostazioniService.LeggiBool(ChiaviImpostazioni.OrdiniAutomaticiAbilitato) Then
            esito.Messaggio = "Automatismo ordini automatici disattivato."
            Return esito
        End If

        ' Parametri configurabili dal pannello Automazioni.
        Dim prefissi = LeggiPrefissi()
        Dim moltiplicatore As Decimal = CDec(ImpostazioniService.LeggiNumero(ChiaviImpostazioni.OrdiniAutomaticiMoltiplicatore, 2))
        If moltiplicatore <= 0 Then moltiplicatore = 2
        Dim includiSemilavorati = ImpostazioniService.LeggiBool(ChiaviImpostazioni.OrdiniAutomaticiIncludiSemilavorati, True)

        Using db As New BrighettiModels
            ' Magazzini conto-lavoro: usati per distinguere i semilavorati.
            Dim magazziniContoLavoro = db.Brighetti_Magazzini _
                .Where(Function(m) m.TipologiaMagazzino = TipoMagazzino.Magazzino_ContoLavoro) _
                .Select(Function(m) m.CodiceMagazzino).ToList()

            Dim giacenze = db.Brighetti_Giacenze.ToList()
            For Each g In giacenze
                ' Solo articoli con un punto di riordino definito e sotto soglia.
                If g.QuantitàSottoscorta <= 0 Then Continue For
                If (g.QuantitàGiacenza + g.InPrevisioneEntrata) > g.QuantitàSottoscorta Then Continue For

                ' Filtro per prefisso codice (es. solo "G8" in fase di test).
                If prefissi.Count > 0 Then
                    Dim codice = If(g.CodiceArticolo, "")
                    If Not prefissi.Any(Function(p) codice.StartsWith(p, StringComparison.OrdinalIgnoreCase)) Then Continue For
                End If

                ' Esclusione semilavorati (se richiesto): giacenze nei magazzini conto-lavoro.
                If Not includiSemilavorati AndAlso magazziniContoLavoro.Contains(g.CodiceMagazzino) Then
                    esito.Saltate += 1
                    Continue For
                End If

                ' Evita doppioni: salta se esiste già un ordine NON terminato per lo stesso
                ' articolo/magazzino (Proposto, Confermato o Inviato). Così un ordine confermato
                ' ma non ancora arrivato non viene riproposto a ogni giro. Si riproporrà solo
                ' dopo che sarà stato segnato "Evaso" (o annullato).
                Dim giaInCorso = db.Brighetti_OrdiniAutomatici.Any(
                    Function(o) o.CodiceArticolo = g.CodiceArticolo _
                            AndAlso o.CodiceMagazzino = g.CodiceMagazzino _
                            AndAlso (o.Stato = StatoOrdineAutomatico.Proposto _
                                  OrElse o.Stato = StatoOrdineAutomatico.Confermato _
                                  OrElse o.Stato = StatoOrdineAutomatico.Inviato))
                If giaInCorso Then
                    esito.Saltate += 1
                    Continue For
                End If

                ' Obiettivo di riordino: scorta massima se impostata, altrimenti multiplo della minima.
                Dim obiettivo As Decimal = If(g.QuantitàScortaMassima > 0,
                                              g.QuantitàScortaMassima,
                                              g.QuantitàSottoscorta * moltiplicatore)
                Dim fabbisogno As Decimal = obiettivo - (g.QuantitàGiacenza + g.InPrevisioneEntrata)
                If fabbisogno <= 0 Then Continue For

                ' Arrotondamento al lotto minimo dell'articolo.
                Dim art = db.Brighetti_Articoli.FirstOrDefault(Function(a) a.CodiceArticolo = g.CodiceArticolo)
                Dim lottoMin As Integer = If(art IsNot Nothing, art.LottoMinimo, 0)
                Dim quantitaProposta As Decimal = fabbisogno
                If lottoMin > 0 Then
                    quantitaProposta = Math.Ceiling(fabbisogno / lottoMin) * lottoMin
                End If

                db.Brighetti_OrdiniAutomatici.Add(New Brighetti_OrdineAutomatico With {
                    .CodiceArticolo = g.CodiceArticolo,
                    .CodiceMagazzino = g.CodiceMagazzino,
                    .QuantitàGiacenza = g.QuantitàGiacenza,
                    .ScortaMinima = g.QuantitàSottoscorta,
                    .ScortaMassima = g.QuantitàScortaMassima,
                    .LottoMinimo = lottoMin,
                    .QuantitàProposta = quantitaProposta,
                    .Stato = StatoOrdineAutomatico.Proposto,
                    .Origine = origine,
                    .Note = "",
                    .DataGenerazione = DateTime.Now,
                    .UltimaModifica = New TipoUltimaModifica With {
                        .Data = DateTime.Now, .OperatoreID = operatoreId, .Operatore = operatoreNome
                    }
                })
                esito.Generate += 1
            Next

            If esito.Generate > 0 Then db.SaveChanges()

            ' Tracciatura nel log applicativo.
            db.Log.Add(New Log With {
                .Livello = TipoLogLivello.Info,
                .Indirizzo = "OrdiniAutomaticiService/GeneraProposte",
                .Messaggio = "Proposte di riordino generate: " & esito.Generate & " (saltate: " & esito.Saltate & ", origine: " & origine & ")",
                .Dati = "",
                .UltimaModifica = New TipoUltimaModifica With {
                    .Data = DateTime.Now, .OperatoreID = operatoreId, .Operatore = operatoreNome
                }
            })
            db.SaveChanges()
        End Using

        esito.Messaggio = "Generate " & esito.Generate & " proposte di riordino."
        Return esito
    End Function

    ''' <summary>Legge la lista dei prefissi codice dal pannello (vuota = tutti gli articoli).</summary>
    Private Shared Function LeggiPrefissi() As List(Of String)
        Dim grezzo = ImpostazioniService.LeggiTesto(ChiaviImpostazioni.OrdiniAutomaticiPrefissi, "")
        If String.IsNullOrWhiteSpace(grezzo) Then Return New List(Of String)
        Return grezzo.Split(","c) _
            .Select(Function(p) p.Trim()) _
            .Where(Function(p) p.Length > 0) _
            .ToList()
    End Function
End Class
