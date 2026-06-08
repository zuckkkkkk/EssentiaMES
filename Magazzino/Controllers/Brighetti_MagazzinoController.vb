Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Entity
Imports System.Linq
Imports System.Net
Imports System.Web
Imports System.Web.Mvc
Imports Brighetti
Imports Microsoft.AspNet.Identity

Namespace Controllers
    Public Class Brighetti_MagazzinoController
        Inherits System.Web.Mvc.Controller

        Private db As New BrighettiModels

        ' GET: Brighetti_Magazzino
        Function Index() As ActionResult
            Return View(db.Brighetti_Magazzini.ToList())
        End Function

        ' GET: Brighetti_Magazzino/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Magazzino As Brighetti_Magazzino = db.Brighetti_Magazzini.Find(id)
            If IsNothing(brighetti_Magazzino) Then
                Return HttpNotFound()
            End If
            Return PartialView(brighetti_Magazzino)
        End Function

        ' GET: Brighetti_Magazzino/Create
        Function Create() As ActionResult
            Return PartialView()
        End Function

        ' POST: Brighetti_Magazzino/Create
        'Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        'Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="Id,CodiceMagazzino,DescrizioneMagazzino,UltimaModifica")> ByVal brighetti_Magazzino As Brighetti_Magazzino) As JsonResult
            Dim Opid = vbNullString
            Dim OpName = vbNullString
            Try
                Opid = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                If ModelState.IsValid Then
                    db.Brighetti_Magazzini.Add(New Brighetti_Magazzino With {
                        .CodiceMagazzino = brighetti_Magazzino.CodiceMagazzino,
                        .DescrizioneMagazzino = brighetti_Magazzino.DescrizioneMagazzino,
                        .UltimaModifica = New TipoUltimaModifica With {
                            .Data = DateTime.Now,
                            .Operatore = OpName,
                            .OperatoreID = Opid
                        }
                    })
                    db.SaveChanges()
                    Return Json(New With {.ok = True, .message = "Magazzino correttamente inserito"})
                End If
                Return Json(New With {.ok = False, .message = "Errore nella creazione Magazzino"})
            Catch ex As Exception
                db.Log.Add(New Log With {
                                 .Livello = TipoLogLivello.Warning,
                                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                                 .Messaggio = "Errore inserimento Magazzino: " & vbNewLine & ex.Message,
                                 .Dati = "",
                                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = Opid, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore nella creazione Magazzino"})
            End Try
            Return Json(New With {.ok = False, .message = "Errore nella creazione Magazzino"})
        End Function

        ' GET: Brighetti_Magazzino/Edit/5
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Magazzino As Brighetti_Magazzino = db.Brighetti_Magazzini.Find(id)
            If IsNothing(brighetti_Magazzino) Then
                Return HttpNotFound()
            End If
            Return PartialView(brighetti_Magazzino)
        End Function

        ' POST: Brighetti_Magazzino/Edit/5
        'Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        'Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="Id,CodiceMagazzino,DescrizioneMagazzino,UltimaModifica")> ByVal brighetti_Magazzino As Brighetti_Magazzino) As JsonResult
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Try
                OpID = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                If ModelState.IsValid Then
                    Dim mag = db.Brighetti_Magazzini.Find(brighetti_Magazzino.Id)
                    If mag.CodiceMagazzino <> brighetti_Magazzino.CodiceMagazzino Then
                        mag.CodiceMagazzino = brighetti_Magazzino.CodiceMagazzino
                        db.SaveChanges()
                    End If
                    If mag.DescrizioneMagazzino <> brighetti_Magazzino.DescrizioneMagazzino Then
                        mag.DescrizioneMagazzino = brighetti_Magazzino.DescrizioneMagazzino
                        db.SaveChanges()
                    End If
                    mag.UltimaModifica = New TipoUltimaModifica With {
                        .Data = DateTime.Now,
                        .Operatore = OpName,
                        .OperatoreID = OpID
                    }
                    db.SaveChanges()
                    Return Json(New With {.ok = True, .message = "Magazzino correttamente modificato"})
                End If
                Return Json(New With {.ok = False, .message = "Errore nella modifica magazzino"})
            Catch ex As Exception
                db.Log.Add(New Log With {
                 .Livello = TipoLogLivello.Warning,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Errore modifica magazzino: " & vbNewLine & ex.Message,
                 .Dati = "",
                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore nella modifica magazzino"})
            End Try
            Return Json(New With {.ok = False, .message = "Errore nella modifica magazzino"})
        End Function

        ' GET: Brighetti_Magazzino/Delete/5
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Magazzino As Brighetti_Magazzino = db.Brighetti_Magazzini.Find(id)
            If IsNothing(brighetti_Magazzino) Then
                Return HttpNotFound()
            End If
            Return PartialView(brighetti_Magazzino)
        End Function

        ' POST: Brighetti_Magazzino/Delete/5
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As Integer) As JsonResult
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Try
                OpID = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim Mag As Brighetti_Magazzino = db.Brighetti_Magazzini.Find(id)
                db.Brighetti_Magazzini.Remove(Mag)
                db.SaveChanges()
                Return Json(New With {.ok = True, .message = "Cancellazione Magazzino confermata correttamente"})
            Catch ex As Exception
                db.Log.Add(New Log With {
                 .Livello = TipoLogLivello.Warning,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Errore cancellazione Magazzino: " & vbNewLine & ex.Message,
                 .Dati = "",
                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore nella cancellazione Magazzino"})
            End Try
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace
