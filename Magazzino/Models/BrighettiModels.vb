Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Public Class BrighettiModels
    Inherits DbContext

    'Il contesto è stato configurato per utilizzare una stringa di connessione 'EbsModels' dal file di configurazione 
    ' dell'applicazione (App.config o Web.config). Per impostazione predefinita, la stringa di connessione è destinata al 
    ' database 'EbsWebBackOffice.EbsModels' nell'istanza di LocalDb. 
    ' 
    ' Per destinarla a un database o un provider di database differente, modificare la stringa di connessione 'EbsModels' 
    ' nel file di configurazione dell'applicazione.
    Public Sub New()
        MyBase.New("name=BrighettiModels")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
        MyBase.OnModelCreating(modelBuilder)

        'modelBuilder.Entity(Of Contabilizzatore)().[Property](Function(p) p.FattoreK).HasPrecision(12, 6)

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    ' Aggiungere DbSet per ogni tipo di entità che si desidera includere nel modello. Per ulteriori informazioni 
    ' sulla configurazione e sull'utilizzo di un modello Code, vedere http://go.microsoft.com/fwlink/?LinkId=390109.
    ' Public Overridable Property MyEntities() As DbSet(Of MyEntity)
    '===================================================================== DA RIMUOVERE
    Public Overridable Property Magazzino() As DbSet(Of Magazzino)
    Public Overridable Property Articoli() As DbSet(Of Articoli)
    Public Overridable Property Ordini() As DbSet(Of Ordini)
    Public Overridable Property OrdiniInCorso() As DbSet(Of OrdiniInCorso)
    Public Overridable Property Macchine() As DbSet(Of Macchine)
    Public Overridable Property DatiMacchina() As DbSet(Of DatiMacchina)
    'Public Overridable Property TempiCAD() As DbSet(Of TempiCAD)
    '===================================================================== DA RIMUOVERE

    'NUOVO MODELLO
    Public Overridable Property Brighetti_Articoli() As DbSet(Of Brighetti_Articolo)
    Public Overridable Property Brighetti_Magazzini() As DbSet(Of Brighetti_Magazzino)
    Public Overridable Property Brighetti_Tempi() As DbSet(Of Brighetti_Tempi)
    Public Overridable Property Brighetti_FamiglieProdotto() As DbSet(Of Brighetti_FamiglieProdotto)
    Public Overridable Property Brighetti_Giacenze() As DbSet(Of Brighetti_Giacenza)
    Public Overridable Property Brighetti_Macchine() As DbSet(Of Brighetti_Macchina)
    Public Overridable Property Brighetti_Reparti() As DbSet(Of Brighetti_Reparto)
    Public Overridable Property Brighetti_Procedure() As DbSet(Of Brighetti_Procedura)
    Public Overridable Property Brighetti_Note() As DbSet(Of Brighetti_Note)
    Public Overridable Property Brighetti_Documenti() As DbSet(Of Brighetti_Documenti)
    Public Overridable Property Brighetti_Attività() As DbSet(Of Brighetti_Attività)
    Public Overridable Property Brighetti_Lotti_Articoli() As DbSet(Of Brighetti_Lotti_Articoli)
    Public Overridable Property Brighetti_Dati_Macchina() As DbSet(Of Brighetti_Dati_Macchina)
    Public Overridable Property Brighetti_Dati_Macchina_Csv() As DbSet(Of Brighetti_Dati_Macchina_Csv)
    Public Overridable Property Brighetti_DettagliGiacenza() As DbSet(Of Brighetti_DettagliGiacenza)
    Public Overridable Property Brighetti_Lotti() As DbSet(Of Brighetti_Lotti)
    Public Overridable Property Brighetti_Previsione() As DbSet(Of Brighetti_Previsione)
    Public Overridable Property Log() As DbSet(Of Log)
    Public Overridable Property Audit() As DbSet(Of Audit)
    Public Overridable Property Brighetti_Planimetria() As DbSet(Of Brighetti_Planimetria)


End Class
<Table("Macchine")>
Public Class Macchine
    <Key>
    Public Property Id As Integer
    Public Property NomeMacchina As String
    Public Property DescrizioneMacchina As String
    Public Property IndirizzoMacchina As String
End Class

<Table("DatiMacchina")>
Public Class DatiMacchina
    <Key>
    Public Property Id As Integer
    Public Property IdMacchina As Integer
    Public Property StatoAttività As String
    Public Property NomeProgramma As String
    Public Property ContaPezzi As String
    Public Property Data As TipoUltimaModifica
End Class

<Table("Magazzino")>
Public Class Magazzino
    <Key>
    Public Property Id As Integer
    Public Property NomeMagazzino As String
    Public Property DescrizioneMagazzino As String
End Class
<Table("Articoli")>
Public Class Articoli
    <Key>
    Public Property Id As Integer
    Public Property Nome_Articolo As String
    Public Property Descrizione_Articolo As String
    Public Property MagazzinoId As Integer
    Public Property FamigliaId As Integer
    Public Property ArticoloCancellato As Boolean = 0
End Class
<Table("OrdiniInCorso")>
Public Class OrdiniInCorso
    <Key>
    Public Property Id As Integer
    Public Property IdArticolo As Integer
    Public Property QtaArticolo As Integer
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class
<Table("Ordini")>
Public Class Ordini
    <Key>
    Public Property Id As Integer
    Public Property IdOrdine As Integer
    Public Property IdArticolo As Integer
    Public Property QtaArticolo As Integer
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class
'========================================================================== BRIGHETTI NEW
<Table("Brighetti_Previsione")>
Public Class Brighetti_Previsione
    <Key>
    Public Property Id As Integer
    <Display(Name:="Codice Articolo")>
    Public Property CodiceArticolo As String
    <Display(Name:="Descrizione Articolo")>
    Public Property DescrizioneArticolo As String
    <Display(Name:="Quantità In Entrata")>
    Public Property QuantitaInEntrata As Integer
    <Display(Name:="Id Attività")>
    Public Property IdAttività As Integer
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class
'========================================================================== Brighetti_Articoli
<Table("Brighetti_Articoli")>
Public Class Brighetti_Articolo
    <Key>
    Public Property Id As Integer
    <Display(Name:="Codice Articolo")>
    Public Property CodiceArticolo As String
    <Display(Name:="Descrizione Articolo")>
    Public Property DescrizioneArticolo As String
    <Display(Name:="Note Articolo")>
    Public Property NoteArticolo As String
    <Display(Name:="Famiglia Prodotto")>
    Public Property FamigliaId As Integer
    <Display(Name:="Importanza")>
    Public Property Importanza As ImportanzaArticolo
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class
<Table("Brighetti_FamiglieProdotto")>
Public Class Brighetti_FamiglieProdotto
    <Key>
    Public Property Id As Integer
    Public Property NomeFamiglia As String
    Public Property DescrizioneFamiglia As String
End Class
<Table("Brighetti_Magazzini")>
Public Class Brighetti_Magazzino
    <Key>
    Public Property Id As Integer
    <Display(Name:="Codice Magazzino")>
    Public Property CodiceMagazzino As String
    <Display(Name:="Descrizione Magazzino")>
    Public Property DescrizioneMagazzino As String
    <Display(Name:="Tipo Magazzino")>
    Public Property TipologiaMagazzino As TipoMagazzino
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class
<Table("Brighetti_Giacenze")>
Public Class Brighetti_Giacenza
    <Key>
    Public Property Id As Integer
    <Display(Name:="Codice Articolo")>
    Public Property CodiceArticolo As String
    <Display(Name:="Codice Magazzino")>
    Public Property CodiceMagazzino As String
    <Display(Name:="Q.tà Giacenza")>
    Public Property QuantitàGiacenza As Decimal
    <Display(Name:="Q.tà Sottoscorta")>
    Public Property QuantitàSottoscorta As Decimal
    <Display(Name:="Q.tà In Previsione D'entrata")>
    Public Property InPrevisioneEntrata As Decimal
    Public Property ListaQuantità As List(Of Brighetti_DettagliGiacenza)
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class
<Table("Brighetti_Macchine")>
Public Class Brighetti_Macchina
    <Key>
    Public Property IdMacchina As Integer
    <Display(Name:="Nome Macchina")>
    Public Property NomeMacchina As String
    <Display(Name:="Descrizione Macchina")>
    Public Property DescrizioneMacchina As String
    <Display(Name:="Reparto")>
    Public Property IdReparto As Integer
    <Display(Name:="Utente")>
    Public Property IdUtente As String
    Public Property TipologiaMacchina As TipoMacchina
    Public Property CollegamentoMacchina As String
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class
<Table("Brighetti_Reparti")>
Public Class Brighetti_Reparto
    <Key>
    Public Property IdReparto As Integer
    <Display(Name:="Nome Reparto")>
    Public Property NomeReparto As String
    <Display(Name:="Descrizione Reparto")>
    Public Property DescrizioneReparto As String
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class
<Table("Brighetti_Procedure")>
Public Class Brighetti_Procedura
    <Key>
    Public Property IdProcedura As Integer
    <Display(Name:="Codice Articolo")>
    Public Property CodiceArticolo As String
    <Display(Name:="Incrementale Procedura")>
    Public Property IncrementaleProcedura As Int16
    <Display(Name:="Tipologia Attività")>
    Public Property TipoAttivita As TipoAttivitaProcedura
    <Display(Name:="Nome Attività")>
    Public Property NomeAttività As String
    Public Property idReparto As Integer
    Public Property idMacchinaMagazzino As Integer
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class
<Table("Brighetti_DettagliGiacenza")>
Public Class Brighetti_DettagliGiacenza
    <Key>
    Public Property id As Integer
    Public Property idGiacenza As Integer
    Public Property idProcedura As Integer
    Public Property QuantitàProdotta As Decimal
End Class
<Table("Brighetti_Tempi")>
Public Class Brighetti_Tempi
    <Key>
    Public Property IdTempo As Integer
    <Display(Name:="Id Attività")>
    Public Property IdAttività As Integer
    Public Property TipoAttività As TipoTempoAttività
    Public Property DataInizio As DateTime
    Public Property DataFine As DateTime
    Public Property Note As String
End Class
<Table("Brighetti_Lotti")>
Public Class Brighetti_Lotti
    <Key>
    <Display(Name:="Id Lotto")>
    Public Property IdLotto As Integer
    <Display(Name:="Nome Lotto")>
    Public Property NomeLotto As String
    <Display(Name:="Descrizione Lotto")>
    Public Property DescrizioneLotto As String
    <Display(Name:="Stato Lotto")>
    Public Property StatoLotto As StatoLotto
    <Display(Name:="Tipologia Lotto")>
    Public Property TipologiaLotto As String
    Public Property IdMagazzinoProvenienza As Integer
    <Display(Name:="Fornitore")>
    Public Property Fornitore As String
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class
<Table("Brighetti_Lotti_Articoli")>
Public Class Brighetti_Lotti_Articoli
    <Key>
    <Display(Name:="Id Lotto Articolo")>
    Public Property IdLottoArticolo As Integer
    Public Property IdLotto As Integer
    Public Property IdArticolo As Integer
    Public Property NomeArticolo As String
    Public Property QuantitàArticolo As String
    Public Property NoteArticolo As String
    Public Property StatoArticoloLotto As StatoArticoloLotto
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class

<Table("Brighetti_Note")>
Public Class Brighetti_Note
    <Key>
    Public Property Id As Integer
    Public Property TypeElem As TipoElemento
    Public Property IdEsterno As Integer
    Public Property Contenuto_Nota As String
    Public Property Data_Nota As DateTime
    Public Property Operatore_Id As String
    Public Property Operatore_Nome As String
End Class
<Table("Brighetti_Documenti")>
Public Class Brighetti_Documenti
    <Key>
    Public Property Id As Integer
    Public Property Nome_File As String
    Public Property IdEsterno As Integer
    Public Property TypeElem As TipoElemento
    Public Property Percorso_File As String
    Public Property DataCreazioneFile As DateTime
    Public Property Operatore_Id As String
    Public Property Operatore_Nome As String
End Class
<Table("Brighetti_Dati_Macchina")>
Public Class Brighetti_Dati_Macchina
    <Key>
    Public Property id As Integer
    Public Property idMacchina As Integer
    Public Property DatiValidi As Boolean
    Public Property DataRilevazione As DateTime
    Public Property Programma As String
    Public Property ProgrammaDescrizione As String
    Public Property StatoOperativoMacchina As String
    Public Property EsecuzioneMacchina As String
    Public Property PezziProdotti As Integer
End Class
<Table("Brighetti_Dati_Macchina_Csv")>
Public Class Brighetti_Dati_Macchina_Csv
    <Key>
    Public Property id As Integer
    Public Property idMacchina As Integer
    Public Property DatiValidi As Boolean
    Public Property DataRilevazione As DateTime
    Public Property DataLog As DateTime
    Public Property Programma As String
    Public Property ProgrammaDescrizione As String
    Public Property DataInizioLavorazione As DateTime
    Public Property DataFineLavorazione As DateTime
    Public Property PezziProdotti As Integer
    Public Property TempoInManuale As Decimal
    Public Property TempoInAttività As Decimal
    Public Property TempoInAutomatico As Decimal
    Public Property TempoInLavorazione As Decimal
End Class
<Table("Brighetti_Attività")>
Public Class Brighetti_Attività
    <Key>
    Public Property IdAttività As Integer
    Public Property OrdineDiProduzione As String
    <Display(Name:="Incrementale Procedura")>
    Public Property IncrementaleProcedura As Int16
    <Display(Name:="Codice Articolo")>
    Public Property CodiceArticolo As String
    <Display(Name:="Nome Attività")>
    Public Property NomeAttività As String
    Public Property idReparto As Integer
    Public Property idMacchinaMagazzino As Integer
    <Display(Name:="Q.tà Da Produrre")>
    Public Property QuantitàdaProdurre As Decimal
    <Display(Name:="Q.tà Prodotta")>
    Public Property QuantitàProdotta As Decimal
    <Display(Name:="Q.tà scartata")>
    Public Property QuantitàScartata As Decimal
    <Display(Name:="Attività")>
    Public Property StatoAttività As TipoStatoAttività
    <Display(Name:="Tipo Attività")>
    Public Property TipoAttivitaProcedura As TipoAttivitaProcedura
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class
<Table("Log")>
Public Class Log
    <Key>
    Public Property Id As Integer
    Public Property Livello As TipoLogLivello?
    Public Property Indirizzo As String
    Public Property Messaggio As String
    Public Property Dati As String
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class
<Table("Audit")>
Public Class Audit
    <Key>
    Public Property Id As Integer
    Public Property Livello As TipoLogLivello?
    Public Property Indirizzo As String
    Public Property Messaggio As String
    Public Property Dati As String
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class
<Table("Brighetti_Planimetria")>
Public Class Brighetti_Planimetria
    <Key>
    Public Property Id As Integer
    Public Property IdEsternoMacchina As Integer
    Public Property posX As String
    Public Property posY As String

    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class
'======================================================================================= COMPLEX TYPES
<ComplexType>
Public Class TipoUltimaModifica
    Public Property OperatoreID As String
    Public Property Operatore As String
    <DataType(DataType.DateTime)>
    <DisplayFormat(DataFormatString:=Costanti.DisplayFormatDateStandard)>
    Public Property Data As DateTime?
End Class
'======================================================================================= ENUM
Public Enum TipoElemento
    Articolo = 1
    Magazzino = 2
    Giacenza = 3
    Lavorazione = 4
    Lotto = 5
    Macchina = 6
End Enum
Public Enum TipoAttivitaProcedura
    Lavorazione = 1
    Versamento = 2
    Conclusione = 99
End Enum
Public Enum ImportanzaArticolo
    Nessuna = 1
    Bassa = 2
    Intermedia = 3
    Alta = 4
    Massima = 5
End Enum
Public Enum StatoLotto
    In_Attesa = 0
    Inviato = 1
    Ritornato = 2
    Non_Completato = 3
End Enum
Public Enum StatoArticoloLotto
    In_Attesa = 0
    Inviato = 1
    Ritornato = 2
    Mancante = 3
End Enum
Public Enum TipoStatoAttività
    In_attesa = 0
    In_Attrezzaggio = 1
    In_Lavorazione = 2
    StandBy = 3
    Completato = 4
    Annullato = 5
    BloccoDaAttivitàPrecedente = 99
End Enum
Public Enum TipoTempoAttività
    Attrezzaggio = 0
    Lavorazione = 1
End Enum
Public Enum TipoLogLivello As Byte
    Errors = 0
    Warning = 1
    Info = 2
    Debug = 3
End Enum
Public Enum TipoMagazzino As Byte
    Magazzino_Interno = 0
    Magazzino_ContoLavoro = 1
End Enum
Public Enum TipoAuditLivello As Byte
    Errors = 0
    Warning = 1
    Info = 2
    Debug = 3
End Enum

Public Enum TipoMacchina As Byte
    non_collegabile = 0
    mtconnect = 1
    opcua = 2
    Scheenberger = 3
End Enum
'======================================================================================= VIEWMODEL
Public Class Brighetti_Macchina_Viewmodel
    <Key>
    Public Property IdMacchina As Integer
    Public Property NomeMacchina As String
    Public Property DescrizioneMacchina As String
    Public Property IdReparto As Integer
    Public Property IdUtente As String
    Public Property TipologiaMacchina As TipoMacchina
    Public Property ListaDocumenti As List(Of Brighetti_Documenti)
    Public Property ListaNote As List(Of Brighetti_Note)
    Public Property ListaDatiMacchina As List(Of Brighetti_Dati_Macchina)
    Public Property CollegamentoMacchina As String
    Public Property UltimaModifica As TipoUltimaModifica
    Public Property MacchinaAccesa As Boolean
    Public Property posX As String
    Public Property posY As String
End Class


Public Class Brighetti_Articoli_Viewmodel
    <Key>
    Public Property Id As Integer
    <Display(Name:="Codice Articolo")>
    Public Property CodiceArticolo As String
    <Display(Name:="Descrizione Articolo")>
    Public Property DescrizioneArticolo As String
    <Display(Name:="Note Articolo")>
    Public Property NoteArticolo As String
    <Display(Name:="Famiglia Prodotto")>
    Public Property FamigliaId As Integer
    <Display(Name:="Importanza")>
    Public Property Importanza As ImportanzaArticolo
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
    Public Property ListaNote As List(Of Brighetti_Note)
    Public Property ListaProcedura As List(Of Brighetti_Procedura)
    Public Property ListaDocumenti As List(Of Brighetti_Documenti)
End Class
Public Class Brighetti_Lotti_Articoli_ViewModel
    Public Property IdLottoArticolo As Integer
    Public Property Quantità As Integer
    Public Property NoteArticolo As String
    Public Property StatoArticoloLotto As StatoArticoloLotto
End Class
Public Class Brighetti_Lotti_Edit_ViewModel
    <Key>
    <Display(Name:="Id Lotto")>
    Public Property IdLotto As Integer
    <Display(Name:="Nome Lotto")>
    Public Property NomeLotto As String
    <Display(Name:="Descrizione Lotto")>
    Public Property DescrizioneLotto As String
    <Display(Name:="Stato Lotto")>
    Public Property StatoLotto As StatoLotto
    <Display(Name:="Fornitore")>
    Public Property Fornitore As String
    <Display(Name:="Lista Articoli")>
    Public Property ListaArticoli As List(Of Brighetti_Lotti_Articoli)
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class
Public Class Brighetti_Lotti_ViewModel
    <Key>
    <Display(Name:="Id Lotto")>
    Public Property IdLotto As Integer
    <Display(Name:="Nome Lotto")>
    Public Property NomeLotto As String
    <Display(Name:="Descrizione Lotto")>
    Public Property DescrizioneLotto As String
    <Display(Name:="Stato Lotto")>
    Public Property StatoLotto As String
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
    Public Property ListaNote As List(Of Brighetti_Note)
    Public Property ListaDocumenti As List(Of Brighetti_Documenti)

End Class

Public Class CreaOdpViewModel
    <Display(Name:="Codice Articolo")>
    Public Property IdArticolo As Integer
    <Display(Name:="Quantità da produrre")>
    Public Property Qta As Integer
End Class
Public Class AttivitaMacchineViewModel
    Public Property ListaAttivita As List(Of Brighetti_Attività)
    Public Property Macchina As Brighetti_Macchina
    Public Property Utente As Object
End Class
Public Class OrdiniInCorsoViewModel
    <Key>
    Public Property Id As Integer
    Public Property IdArticolo As Integer
    Public Property NomeArticolo As String
    Public Property DescrizioneArticolo As String
    Public Property QtaArticolo As Integer
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class
Public Class StoricoViewModel
    Public Property Id As String
    Public Property OpName As String
    Public Property DataOrdine As DateTime
    Public Property CodiceArticolo As String
    Public Property ListaAttività As List(Of Attivitàviewmodel)
End Class
Public Class AttivitàviewModel
    <Display(Name:="IncrementaleAttività")>
    Public Property IncrementaleAttività As Integer
    <Display(Name:="Nome")>
    Public Property NomeAttività As String
    <Display(Name:="Q.tà Prod.")>
    Public Property Quantitàprodotta As Decimal
    <Display(Name:="Q.tà Scart.")>
    Public Property QuantitàScartata As Decimal
    <Display(Name:="Note")>
    Public Property CountNote As Integer
    <Display(Name:="File")>
    Public Property CountAllegati As Integer
    <Display(Name:="Tempo Lav.")>
    Public Property TempoLavorazione As Decimal
    <Display(Name:="Tempo Att.")>
    Public Property TempoAttrezzaggio As Decimal
    <Display(Name:="Perc.")>
    Public Property PercCompletamento As Integer
End Class
Public Class MacchineStatiViewModel
    Public Property IdMacchina As Integer
    <Display(Name:="Macchina")>
    Public Property NomeMacchina As String
    <Display(Name:="Stato Macchina")>
    Public Property StatoMacchina As String
    <Display(Name:="Data Ult. Ril.")>
    Public Property UltimiDatiRilevatia As String
    <Display(Name:="Programma")>
    Public Property UltimoProgrammaMacchina As String
    <Display(Name:="Efficienza")>
    Public Property GraficoAndamento As String
End Class
Public Class AnalisiViewModel
    Public Property IdMacchina As Integer
    Public Property NomeMacchina As String
    Public Property ListaEff As String
    Public Property ListaProduzione As List(Of ProduzioneViewModel)
    Public Property ListaPrevisione As List(Of PrevisioneViewModel)
    Public Property GaugeProdotti As Integer
    Public Property GaugeScartati As Integer
    Public Property GaugeCoda As Integer
End Class
Public Class PrevisioneViewModel
    Public Property Articolo As String
    Public Property qtaProdurre As String
    Public Property DataCreazione As String
End Class
Public Class ProduzioneViewModel
    Public Property Articolo As String
    Public Property QuantitàProdotta As Decimal
    Public Property QuantitàScartata As Decimal
    Public Property TempoDiProduzione As Double
End Class
Public Class EfficienzaViewModel
    Public Property Data As String
    Public Property PercEfficienza As Double
    Public Property PercTotale As Double
End Class
Public Class UtentiViewModel
    Public Property Id As String
    Public Property Username As String
    Public Property Email As String
End Class
Public Class UtentiMacchineViewModel
    Public Property IdUtente As String
    Public Property Username As String
End Class
Public Class MacchineViewModel
    Public Property Id As Integer
    Public Property NomeMacchina As String
    Public Property DescrizioneMacchina As String
    Public Property IndirizzoMacchina As String
    Public Property IdUtente As String
End Class

Public Class Brighetti_Articolo_ViewModel
    <Key>
    Public Property Id As Integer
    <Display(Name:="Codice Articolo")>
    Public Property CodiceArticolo As String
    <Display(Name:="Descrizione Articolo")>
    Public Property DescrizioneArticolo As String
    <Display(Name:="Importanza Articolo")>
    Public Property Importanza As String
    <Display(Name:="Note Articolo")>
    Public Property NoteArticolo As String
    <Display(Name:="Famiglia Prodotto")>
    Public Property FamigliaId As String
    <Display(Name:="Ultima Modifica")>
    Public Property UltimaModifica As TipoUltimaModifica
End Class