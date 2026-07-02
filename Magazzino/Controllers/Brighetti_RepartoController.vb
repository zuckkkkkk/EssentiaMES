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
    Public Class Brighetti_RepartoController
        Inherits System.Web.Mvc.Controller

        Private db As New BrighettiModels

        ' GET: Brighetti_Reparto
        Function Index() As ActionResult
            Return View(db.Brighetti_Reparti.ToList())
        End Function

        ' GET: Brighetti_Reparto/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Reparto As Brighetti_Reparto = db.Brighetti_Reparti.Find(id)
            If IsNothing(brighetti_Reparto) Then
                Return HttpNotFound()
            End If
            Return PartialView(brighetti_Reparto)
        End Function

        ' GET: Brighetti_Reparto/Create
        Function Create() As ActionResult
            Return PartialView()
        End Function

        ' POST: Brighetti_Reparto/Create
        'Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        'Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="IdReparto,NomeReparto,DescrizioneReparto,UltimaModifica")> ByVal brighetti_Reparto As Brighetti_Reparto) As ActionResult
            Dim Opid = vbNullString
            Dim OpName = vbNullString
            Try
                Opid = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                If ModelState.IsValid Then
                    db.Brighetti_Reparti.Add(New Brighetti_Reparto With {
                        .DescrizioneReparto = brighetti_Reparto.DescrizioneReparto,
                        .NomeReparto = brighetti_Reparto.NomeReparto,
                        .UltimaModifica = New TipoUltimaModifica With {
                            .Data = DateTime.Now,
                            .Operatore = OpName,
                            .OperatoreID = Opid
                    }
                    })
                    db.SaveChanges()
                    db.Audit.Add(New Audit With {
                        .Livello = TipoAuditLivello.Info,
                        .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                        .Messaggio = "Reparto creato: " & brighetti_Reparto.NomeReparto,
                        .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(brighetti_Reparto),
                        .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = Opid, .Operatore = OpName, .Data = DateTime.Now}
                    })
                    db.SaveChanges()
                    Return Json(New With {.ok = True, .message = "Reparto correttamente inserito"})
                End If
            Catch ex As Exception
                db.Log.Add(New Log With {
                                 .Livello = TipoLogLivello.Warning,
                                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                                 .Messaggio = "Errore inserimento Reparto: " & vbNewLine & ex.Message,
                                 .Dati = "",
                                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = Opid, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore nella creazione Reparto"})
            End Try
            Return Json(New With {.ok = False, .message = "Errore nella creazione Reparto"})
        End Function

        ' GET: Brighetti_Reparto/Edit/5
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Reparto As Brighetti_Reparto = db.Brighetti_Reparti.Find(id)
            If IsNothing(brighetti_Reparto) Then
                Return HttpNotFound()
            End If
            Return PartialView(brighetti_Reparto)
        End Function

        ' POST: Brighetti_Reparto/Edit/5
        'Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        'Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="IdReparto,NomeReparto,DescrizioneReparto,UltimaModifica")> ByVal brighetti_Reparto As Brighetti_Reparto) As JsonResult
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Try
                OpID = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                If ModelState.IsValid Then
                    Dim rep = db.Brighetti_Reparti.Find(brighetti_Reparto.IdReparto)
                    If rep.NomeReparto <> brighetti_Reparto.NomeReparto Then
                        rep.NomeReparto = brighetti_Reparto.NomeReparto
                        db.SaveChanges()
                    End If
                    If rep.DescrizioneReparto <> brighetti_Reparto.DescrizioneReparto Then
                        rep.DescrizioneReparto = brighetti_Reparto.DescrizioneReparto
                        db.SaveChanges()
                    End If
                    rep.UltimaModifica = New TipoUltimaModifica With {
                        .Data = DateTime.Now,
                        .Operatore = OpName,
                        .OperatoreID = OpID
                    }
                    db.SaveChanges()
                    db.Audit.Add(New Audit With {
                        .Livello = TipoAuditLivello.Info,
                        .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                        .Messaggio = "Reparto modificato: " & rep.NomeReparto & " (Id " & rep.IdReparto & ")",
                        .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(brighetti_Reparto),
                        .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}
                    })
                    db.SaveChanges()
                    Return Json(New With {.ok = True, .message = "Reparto correttamente modificato"})
                End If
                Return Json(New With {.ok = False, .message = "Errore nella modifica Reparto"})
            Catch ex As Exception
                db.Log.Add(New Log With {
                 .Livello = TipoLogLivello.Warning,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Errore modifica Reparto: " & vbNewLine & ex.Message,
                 .Dati = "",
                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore nella modifica Reparto"})
            End Try
            Return Json(New With {.ok = False, .message = "Errore nella modifica Reparto"})
        End Function
        ' GET: Brighetti_Reparto/Delete/5
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Reparto As Brighetti_Reparto = db.Brighetti_Reparti.Find(id)
            If IsNothing(brighetti_Reparto) Then
                Return HttpNotFound()
            End If
            Return PartialView(brighetti_Reparto)
        End Function

        ' POST: Brighetti_Reparto/Delete/5
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal idReparto As Integer) As ActionResult
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Try
                OpID = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim Rep As Brighetti_Reparto = db.Brighetti_Reparti.Find(idReparto)
                Dim nomeReparto = Rep.NomeReparto
                db.Brighetti_Reparti.Remove(Rep)
                db.SaveChanges()
                db.Audit.Add(New Audit With {
                    .Livello = TipoAuditLivello.Warning,
                    .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                    .Messaggio = "Reparto eliminato: " & nomeReparto & " (Id " & idReparto & ")",
                    .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {.Id = idReparto, .NomeReparto = nomeReparto}),
                    .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}
                })
                db.SaveChanges()
                Return Json(New With {.ok = True, .message = "Cancellazione Reparto confermata correttamente"})
            Catch ex As Exception
                db.Log.Add(New Log With {
                 .Livello = TipoLogLivello.Warning,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Errore cancellazione Reparto: " & vbNewLine & ex.Message,
                 .Dati = "",
                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore nella cancellazione Reparto"})
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
