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
    Public Class Brighetti_GiacenzaController
        Inherits System.Web.Mvc.Controller

        Private db As New BrighettiModels
        <HttpPost()>
        <ValidateInput(False)>
        Function ServerProcessing(PostedData As DataTableAjaxPostModel) As JsonResult
            Dim OpID As String = vbNullString
            Dim OpName As String = vbNullString
            Dim CurrentDate As DateTime = Now
            Try
                OpID = User.Identity.GetUserId()
                OpName = User.Identity.GetUserName()

                Dim result As New List(Of Object)
                Dim data As IQueryable(Of Brighetti_Giacenza)
                data = db.Brighetti_Giacenze.OrderBy(Function(x) x.UltimaModifica.Data)

                'ricerca
                Try
                    If Not IsNothing(PostedData.search.value) Then
                        If Not PostedData.search.value.Contains(" ") Then 'singola parola
                            Dim search As String = PostedData.search.value
                            Dim w As Expressions.Expression(Of Func(Of Brighetti_Giacenza, Boolean)) = MakeWhereExpression(search)
                            w.Compile()
                            data = data.Where(w)
                        Else 'multiple
                            For Each term As String In PostedData.search.value.Split(" ")
                                Dim wpartial As Expressions.Expression(Of Func(Of Brighetti_Giacenza, Boolean)) = MakeWhereExpression(term)
                                wpartial.Compile()
                                data = data.Where(wpartial)
                            Next
                        End If
                    End If
                Catch ex As SystemException
                    db.Log.Add(New Log With {
                      .UltimaModifica = New TipoUltimaModifica With {.data = CurrentDate, .OperatoreID = OpID, .Operatore = OpName},
                      .Livello = TipoLogLivello.Errors,
                      .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                      .Messaggio = "Errore Ricerca -> " & ex.Message,
                      .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {PostedData})
                      })
                    db.SaveChanges()
                End Try

                'ordinamento
                Try
                    If Not IsNothing(PostedData.order) Then
                        If PostedData.order.Count = 0 Then
                            Dim o As Expressions.Expression(Of Func(Of Brighetti_Giacenza, String))
                            o = MakeOrderExpression(Nothing) 'default
                            o.Compile()
                            data = data.OrderBy(o)
                        ElseIf PostedData.order.Count = 1 Then 'singolo
                            Dim o As Expressions.Expression(Of Func(Of Brighetti_Giacenza, String)) = MakeOrderExpression(PostedData.order(0).column)
                            o.Compile()

                            Select Case PostedData.order(0).dir
                                Case "asc"
                                    data = data.OrderBy(o)
                                Case "desc"
                                    data = data.OrderByDescending(o)
                            End Select
                        ElseIf PostedData.order.Count = 2 Then 'doppio
                            Dim o As Expressions.Expression(Of Func(Of Brighetti_Giacenza, String)) = MakeOrderExpression(PostedData.order(0).column)
                            o.Compile()

                            Dim o2 As Expressions.Expression(Of Func(Of Brighetti_Giacenza, String)) = MakeOrderExpression(PostedData.order(1).column)
                            o2.Compile()

                            Select Case PostedData.order(0).dir
                                Case "asc"
                                    Select Case PostedData.order(1).dir
                                        Case "asc"
                                            data = data.OrderBy(o).ThenBy(o2)
                                        Case "desc"
                                            data = data.OrderBy(o).ThenByDescending(o2)
                                    End Select

                                Case "desc"
                                    Select Case PostedData.order(1).dir
                                        Case "asc"
                                            data = data.OrderByDescending(o).ThenBy(o2)
                                        Case "desc"
                                            data = data.OrderByDescending(o).ThenByDescending(o2)
                                    End Select
                            End Select
                        Else 'solo i primi tre
                            Dim o As Expressions.Expression(Of Func(Of Brighetti_Giacenza, String)) = MakeOrderExpression(PostedData.order(0).column)
                            o.Compile()

                            Dim o2 As Expressions.Expression(Of Func(Of Brighetti_Giacenza, String)) = MakeOrderExpression(PostedData.order(1).column)
                            o2.Compile()

                            Dim o3 As Expressions.Expression(Of Func(Of Brighetti_Giacenza, String)) = MakeOrderExpression(PostedData.order(2).column)
                            o3.Compile()

                            Select Case PostedData.order(0).dir
                                Case "asc"
                                    Select Case PostedData.order(1).dir
                                        Case "asc"
                                            Select Case PostedData.order(2).dir
                                                Case "asc"
                                                    data = data.OrderBy(o).ThenBy(o2).ThenBy(o3)
                                                Case "desc"
                                                    data = data.OrderBy(o).ThenBy(o2).ThenByDescending(o3)
                                            End Select
                                        Case "desc"
                                            Select Case PostedData.order(2).dir
                                                Case "asc"
                                                    data = data.OrderBy(o).ThenByDescending(o2).ThenBy(o3)
                                                Case "desc"
                                                    data = data.OrderBy(o).ThenByDescending(o2).ThenByDescending(o3)
                                            End Select
                                    End Select

                                Case "desc"
                                    Select Case PostedData.order(1).dir
                                        Case "asc"
                                            Select Case PostedData.order(2).dir
                                                Case "asc"
                                                    data = data.OrderByDescending(o).ThenBy(o2).ThenBy(o3)
                                                Case "desc"
                                                    data = data.OrderByDescending(o).ThenBy(o2).ThenByDescending(o3)
                                            End Select
                                        Case "desc"
                                            Select Case PostedData.order(2).dir
                                                Case "asc"
                                                    data = data.OrderByDescending(o).ThenByDescending(o2).ThenBy(o3)
                                                Case "desc"
                                                    data = data.OrderByDescending(o).ThenByDescending(o2).ThenByDescending(o3)
                                            End Select
                                    End Select
                            End Select
                        End If
                    Else
                        Dim o As Expressions.Expression(Of Func(Of Brighetti_Giacenza, String))
                        o = MakeOrderExpression(Nothing) 'default
                        o.Compile()
                        data = data.OrderBy(o)
                    End If
                Catch ex As SystemException
                    db.Log.Add(New Log With {
                     .UltimaModifica = New TipoUltimaModifica With {.data = CurrentDate, .OperatoreID = OpID, .Operatore = OpName},
                     .Livello = TipoLogLivello.Errors,
                     .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                     .Messaggio = "Errore Ordinamento -> " & ex.Message,
                     .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {PostedData})
                     })
                    db.SaveChanges()
                End Try

                'paginazione
                Dim filtered As Integer = 0
                Try
                    filtered = data.Count
                    If PostedData.length > 0 Then
                        data = data.Skip(PostedData.start).Take(PostedData.length)
                    End If
                Catch ex As SystemException
                    db.Log.Add(New Log With {
                                         .UltimaModifica = New TipoUltimaModifica With {.data = CurrentDate, .OperatoreID = OpID, .Operatore = OpName},
                                         .Livello = TipoLogLivello.Errors,
                                         .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                                         .Messaggio = "Errore Paginazione -> " & ex.Message,
                                         .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {PostedData})
                                         })
                    db.SaveChanges()
                End Try
                'esecuzione (spero)
                For Each gia As Brighetti_Giacenza In data.ToList
                    Try
                        Dim idArt = Convert.ToInt32(gia.CodiceArticolo)
                        Dim idMag = Convert.ToInt32(gia.CodiceMagazzino)
                        Dim codArt = db.Brighetti_Articoli.Find(idArt)
                        Dim codMag = db.Brighetti_Magazzini.Find(idMag)
                        Dim finalButtons = "<button data-type='create_odp' data-value='" + gia.Id.ToString + "' Class='fa-gradient  btn  w-auto' style='color:white;' data-bs-toggle='modal' data-bs-target='#exampleModal'>
                        <i class='fa-solid fa-plus'></i>
                    </button>
                    <button data-type='edit_giacenza' data-value='" + gia.Id.ToString + "' Class='fa-gradient  btn  w-auto' style='color:white;' data-bs-toggle='modal' data-bs-target='#exampleModal'>
                        <i class='fa-solid fa-pen-to-square'></i>
                    </button>
                    <button data-type='details_giacenza' data-value='" + gia.Id.ToString + "' Class='fa-gradient  btn  w-auto' style='color:white;' data-bs-toggle='modal' data-bs-target='#exampleModal'>
                        <i class='fa-solid fa-magnifying-glass'></i>
                    </button>
                    <button data-type='delete_giacenza' data-value='" + gia.Id.ToString + "' Class='fa-gradient  btn  w-auto' style='color:white;' data-bs-toggle='modal' data-bs-target='#exampleModal'>
                        <i class='fa-solid fa-trash'></i>
                    </button>"
                        result.Add(New With {
                                .DT_RowData = New With {.value = gia.Id},
                                .DT_RowId = "row_" & gia.Id,
                                .CodiceArticolo = codArt.CodiceArticolo,
                                .CodiceMagazzino = codMag.CodiceMagazzino,
                                .Id = gia.Id,
                                .QuantitàGiacenza = gia.QuantitàGiacenza,
                                .QuantitàSottoscorta = gia.QuantitàSottoscorta,
                                .UltimaModifica = gia.UltimaModifica.Data.ToString.Split(" ")(0),
                                .Azioni = finalButtons
                           })


                    Catch ex As SystemException
                        db.Log.Add(New Log With {
                             .UltimaModifica = New TipoUltimaModifica With {.data = CurrentDate, .OperatoreID = OpID, .Operatore = OpName},
                             .Livello = TipoLogLivello.Errors,
                             .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                             .Messaggio = "Errore Creazione Lista Giacenze (" & gia.Id & ") -> " & ex.Message & " [" & ex.InnerException.Message & "]",
                             .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {PostedData})
                        })
                        db.SaveChanges()
                    End Try
                Next

                Return Json(New With {PostedData.draw, .recordsTotal = db.Brighetti_Giacenze.Count, .recordsFiltered = filtered, .data = result})
            Catch ex As SystemException
                db.Log.Add(New Log With {
                     .UltimaModifica = New TipoUltimaModifica With {.Data = CurrentDate, .OperatoreID = OpID, .Operatore = OpName},
                     .Livello = TipoLogLivello.Errors,
                     .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                     .Messaggio = "Errore Generico -> " & ex.Message,
                     .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {PostedData})
                     })
                db.SaveChanges()
            End Try
            Return Json(New With {PostedData.draw, .recordsTotal = db.Brighetti_Giacenze.Count, .recordsFiltered = 0})
        End Function
        ' GET: Brighetti_Giacenza
        Function Index(Optional id As Integer = 0) As ActionResult
            Dim listaRet As New List(Of Brighetti_Giacenza)
            Return View(listaRet)
        End Function

        ' GET: Brighetti_Giacenza/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Giacenza As Brighetti_Giacenza = db.Brighetti_Giacenze.Find(id)
            If IsNothing(brighetti_Giacenza) Then
                Return HttpNotFound()
            End If
            Return PartialView(brighetti_Giacenza)
        End Function

        ' GET: Brighetti_Giacenza/Create
        Function Create() As ActionResult
            Dim CodiceMagazzino = db.Brighetti_Magazzini.ToList
            CodiceMagazzino.Insert(0, New Brighetti_Magazzino With {.Id = 0, .CodiceMagazzino = "Selezionare Voce..."})
            ViewBag.CodiceMagazzino = New SelectList(CodiceMagazzino, "Id", "CodiceMagazzino")
            Dim CodiceArticolo = db.Brighetti_Articoli.ToList
            CodiceArticolo.Insert(0, New Brighetti_Articolo With {.Id = 0, .CodiceArticolo = "Selezionare Voce..."})
            ViewBag.CodiceArticolo = New SelectList(CodiceArticolo, "Id", "CodiceArticolo")
            Return PartialView()
        End Function
        Function CreateODP() As ActionResult
            Dim CodiceArticolo = db.Brighetti_Articoli.ToList
            CodiceArticolo.Insert(0, New Brighetti_Articolo With {.Id = 0, .CodiceArticolo = "Selezionare Voce..."})
            ViewBag.IdArticolo = New SelectList(CodiceArticolo, "Id", "CodiceArticolo")
            Return PartialView()
        End Function
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function CreateODP(<Bind(Include:="IdArticolo,Qta")> ByVal CreaOdpViewModel As CreaOdpViewModel) As ActionResult
            Dim Opid = vbNullString
            Dim OpName = vbNullString
            Try
                Opid = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                If ModelState.IsValid Then
                    Dim art = db.Brighetti_Articoli.Find(CreaOdpViewModel.IdArticolo)
                    Dim listaProcedura = db.Brighetti_Procedure.Where(Function(x) x.CodiceArticolo = art.CodiceArticolo).ToList
                    Dim first = True
                    For Each l In listaProcedura
                        If first Then
                            db.Brighetti_Attività.Add(New Brighetti_Attività With {
                            .CodiceArticolo = l.CodiceArticolo,
                            .idMacchinaMagazzino = l.idMacchinaMagazzino,
                            .idReparto = l.idReparto,
                            .IncrementaleProcedura = l.IncrementaleProcedura,
                            .NomeAttività = l.NomeAttività,
                            .TipoAttivitaProcedura = l.TipoAttivita,
                            .OrdineDiProduzione = DateTime.Now.ToString.Split(" ")(0) + "_" + listaProcedura.First.IdProcedura.ToString,
                            .QuantitàProdotta = 0,
                            .QuantitàdaProdurre = CreaOdpViewModel.Qta,
                            .QuantitàScartata = 0,
                            .StatoAttività = TipoStatoAttività.In_attesa,
                            .UltimaModifica = New TipoUltimaModifica With {
                                .Data = DateTime.Now,
                                .Operatore = OpName,
                                .OperatoreID = Opid
                        }
                        })
                            first = False
                            db.SaveChanges()
                        Else
                            db.Brighetti_Attività.Add(New Brighetti_Attività With {
                            .CodiceArticolo = l.CodiceArticolo,
                            .idMacchinaMagazzino = l.idMacchinaMagazzino,
                            .idReparto = l.idReparto,
                            .IncrementaleProcedura = l.IncrementaleProcedura,
                            .TipoAttivitaProcedura = l.TipoAttivita,
                            .NomeAttività = l.NomeAttività,
                            .OrdineDiProduzione = DateTime.Now.ToString.Split(" ")(0) + "_" + listaProcedura.First.IdProcedura.ToString,
                            .QuantitàProdotta = 0,
                            .QuantitàdaProdurre = CreaOdpViewModel.Qta,
                            .QuantitàScartata = 0,
                            .StatoAttività = TipoStatoAttività.BloccoDaAttivitàPrecedente,
                            .UltimaModifica = New TipoUltimaModifica With {
                                .Data = DateTime.Now,
                                .Operatore = OpName,
                                .OperatoreID = Opid
                        }
                        })
                            db.SaveChanges()
                        End If

                    Next
                    Return Json(New With {.ok = True, .message = "ODP Creato Correttamente"})
                End If
                Return Json(New With {.ok = False, .message = "Errore creazione ODP"})
            Catch ex As Exception
                db.Log.Add(New Log With {
                                 .Livello = TipoLogLivello.Warning,
                                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                                 .Messaggio = "Errore creazione giacenza ODP: " & vbNewLine & ex.Message,
                                 .Dati = "",
                                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = Opid, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore creazione ODP"})

            End Try
            Return Json(New With {.ok = False, .message = "Errore creazione ODP"})
        End Function
        ' POST: Brighetti_Giacenza/Create
        'Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        'Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="Id,CodiceArticolo,CodiceMagazzino,QuantitàGiacenza,QuantitàSottoscorta,UltimaModifica")> ByVal brighetti_Giacenza As Brighetti_Giacenza) As ActionResult
            Dim Opid = vbNullString
            Dim OpName = vbNullString
            Try
                Opid = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                If ModelState.IsValid Then
                    db.Brighetti_Giacenze.Add(New Brighetti_Giacenza With {
                        .CodiceArticolo = brighetti_Giacenza.CodiceArticolo,
                        .CodiceMagazzino = brighetti_Giacenza.CodiceMagazzino,
                        .QuantitàGiacenza = brighetti_Giacenza.QuantitàGiacenza,
                        .QuantitàSottoscorta = brighetti_Giacenza.QuantitàSottoscorta,
                        .UltimaModifica = New TipoUltimaModifica With {
                        .Data = DateTime.Now,
                        .Operatore = OpName,
                        .OperatoreID = Opid
                    }
                    })
                    db.SaveChanges()
                    Return Json(New With {.ok = True, .message = "Giacenza correttamente inserita"})
                End If
                Return Json(New With {.ok = False, .message = "Errore creazione giacenza"})
            Catch ex As Exception
                db.Log.Add(New Log With {
                                 .Livello = TipoLogLivello.Warning,
                                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                                 .Messaggio = "Errore creazione giacenza articolo: " & vbNewLine & ex.Message,
                                 .Dati = "",
                                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = Opid, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore creazione giacenza"})
            End Try
            Return Json(New With {.ok = False, .message = "Errore creazione giacenza"})
        End Function

        ' GET: Brighetti_Giacenza/Edit/5
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Giacenza As Brighetti_Giacenza = db.Brighetti_Giacenze.Find(id)
            Dim CodiceMagazzino = db.Brighetti_Magazzini.ToList
            CodiceMagazzino.Insert(0, New Brighetti_Magazzino With {.id = 0, .CodiceMagazzino = "Selezionare Voce..."})
            ViewBag.CodiceMagazzino = New SelectList(CodiceMagazzino, "Id", "CodiceMagazzino")
            Dim CodiceArticolo = db.Brighetti_Articoli.ToList
            CodiceArticolo.Insert(0, New Brighetti_Articolo With {.id = 0, .CodiceArticolo = "Selezionare Voce..."})
            ViewBag.CodiceArticolo = New SelectList(CodiceArticolo, "Id", "CodiceArticolo")
            If IsNothing(brighetti_Giacenza) Then
                Return HttpNotFound()
            End If
            Return PartialView(brighetti_Giacenza)
        End Function

        ' POST: Brighetti_Giacenza/Edit/5
        'Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        'Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="Id,CodiceArticolo,CodiceMagazzino,QuantitàGiacenza,QuantitàSottoscorta,UltimaModifica")> ByVal brighetti_Giacenza As Brighetti_Giacenza) As ActionResult
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Try
                OpID = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                If ModelState.IsValid Then
                    Dim giacenza = db.Brighetti_Giacenze.Find(brighetti_Giacenza.Id)
                    If IsNothing(giacenza) Then
                        Return Json(New With {.ok = False, .message = "Impossibile trovare l'articolo richiesto"})
                        db.SaveChanges()
                    End If
                    If giacenza.CodiceArticolo <> giacenza.CodiceArticolo Then
                        giacenza.CodiceArticolo = giacenza.CodiceArticolo
                        db.SaveChanges()
                    End If
                    If giacenza.CodiceMagazzino <> giacenza.CodiceMagazzino Then
                        giacenza.CodiceMagazzino = giacenza.CodiceMagazzino
                        db.SaveChanges()
                    End If
                    If giacenza.QuantitàGiacenza <> giacenza.QuantitàGiacenza Then
                        giacenza.QuantitàGiacenza = giacenza.QuantitàGiacenza
                        db.SaveChanges()
                    End If
                    If giacenza.QuantitàSottoscorta <> giacenza.QuantitàSottoscorta Then
                        giacenza.QuantitàSottoscorta = giacenza.QuantitàSottoscorta
                        db.SaveChanges()
                    End If
                    giacenza.UltimaModifica = New TipoUltimaModifica With {
                        .Data = DateTime.Now,
                        .Operatore = OpName,
                        .OperatoreID = OpID
                    }
                    db.SaveChanges()
                    Return Json(New With {.ok = True, .message = "Giacenza correttamente aggiornata"})
                End If
            Catch ex As Exception
                db.Log.Add(New Log With {
               .Livello = TipoLogLivello.Warning,
               .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
               .Messaggio = "Errore modifica giacenza articolo: " & vbNewLine & ex.Message,
               .Dati = "",
               .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Impossibile aggiornare giacenza"})
            End Try
            Return Json(New With {.ok = False, .message = "Impossibile aggiornare giacenza"})
        End Function

        ' GET: Brighetti_Giacenza/Delete/5
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Giacenza As Brighetti_Giacenza = db.Brighetti_Giacenze.Find(id)
            If IsNothing(brighetti_Giacenza) Then
                Return HttpNotFound()
            End If
            Return PartialView(brighetti_Giacenza)
        End Function

        ' POST: Brighetti_Giacenza/Delete/5
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As Integer) As ActionResult
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Try
                OpID = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim brighetti_Giacenza As Brighetti_Giacenza = db.Brighetti_Giacenze.Find(id)
                db.Brighetti_Giacenze.Remove(brighetti_Giacenza)
                db.SaveChanges()
                Return Json(New With {.ok = True, .message = "Cancellazione giacenza confermata correttamente"})
            Catch ex As Exception
                db.Log.Add(New Log With {
                 .Livello = TipoLogLivello.Warning,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Errore cancellazione giacenza: " & vbNewLine & ex.Message,
                 .Dati = "",
                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore nella cancellazione giacenza"})

            End Try
        End Function
        Private Function MakeWhereExpression(Search As String) As Expressions.Expression(Of Func(Of Brighetti_Giacenza, Boolean))
            Return Function(x) x.CodiceArticolo.Contains(Search) Or
                               x.CodiceMagazzino.Contains(Search)
        End Function
        Private Function MakeOrderExpression(Column As Integer) As Expressions.Expression(Of Func(Of Brighetti_Giacenza, String))
            Select Case Column
                Case Nothing : Return Function(x) x.CodiceArticolo
                Case 1 : Return Function(x) x.CodiceArticolo
                Case 2 : Return Function(x) x.CodiceMagazzino
                Case Else : Return Function(x) x.CodiceArticolo
            End Select
        End Function
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace
