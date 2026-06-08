Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Entity
Imports System.Linq
Imports System.Net
Imports System.Web
Imports System.Web.Mvc
Imports Brighetti

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
            If ModelState.IsValid Then
                db.Brighetti_Procedure.Add(brighetti_Procedura)
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
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
            If ModelState.IsValid Then
                db.Entry(brighetti_Procedura).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
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
            Dim brighetti_Procedura As Brighetti_Procedura = db.Brighetti_Procedure.Find(id)
            db.Brighetti_Procedure.Remove(brighetti_Procedura)
            db.SaveChanges()
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
