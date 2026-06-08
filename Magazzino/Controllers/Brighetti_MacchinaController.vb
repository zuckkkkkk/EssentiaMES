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
    Public Class Brighetti_MacchinaController
        Inherits System.Web.Mvc.Controller

        Private db As New BrighettiModels
        Private appctx As New ApplicationDbContext
        ' GET: Brighetti_Macchina
        Function Index() As ActionResult
            Dim listaRet As New List(Of Brighetti_Macchina_ViewModel)
            Dim listaIniziale = db.Brighetti_Macchine.ToList()
            For Each l In listaIniziale
                Try
                    Dim NomeUtente = appctx.Users.Where(Function(x) x.Id = l.IdUtente).FirstOrDefault
                    Dim NomeReparto = db.Brighetti_Reparti.Where(Function(x) x.IdReparto = l.IdReparto).FirstOrDefault
                    listaRet.Add(New Brighetti_Macchina_Viewmodel With {
                        .DescrizioneMacchina = l.DescrizioneMacchina,
                        .IdMacchina = l.IdMacchina,
                        .IdReparto = NomeReparto.IdReparto,
                        .IdUtente = IIf(Not IsNothing(NomeUtente.UserName), NomeUtente.UserName, ""),
                        .NomeMacchina = l.NomeMacchina,
                        .UltimaModifica = l.UltimaModifica
                    })
                Catch ex As Exception

                End Try
            Next
            Return View(listaRet)
        End Function

        ' GET: Brighetti_Macchina/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim b As Brighetti_Macchina = db.Brighetti_Macchine.Find(id)
            If IsNothing(b) Then
                Return HttpNotFound()
            End If
            Dim listaNote = db.Brighetti_Note.Where(Function(x) x.IdEsterno = b.IdMacchina And x.TypeElem = TipoElemento.Macchina).ToList
            Dim listaDocumenti = db.Brighetti_Documenti.Where(Function(x) x.IdEsterno = b.IdMacchina And x.TypeElem = TipoElemento.Macchina).ToList
            Dim ret As New Brighetti_Macchina_ViewModel With {
            .UltimaModifica = b.UltimaModifica,
            .DescrizioneMacchina = b.DescrizioneMacchina,
            .IdMacchina = b.IdMacchina,
            .IdReparto = b.IdReparto,
            .IdUtente = b.IdUtente,
            .ListaDocumenti = listaDocumenti,
            .ListaNote = listaNote
            }
            Return PartialView(ret)
        End Function

        ' GET: Brighetti_Macchina/Create
        Function Create() As ActionResult
            'Section Ricerca Utenti
            Dim roleId = appctx.Roles.Where(Function(m) m.Name = "Operatore").[Select](Function(m) m.Id).SingleOrDefault()
            Dim list = appctx.Users.Where(Function(u) u.Roles.Any(Function(r) r.RoleId = roleId)).ToList()
            list.Insert(0, New ApplicationUser With {.Id = "0", .UserName = "Selezionare Voce..."})
            ViewBag.IdUtente = New SelectList(list, "Id", "Username")
            'Section Ricerca Reparti
            Dim listReparti = db.Brighetti_Reparti.ToList
            listReparti.Insert(0, New Brighetti_Reparto With {.IdReparto = "0", .NomeReparto = "Selezionare Voce..."})
            ViewBag.IdReparto = New SelectList(listReparti, "IdReparto", "NomeReparto")
            Return PartialView()
        End Function

        ' POST: Brighetti_Macchina/Create
        'Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        'Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="IdMacchina,NomeMacchina,DescrizioneMacchina,IdReparto,IdUtente,UltimaModifica,Utente,Macchina")> ByVal brighetti_Macchina As Brighetti_Macchina) As JsonResult
            Dim Opid = vbNullString
            Dim OpName = vbNullString
            Try
                Opid = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                If ModelState.IsValid Then
                    db.Brighetti_Macchine.Add(New Brighetti_Macchina With {
                        .IdReparto = brighetti_Macchina.IdReparto,
                        .DescrizioneMacchina = brighetti_Macchina.DescrizioneMacchina,
                        .IdMacchina = brighetti_Macchina.IdMacchina,
                        .IdUtente = brighetti_Macchina.IdUtente,
                        .NomeMacchina = brighetti_Macchina.NomeMacchina,
                        .UltimaModifica = New TipoUltimaModifica With {
                            .Data = DateTime.Now,
                            .Operatore = OpName,
                            .OperatoreID = Opid
                        }
                    })
                    db.SaveChanges()
                    appctx.MacchineUsers.Add(New MacchineUsers With {
                        .IdMacchina = brighetti_Macchina.IdMacchina,
                        .IdReparto = brighetti_Macchina.IdReparto,
                        .IdUtente = brighetti_Macchina.IdUtente
                    })
                    db.SaveChanges()
                    Return Json(New With {.ok = True, .message = "Reparto correttamente inserito"})
                End If
                Return Json(New With {.ok = False, .message = "Errore creazione Reparto"})
            Catch ex As Exception
                db.Log.Add(New Log With {
                                 .Livello = TipoLogLivello.Warning,
                                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                                 .Messaggio = "Errore inserimento Reparto: " & vbNewLine & ex.Message,
                                 .Dati = "",
                                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = Opid, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore creazione Reparto"})
            End Try
            Return Json(New With {.ok = False, .message = "Errore creazione Reparto"})
        End Function

        ' GET: Brighetti_Macchina/Edit/5
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Macchina As Brighetti_Macchina = db.Brighetti_Macchine.Find(id)
            If IsNothing(brighetti_Macchina) Then
                Return HttpNotFound()
            End If
            'Section Ricerca Utenti
            Dim roleId = appctx.Roles.Where(Function(m) m.Name = "Operatore").[Select](Function(m) m.Id).SingleOrDefault()
            Dim list = appctx.Users.Where(Function(u) u.Roles.Any(Function(r) r.RoleId = roleId)).ToList()
            list.Insert(0, New ApplicationUser With {.Id = "0", .UserName = "Selezionare Voce..."})
            ViewBag.IdUtente = New SelectList(list, "Id", "Username")
            'Section Ricerca Reparti
            Dim listReparti = db.Brighetti_Reparti.ToList
            listReparti.Insert(0, New Brighetti_Reparto With {.IdReparto = "0", .NomeReparto = "Selezionare Voce..."})
            ViewBag.IdReparto = New SelectList(listReparti, "IdReparto", "NomeReparto")
            Return PartialView(brighetti_Macchina)
        End Function

        ' POST: Brighetti_Macchina/Edit/5
        'Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        'Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="IdMacchina,NomeMacchina,DescrizioneMacchina,IdReparto,IdUtente,UltimaModifica")> ByVal brighetti_Macchina As Brighetti_Macchina) As JsonResult
            Dim Opid = vbNullString
            Dim OpName = vbNullString
            Try
                Opid = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                If ModelState.IsValid Then
                    Dim Mac = db.Brighetti_Macchine.Find(brighetti_Macchina.IdMacchina)
                    If Mac.IdReparto <> brighetti_Macchina.IdReparto Then
                        Mac.IdReparto = brighetti_Macchina.IdReparto
                        db.SaveChanges()
                    End If
                    If Mac.IdUtente <> brighetti_Macchina.IdUtente Then
                        Dim asp = appctx.MacchineUsers.Where(Function(x) x.IdUtente = Mac.IdUtente).FirstOrDefault
                        If Not IsNothing(asp) Then
                            asp.IdUtente = brighetti_Macchina.IdUtente
                            appctx.SaveChanges()
                        End If
                        Mac.IdUtente = brighetti_Macchina.IdUtente
                        db.SaveChanges()
                    End If
                    If Mac.DescrizioneMacchina <> brighetti_Macchina.DescrizioneMacchina Then
                        Mac.DescrizioneMacchina = brighetti_Macchina.DescrizioneMacchina
                        db.SaveChanges()
                    End If
                    If Mac.NomeMacchina <> brighetti_Macchina.NomeMacchina Then
                        Mac.NomeMacchina = brighetti_Macchina.NomeMacchina
                        db.SaveChanges()
                    End If
                    db.SaveChanges()
                    Mac.UltimaModifica = New TipoUltimaModifica With {
                        .Data = DateTime.Now,
                        .Operatore = OpName,
                        .OperatoreID = Opid
                    }
                    Return Json(New With {.ok = True, .message = "Macchina correttamente modificata"})
                End If
                Return Json(New With {.ok = False, .message = "Errore modifica macchina"})
            Catch ex As Exception
                db.Log.Add(New Log With {
                                 .Livello = TipoLogLivello.Warning,
                                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                                 .Messaggio = "Errore modifica Macchina: " & vbNewLine & ex.Message,
                                 .Dati = "",
                                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = Opid, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore modifica macchina"})
            End Try
            Return Json(New With {.ok = False, .message = "Errore modifica macchina"})
        End Function

        ' GET: Brighetti_Macchina/Delete/5
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Macchina As Brighetti_Macchina = db.Brighetti_Macchine.Find(id)
            If IsNothing(brighetti_Macchina) Then
                Return HttpNotFound()
            End If
            Return PartialView(brighetti_Macchina)
        End Function

        ' POST: Brighetti_Macchina/Delete/5
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal idMacchina As Integer) As ActionResult
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Try
                OpID = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim Mac As Brighetti_Macchina = db.Brighetti_Macchine.Find(idMacchina)
                db.Brighetti_Macchine.Remove(Mac)
                db.SaveChanges()
                Return Json(New With {.ok = True, .message = "Cancellazione Macchina confermata correttamente"})
            Catch ex As Exception
                db.Log.Add(New Log With {
                 .Livello = TipoLogLivello.Warning,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Errore cancellazione Macchina: " & vbNewLine & ex.Message,
                 .Dati = "",
                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore nella cancellazione della Macchina"})
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
