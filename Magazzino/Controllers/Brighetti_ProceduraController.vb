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
    Public Class Brighetti_ProceduraController
        Inherits System.Web.Mvc.Controller

        Private db As New BrighettiModels

        ' GET: Brighetti_Procedura
        Function Index() As ActionResult
            Return View(db.Brighetti_Procedure.ToList())
        End Function

        ' GET: Brighetti_Procedura/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Procedura As Brighetti_Procedura = db.Brighetti_Procedure.Find(id)
            If IsNothing(brighetti_Procedura) Then
                Return HttpNotFound()
            End If
            Return View(brighetti_Procedura)
        End Function

        ' GET: Brighetti_Procedura/Create
        Function Create() As ActionResult
            Return View()
        End Function

        ' POST: Brighetti_Procedura/Create
        'Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        'Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="IdProcedura,CodiceArticolo,IncrementaleProcedura,NomeAttività,idReparto,idMacchina,UltimaModifica")> ByVal brighetti_Procedura As Brighetti_Procedura) As ActionResult
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Try
                OpID = User.Identity.GetUserId()
                OpName = User.Identity.GetUserName()
                If ModelState.IsValid Then
                    brighetti_Procedura.UltimaModifica = New TipoUltimaModifica With {
                        .Data = DateTime.Now, .Operatore = OpName, .OperatoreID = OpID
                    }
                    db.Brighetti_Procedure.Add(brighetti_Procedura)
                    db.SaveChanges()
                    db.Audit.Add(New Audit With {
                        .Livello = TipoAuditLivello.Info,
                        .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                        .Messaggio = "Procedura creata: " & brighetti_Procedura.NomeAttività & " (articolo " & brighetti_Procedura.CodiceArticolo & ")",
                        .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(brighetti_Procedura),
                        .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}
                    })
                    db.SaveChanges()
                    Return RedirectToAction("Index")
                End If
            Catch ex As Exception
                db.Log.Add(New Log With {
                    .Livello = TipoLogLivello.Errors,
                    .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                    .Messaggio = "Errore creazione procedura: " & ex.Message,
                    .Dati = "",
                    .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}
                })
                db.SaveChanges()
                ModelState.AddModelError("", "Errore durante la creazione della procedura.")
            End Try
            Return View(brighetti_Procedura)
        End Function

        ' GET: Brighetti_Procedura/Edit/5
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Procedura As Brighetti_Procedura = db.Brighetti_Procedure.Find(id)
            If IsNothing(brighetti_Procedura) Then
                Return HttpNotFound()
            End If
            Return View(brighetti_Procedura)
        End Function

        ' POST: Brighetti_Procedura/Edit/5
        'Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        'Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="IdProcedura,CodiceArticolo,IncrementaleProcedura,NomeAttività,idReparto,idMacchina,UltimaModifica")> ByVal brighetti_Procedura As Brighetti_Procedura) As ActionResult
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Try
                OpID = User.Identity.GetUserId()
                OpName = User.Identity.GetUserName()
                If ModelState.IsValid Then
                    brighetti_Procedura.UltimaModifica = New TipoUltimaModifica With {
                        .Data = DateTime.Now, .Operatore = OpName, .OperatoreID = OpID
                    }
                    db.Entry(brighetti_Procedura).State = EntityState.Modified
                    db.SaveChanges()
                    db.Audit.Add(New Audit With {
                        .Livello = TipoAuditLivello.Info,
                        .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                        .Messaggio = "Procedura modificata: " & brighetti_Procedura.NomeAttività & " (Id " & brighetti_Procedura.IdProcedura & ")",
                        .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(brighetti_Procedura),
                        .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}
                    })
                    db.SaveChanges()
                    Return RedirectToAction("Index")
                End If
            Catch ex As Exception
                db.Log.Add(New Log With {
                    .Livello = TipoLogLivello.Errors,
                    .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                    .Messaggio = "Errore modifica procedura: " & ex.Message,
                    .Dati = "",
                    .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}
                })
                db.SaveChanges()
                ModelState.AddModelError("", "Errore durante la modifica della procedura.")
            End Try
            Return View(brighetti_Procedura)
        End Function

        ' GET: Brighetti_Procedura/Delete/5
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Procedura As Brighetti_Procedura = db.Brighetti_Procedure.Find(id)
            If IsNothing(brighetti_Procedura) Then
                Return HttpNotFound()
            End If
            Return View(brighetti_Procedura)
        End Function

        ' POST: Brighetti_Procedura/Delete/5
        <HttpPost()>
        <ActionName("Delete")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As Integer) As ActionResult
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Try
                OpID = User.Identity.GetUserId()
                OpName = User.Identity.GetUserName()
                Dim brighetti_Procedura As Brighetti_Procedura = db.Brighetti_Procedure.Find(id)
                Dim nomeAttivita = brighetti_Procedura.NomeAttività
                Dim codiceArticolo = brighetti_Procedura.CodiceArticolo
                db.Brighetti_Procedure.Remove(brighetti_Procedura)
                db.SaveChanges()
                db.Audit.Add(New Audit With {
                    .Livello = TipoAuditLivello.Warning,
                    .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                    .Messaggio = "Procedura eliminata: " & nomeAttivita & " (articolo " & codiceArticolo & ", Id " & id & ")",
                    .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {.Id = id, .NomeAttività = nomeAttivita, .CodiceArticolo = codiceArticolo}),
                    .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}
                })
                db.SaveChanges()
            Catch ex As Exception
                db.Log.Add(New Log With {
                    .Livello = TipoLogLivello.Errors,
                    .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                    .Messaggio = "Errore eliminazione procedura (Id " & id & "): " & ex.Message,
                    .Dati = "",
                    .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}
                })
                db.SaveChanges()
            End Try
            Return RedirectToAction("Index")
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace
