Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Entity
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Web
Imports System.Web.Mvc
Imports Brighetti
Imports Microsoft.AspNet.Identity
Imports Newtonsoft.Json
Imports NPOI.SS.UserModel
Imports NPOI.XSSF.UserModel

Namespace Controllers
    Public Class Brighetti_LottiController
        Inherits System.Web.Mvc.Controller

        Private db As New BrighettiModels

        ' GET: Brighetti_Lotti
        Function Index() As ActionResult
            Dim LottiList As New List(Of Brighetti_Lotti_ViewModel)
            Dim Lista = db.Brighetti_Lotti.OrderBy(Function(x) x.StatoLotto).ToList()
            For Each l In Lista
                Dim StatoLottoRet = ""
                Select Case l.StatoLotto
                    Case StatoLotto.Inviato
                        StatoLottoRet = "<span class='badge bg-info text-dark'>Inviato</span>"
                    Case StatoLotto.In_Attesa
                        StatoLottoRet = "<span class='badge bg-primary'>In Attesa</span>"
                    Case StatoLotto.Non_Completato
                        StatoLottoRet = "<span class='badge bg-danger'>Non Completato</span>"
                    Case StatoLotto.Ritornato
                        StatoLottoRet = "<span class='badge bg-success'>Ritornato da C/O</span>"
                End Select
                LottiList.Add(New Brighetti_Lotti_ViewModel With {
                    .DescrizioneLotto = l.DescrizioneLotto,
                    .IdLotto = l.IdLotto,
                    .NomeLotto = l.NomeLotto,
                    .StatoLotto = StatoLottoRet,
                    .UltimaModifica = l.UltimaModifica
                })
            Next
            Return View(LottiList)
        End Function

        ' GET: Brighetti_Lotti/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim b As Brighetti_Lotti = db.Brighetti_Lotti.Find(id)
            Dim listaNote = db.Brighetti_Note.Where(Function(x) x.IdEsterno = b.IdLotto And x.TypeElem = TipoElemento.Lotto).ToList
            Dim listaDocumenti = db.Brighetti_Documenti.Where(Function(x) x.IdEsterno = b.IdLotto And x.TypeElem = TipoElemento.Lotto).ToList
            Dim ret As New Brighetti_Lotti_ViewModel With {
                .IdLotto = b.IdLotto,
                .DescrizioneLotto = b.DescrizioneLotto,
                .ListaDocumenti = listaDocumenti,
                .ListaNote = listaNote,
                .NomeLotto = b.NomeLotto,
                .StatoLotto = b.StatoLotto,
                .UltimaModifica = b.UltimaModifica
            }
            If IsNothing(b) Then
                Return HttpNotFound()
            End If
            Return PartialView(ret)
        End Function

        ' GET: Brighetti_Lotti/Create
        Function Create() As ActionResult
            Return View()
        End Function

        ' POST: Brighetti_Lotti/Create
        'Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        'Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="IdLotto,NomeLotto,DescrizioneLotto,UltimaModifica")> ByVal brighetti_Lotti As Brighetti_Lotti) As ActionResult
            If ModelState.IsValid Then
                db.Brighetti_Lotti.Add(brighetti_Lotti)
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(brighetti_Lotti)
        End Function

        ' GET: Brighetti_Lotti/Edit/5
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Lotti As Brighetti_Lotti = db.Brighetti_Lotti.Find(id)
            If IsNothing(brighetti_Lotti) Then
                Return HttpNotFound()
            End If

            Dim listaArticoli = db.Brighetti_Lotti_Articoli.Where(Function(x) x.IdLotto = id).ToList

            Dim Brighetti_Lotti_Edit_Viewmodel As New Brighetti_Lotti_Edit_ViewModel With {
            .StatoLotto = brighetti_Lotti.StatoLotto,
            .DescrizioneLotto = brighetti_Lotti.DescrizioneLotto,
            .Fornitore = brighetti_Lotti.Fornitore,
            .IdLotto = brighetti_Lotti.IdLotto,
            .NomeLotto = brighetti_Lotti.NomeLotto,
            .UltimaModifica = brighetti_Lotti.UltimaModifica,
            .ListaArticoli = listaArticoli
            }
            Return PartialView(Brighetti_Lotti_Edit_Viewmodel)
        End Function

        ' POST: Brighetti_Lotti/Edit/5
        'Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        'Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="IdLotto,NomeLotto,StatoLotto,DescrizioneLotto,UltimaModifica,Fornitore,ListaArticoli")> ByVal Brighetti_Lotti_Edit_ViewModel As Brighetti_Lotti_Edit_ViewModel) As JsonResult
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Dim NowDate = DateTime.Now
            Try
                OpID = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                If ModelState.IsValid Then
                    Dim listaArticoli As New List(Of Brighetti_Lotti_Articoli_ViewModel)
                    Dim req = Request.Form
                    For Each r In req.AllKeys
                        If r.EndsWith("_id") Then
                            listaArticoli.Add(New Brighetti_Lotti_Articoli_ViewModel With {
                                .IdLottoArticolo = Request.Form(r)})
                        End If
                        If r.EndsWith("_qta") Then
                            Dim id = Convert.ToInt32(r.Split("_")(0))
                            listaArticoli.Where(Function(x) x.IdLottoArticolo = id).First.Quantità = Request.Form(r)
                        End If
                        If r.EndsWith("_stato") Then
                            Dim id = Convert.ToInt32(r.Split("_")(0))
                            listaArticoli.Where(Function(x) x.IdLottoArticolo = id).First.StatoArticoloLotto = Request.Form(r)
                        End If
                        If r.EndsWith("_nota") Then
                            Dim id = Convert.ToInt32(r.Split("_")(0))
                            listaArticoli.Where(Function(x) x.IdLottoArticolo = id).First.NoteArticolo = Request.Form(r)
                        End If
                    Next
                    Dim lotto = db.Brighetti_Lotti.Find(Brighetti_Lotti_Edit_ViewModel.IdLotto)
                    If lotto.NomeLotto <> Brighetti_Lotti_Edit_ViewModel.NomeLotto Then
                        lotto.NomeLotto = Brighetti_Lotti_Edit_ViewModel.NomeLotto
                        db.SaveChanges()
                    End If
                    If lotto.DescrizioneLotto <> Brighetti_Lotti_Edit_ViewModel.DescrizioneLotto Then
                        lotto.DescrizioneLotto = Brighetti_Lotti_Edit_ViewModel.DescrizioneLotto
                        db.SaveChanges()
                    End If
                    If lotto.Fornitore <> Brighetti_Lotti_Edit_ViewModel.Fornitore Then
                        lotto.Fornitore = Brighetti_Lotti_Edit_ViewModel.Fornitore
                        db.SaveChanges()
                    End If
                    If lotto.StatoLotto <> Brighetti_Lotti_Edit_ViewModel.StatoLotto Then
                        lotto.StatoLotto = Brighetti_Lotti_Edit_ViewModel.StatoLotto
                        db.SaveChanges()
                    End If
                    lotto.UltimaModifica = New TipoUltimaModifica With {
                        .Data = NowDate,
                        .Operatore = OpName,
                        .OperatoreID = OpID
                    }
                    db.SaveChanges()
                    db.Audit.Add(New Audit With {
                             .Livello = TipoLogLivello.Warning,
                             .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                             .Messaggio = "Lotto Modificato correttamente: " & Brighetti_Lotti_Edit_ViewModel.IdLotto,
                             .Dati = JsonConvert.SerializeObject(Brighetti_Lotti_Edit_ViewModel),
                             .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                    db.SaveChanges()
                    For Each art In listaArticoli
                        Dim articoloLotto = db.Brighetti_Lotti_Articoli.Find(art.IdLottoArticolo)
                        If articoloLotto.QuantitàArticolo <> art.Quantità Then
                            articoloLotto.QuantitàArticolo = art.Quantità
                            db.SaveChanges()
                        End If
                        If articoloLotto.StatoArticoloLotto <> art.StatoArticoloLotto Then
                            articoloLotto.StatoArticoloLotto = art.StatoArticoloLotto
                            db.SaveChanges()
                        End If
                        If articoloLotto.NoteArticolo <> art.NoteArticolo Then
                            articoloLotto.NoteArticolo = art.NoteArticolo
                            db.SaveChanges
                        End If
                        db.Audit.Add(New Audit With {
                        .Livello = TipoLogLivello.Warning,
                        .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                        .Messaggio = "Articolo nel Lotto Modificato correttamente: " & art.IdLottoArticolo,
                        .Dati = JsonConvert.SerializeObject(art),
                        .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                        db.SaveChanges()
                    Next
                    Return Json(New With {.ok = True, .message = "Lotto Modificato Correttamente!"})
                End If
            Catch ex As Exception
                db.Log.Add(New Log With {
                              .Livello = TipoLogLivello.Warning,
                              .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                              .Messaggio = "Errore modifica lotto: " & vbNewLine & ex.Message,
                              .Dati = "",
                              .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = NowDate}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore nella modifica del Lotto!"})
            End Try
            Return Json(New With {.ok = True, .message = "Lotto Modificato Correttamente!"})
        End Function

        ' GET: Brighetti_Lotti/Delete/5
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Lotti As Brighetti_Lotti = db.Brighetti_Lotti.Find(id)
            If IsNothing(brighetti_Lotti) Then
                Return HttpNotFound()
            End If
            Return View(brighetti_Lotti)
        End Function

        ' POST: Brighetti_Lotti/Delete/5
        <HttpPost()>
        <ActionName("Delete")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As Integer) As ActionResult
            Dim brighetti_Lotti As Brighetti_Lotti = db.Brighetti_Lotti.Find(id)
            db.Brighetti_Lotti.Remove(brighetti_Lotti)
            db.SaveChanges()
            Return RedirectToAction("Index")
        End Function
        Function ListaArticoli(ByVal id As Integer) As ActionResult
            Dim OpID As String = vbNullString
            Dim OpName As String = vbNullString
            'Apertura file
            Dim fs As New FileStream(Server.MapPath("\Content\Template\Lotti.xlsx"), FileMode.Open, FileAccess.Read)
            Dim workbook As XSSFWorkbook = New XSSFWorkbook(fs)
            'Start Pop
            Dim i As Integer = 1
            Dim ms As New MemoryStream
            Dim ms1 As New MemoryStream
            'Riga Intestazione
            Dim NowDate = DateTime.Now
            Try
                OpID = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim lotto = db.Brighetti_Lotti.Find(id)
                Dim articoliLotto = db.Brighetti_Lotti_Articoli.Where(Function(x) x.IdLotto = id).ToList
                Dim ws As XSSFSheet = workbook.GetSheetAt(0)
                With ws
                    ws.GetRow(3).GetCell(0).SetCellValue("Operatore: " + OpName)
                    ws.GetRow(3).GetCell(4).SetCellValue("Data: " + NowDate.ToString)
                    ws.GetRow(4).GetCell(0).SetCellValue("Lotto N° " + lotto.NomeLotto.ToString)
                    ws.GetRow(6).GetCell(0).SetCellValue("Descrizione Lotto: " + lotto.DescrizioneLotto.ToString)
                    Dim c = 8
                    For Each art In articoliLotto
                        Dim r As IRow = ws.GetRow(c)
                        r.GetCell(0).SetCellValue(art.NomeArticolo)
                        r.GetCell(3).SetCellValue(art.QuantitàArticolo)
                        Select Case art.StatoArticoloLotto
                            Case StatoLotto.Inviato
                                r.GetCell(5).SetCellValue("Inviato")
                            Case StatoLotto.In_Attesa
                                r.GetCell(5).SetCellValue("In Attesa")
                            Case StatoLotto.Non_Completato
                                r.GetCell(5).SetCellValue("Non Completato")
                            Case StatoLotto.Ritornato
                                r.GetCell(5).SetCellValue("Ritornato da C/O")
                        End Select
                        Dim ArticoloAnagrafica = db.Brighetti_Articoli.Where(Function(x) x.Id = art.IdArticolo).FirstOrDefault
                        If Not IsNothing(ArticoloAnagrafica) Then
                            Select Case ArticoloAnagrafica.Importanza
                                Case ImportanzaArticolo.Nessuna
                                    r.GetCell(7).SetCellValue("⭐")
                                Case ImportanzaArticolo.Bassa
                                    r.GetCell(7).SetCellValue("⭐⭐")
                                Case ImportanzaArticolo.Intermedia
                                    r.GetCell(7).SetCellValue("⭐⭐⭐")
                                Case ImportanzaArticolo.Alta
                                    r.GetCell(7).SetCellValue("⭐⭐⭐⭐")
                                Case ImportanzaArticolo.Massima
                                    r.GetCell(7).SetCellValue("⭐⭐⭐⭐⭐")
                                Case Else
                                    r.GetCell(7).SetCellValue("⭐")
                            End Select
                        End If

                        c += 1
                    Next
                End With
                workbook.Write(ms)
                db.Audit.Add(New Audit With {
                        .Livello = TipoLogLivello.Warning,
                        .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                        .Messaggio = "Lotto Stampato Correttamente: " & lotto.IdLotto,
                        .Dati = JsonConvert.SerializeObject(lotto.IdLotto),
                        .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return File(ms.ToArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Now.Year.ToString & "_" & Now.Month.ToString & "_" & Now.Day.ToString & "_LOTTO_" + lotto.NomeLotto + ".xlsx")
            Catch ex As Exception
                db.Log.Add(New Log With {
                           .Livello = TipoLogLivello.Warning,
                           .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                           .Messaggio = "Errore stampa lotto: " & vbNewLine & ex.Message,
                           .Dati = "",
                           .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = NowDate}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore nella modifica del Lotto!"})
            End Try
        End Function
        Private Function GetCellValue(row As IRow, col As Integer, Optional OpID As String = vbNullString, Optional OpName As String = vbNullString) As Object
            Dim result As String = vbNullString
            Try
                If Not IsNothing(row) Then
                    Dim cell As ICell = row.GetCell(col)
                    If Not IsNothing(cell) Then
                        Select Case cell.CellType
                            Case CellType.Numeric
                                If DateUtil.IsCellDateFormatted(cell) Then
                                    Return cell.DateCellValue
                                Else
                                    Return cell.NumericCellValue
                                End If
                            Case CellType.String
                                Return cell.StringCellValue.Trim
                            Case CellType.Boolean
                                Return cell.BooleanCellValue
                            Case CellType.Formula
                                Return cell.NumericCellValue
                            Case CellType.Blank, CellType.Error, CellType.Unknown
                                Return vbNullString
                        End Select
                    End If
                End If
            Catch ex As Exception

            End Try

            Return result
        End Function
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace
