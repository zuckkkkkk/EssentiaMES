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
    Public Class Brighetti_ArticoloController
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
                Dim data As IQueryable(Of Brighetti_Articolo)
                data = db.Brighetti_Articoli.OrderBy(Function(x) x.UltimaModifica.Data)

                'ricerca
                Try
                    If Not IsNothing(PostedData.search.value) Then
                        If Not PostedData.search.value.Contains(" ") Then 'singola parola
                            Dim search As String = PostedData.search.value
                            Dim w As Expressions.Expression(Of Func(Of Brighetti_Articolo, Boolean)) = MakeWhereExpression(search)
                            w.Compile()
                            data = data.Where(w)
                        Else 'multiple
                            For Each term As String In PostedData.search.value.Split(" ")
                                Dim wpartial As Expressions.Expression(Of Func(Of Brighetti_Articolo, Boolean)) = MakeWhereExpression(term)
                                wpartial.Compile()
                                data = data.Where(wpartial)
                            Next
                        End If
                    End If
                Catch ex As SystemException
                    db.Log.Add(New Log With {
                      .UltimaModifica = New TipoUltimaModifica With {.Data = CurrentDate, .OperatoreID = OpID, .Operatore = OpName},
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
                            Dim o As Expressions.Expression(Of Func(Of Brighetti_Articolo, String))
                            o = MakeOrderExpression(Nothing) 'default
                            o.Compile()
                            data = data.OrderBy(o)
                            'data = data.OrderBy(CreateExpression(Of Amministratore)("Studio"))
                            'data = OrderByDynamic(data, "Studio", False)
                        ElseIf PostedData.order.Count = 1 Then 'singolo
                            Dim o As Expressions.Expression(Of Func(Of Brighetti_Articolo, String)) = MakeOrderExpression(PostedData.order(0).column)
                            o.Compile()
                            'If IsNothing(PostedData.columns(PostedData.order(0).column).data) Then
                            '    data = OrderByDynamic(data, "Studio", False)
                            'Else
                            '    data = OrderByDynamic(data, PostedData.columns(PostedData.order(0).column).data, PostedData.order(0).dir = "desc")
                            'End If

                            Select Case PostedData.order(0).dir
                                Case "asc"
                                    data = data.OrderBy(o)
                                Case "desc"
                                    data = data.OrderByDescending(o)
                            End Select
                        ElseIf PostedData.order.Count = 2 Then 'doppio
                            Dim o As Expressions.Expression(Of Func(Of Brighetti_Articolo, String)) = MakeOrderExpression(PostedData.order(0).column)
                            o.Compile()

                            Dim o2 As Expressions.Expression(Of Func(Of Brighetti_Articolo, String)) = MakeOrderExpression(PostedData.order(1).column)
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
                            Dim o As Expressions.Expression(Of Func(Of Brighetti_Articolo, String)) = MakeOrderExpression(PostedData.order(0).column)
                            o.Compile()

                            Dim o2 As Expressions.Expression(Of Func(Of Brighetti_Articolo, String)) = MakeOrderExpression(PostedData.order(1).column)
                            o2.Compile()

                            Dim o3 As Expressions.Expression(Of Func(Of Brighetti_Articolo, String)) = MakeOrderExpression(PostedData.order(2).column)
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
                        Dim o As Expressions.Expression(Of Func(Of Brighetti_Articolo, String))
                        o = MakeOrderExpression(Nothing) 'default
                        o.Compile()
                        data = data.OrderBy(o)
                    End If
                Catch ex As SystemException
                    db.Log.Add(New Log With {
                     .UltimaModifica = New TipoUltimaModifica With {.Data = CurrentDate, .OperatoreID = OpID, .Operatore = OpName},
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
                                         .UltimaModifica = New TipoUltimaModifica With {.Data = CurrentDate, .OperatoreID = OpID, .Operatore = OpName},
                                         .Livello = TipoLogLivello.Errors,
                                         .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                                         .Messaggio = "Errore Paginazione -> " & ex.Message,
                                         .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {PostedData})
                                         })
                    db.SaveChanges()
                End Try
                'esecuzione (spero)
                For Each art As Brighetti_Articolo In data.ToList
                    Try
                        Dim listaNote = db.Brighetti_Note.Where(Function(x) x.IdEsterno = art.Id And x.TypeElem = TipoElemento.Articolo).ToList
                        Dim listaDocumenti = db.Brighetti_Documenti.Where(Function(x) x.IdEsterno = art.Id And x.TypeElem = TipoElemento.Articolo).ToList
                        Dim finalButtons = ""
                        Dim artFamId = Convert.ToInt32(art.FamigliaId)
                        Dim Famiglia = db.Brighetti_FamiglieProdotto.Where(Function(x) x.Id = artFamId).FirstOrDefault
                        Dim NomeFam = "-"
                        If Not IsNothing(Famiglia) Then
                            NomeFam = Famiglia.NomeFamiglia
                        End If
                        Dim ImportanzaArticoloFin = ""
                        Select Case art.Importanza
                            Case ImportanzaArticolo.Nessuna
                                ImportanzaArticoloFin = "<i class='fa-solid fa-star' style='color: #ffdd00;'></i>" +
                                                "<i class='fa-solid fa-star'></i>" +
                                                "<i class='fa-solid fa-star'></i>" +
                                                "<i class='fa-solid fa-star'></i>" +
                                                "<i class='fa-solid fa-star'></i>"
                            Case ImportanzaArticolo.Bassa
                                ImportanzaArticoloFin = "<i class='fa-solid fa-star' style='color: #ffdd00;'></i>" +
                                                "<i class='fa-solid fa-star' style='color: #ffdd00;'></i>" +
                                                "<i class='fa-solid fa-star'></i>" +
                                                "<i class='fa-solid fa-star'></i>" +
                                                "<i class='fa-solid fa-star'></i>"
                            Case ImportanzaArticolo.Intermedia
                                ImportanzaArticoloFin = "<i class='fa-solid fa-star' style='color: #ffdd00;'></i>" +
                                                "<i class='fa-solid fa-star' style='color: #ffdd00;'></i>" +
                                                "<i class='fa-solid fa-star' style='color: #ffdd00;'></i>" +
                                                "<i class='fa-solid fa-star'></i>" +
                                                "<i class='fa-solid fa-star'></i>"
                            Case ImportanzaArticolo.Alta
                                ImportanzaArticoloFin = "<i class='fa-solid fa-star' style='color: #ffdd00;'></i>" +
                                                "<i class='fa-solid fa-star' style='color: #ffdd00;'></i>" +
                                                "<i class='fa-solid fa-star' style='color: #ffdd00;'></i>" +
                                                "<i class='fa-solid fa-star' style='color: #ffdd00;'></i>" +
                                                "<i class='fa-solid fa-star'></i>"
                            Case ImportanzaArticolo.Massima
                                ImportanzaArticoloFin = "<i class='fa-solid fa-star' style='color: #ffdd00;'></i>" +
                                                "<i class='fa-solid fa-star' style='color: #ffdd00;'></i>" +
                                                "<i class='fa-solid fa-star' style='color: #ffdd00;'></i>" +
                                                "<i class='fa-solid fa-star' style='color: #ffdd00;'></i>" +
                                                "<i class='fa-solid fa-star' style='color: #ffdd00;'></i>"
                        End Select

                        finalButtons = "<div class='row'>
                        <div class='col-3'>
                            <button data-type='edit_articolo' data-value='" + art.Id.ToString + "' Class='fa-gradient  btn  w-auto' style='colorwhite;' data-bs-toggle='modal' data-bs-target='#exampleModal'>
                                <i class='fa-solid fa-pen-To-square'></i>
                            </button>
                        </div>
                        <div class='col-3'>
                            <button onclick=""AddNota('" + art.Id.ToString + "', 1);"" type='button' data-value='" + art.Id.ToString + "' Class='fa-gradient  btn  w-auto'>
                                <i Class='fa-solid fa-note-sticky'></i>
                            </button>
                        </div>
                        <div Class='col-3'>
                            <Button onclick=""AddFile('" + art.Id.ToString + "', 1);"" type='button' data-value='" + art.Id.ToString + "' Class='fa-gradient  btn  w-auto'>
                                <i Class='fa-solid fa-file-arrow-up'></i>
                            </button>
                        </div>
                        <div Class='col-3'>
                            <Button data-type='details_articolo' data-value='" + art.Id.ToString + "' Class='fa-gradient  btn  w-auto' style='color:white;' data-bs-toggle='modal' data-bs-target='#exampleModal'>
                                <i Class='fa-solid fa-magnifying-glass'></i>
                            </button>
                        </div>
                    </div>"
                        result.Add(New With {
                                .DT_RowData = New With {.value = art.Id},
                                .DT_RowId = "row_" & art.Id,
                                .Id = art.Id,
                                .FamigliaId = NomeFam,
                                .CodiceArticolo = art.CodiceArticolo,
                                .DescrizioneArticolo = art.DescrizioneArticolo,
                                .NoteArticolo = art.NoteArticolo,
                                .UltimaModifica = art.UltimaModifica.Data.ToString.Split(" ")(0),
                                .Importanza = ImportanzaArticoloFin,
                                .Azioni = finalButtons
                           })


                    Catch ex As SystemException
                        db.Log.Add(New Log With {
                             .UltimaModifica = New TipoUltimaModifica With {.Data = CurrentDate, .OperatoreID = OpID, .Operatore = OpName},
                             .Livello = TipoLogLivello.Errors,
                             .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                             .Messaggio = "Errore Creazione Lista Impianto (" & art.Id & ") -> " & ex.Message & " [" & ex.InnerException.Message & "]",
                             .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {PostedData})
                        })
                        db.SaveChanges()
                    End Try
                Next

                Return Json(New With {PostedData.draw, .recordsTotal = db.Brighetti_Articoli.Count, .recordsFiltered = filtered, .data = result})
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
            Return Json(New With {PostedData.draw, .recordsTotal = db.Brighetti_Articoli.Count, .recordsFiltered = 0})
        End Function
        ' GET: Brighetti_Articolo
        Function Index() As ActionResult
            Dim retList = db.Brighetti_Articoli.ToList
            Dim finalList As New List(Of Brighetti_Articolo_ViewModel)

            Return View(finalList)
        End Function

        ' GET: Brighetti_Articolo/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim b As Brighetti_Articolo = db.Brighetti_Articoli.Find(id)
            Dim listaNote = db.Brighetti_Note.Where(Function(x) x.IdEsterno = b.Id And x.TypeElem = TipoElemento.Articolo).ToList
            Dim listaDocumenti = db.Brighetti_Documenti.Where(Function(x) x.IdEsterno = b.Id And x.TypeElem = TipoElemento.Articolo).ToList
            Dim listaProcedura = db.Brighetti_Procedure.Where(Function(x) x.CodiceArticolo = b.CodiceArticolo).ToList
            Dim retArt As New Brighetti_Articoli_Viewmodel With {
                .CodiceArticolo = b.CodiceArticolo,
                .DescrizioneArticolo = b.DescrizioneArticolo,
                .FamigliaId = b.FamigliaId,
                .Id = b.Id,
                .Importanza = b.Importanza,
                .NoteArticolo = b.NoteArticolo,
                .UltimaModifica = b.UltimaModifica,
                .ListaNote = listaNote,
                .ListaDocumenti = listaDocumenti,
                .ListaProcedura = listaProcedura
            }
            If IsNothing(b) Then
                Return HttpNotFound()
            End If
            Return PartialView(retArt)
        End Function

        ' GET: Brighetti_Articolo/Create
        Function Create() As ActionResult
            Dim Famiglia = db.Brighetti_FamiglieProdotto.ToList
            Famiglia.Insert(0, New Brighetti_FamiglieProdotto With {.Id = 0, .NomeFamiglia = "Selezionare Voce..."})
            ViewBag.FamigliaId = New SelectList(Famiglia, "Id", "NomeFamiglia")
            Return PartialView()
        End Function
        ' POST: Brighetti_Articolo/Create
        'Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        'Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="Id,CodiceArticolo,DescrizioneArticolo,NoteArticolo,FamigliaId,Importanza,LottoMinimo")> ByVal brighetti_Articolo As Brighetti_Articolo) As JsonResult
            Dim Opid = vbNullString
            Dim OpName = vbNullString
            Try
                Opid = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                If ModelState.IsValid Then
                    db.Brighetti_Articoli.Add(New Brighetti_Articolo With {
                    .CodiceArticolo = brighetti_Articolo.CodiceArticolo,
                    .DescrizioneArticolo = brighetti_Articolo.DescrizioneArticolo,
                    .NoteArticolo = brighetti_Articolo.NoteArticolo,
                    .FamigliaId = brighetti_Articolo.FamigliaId,
                    .Importanza = brighetti_Articolo.Importanza,
                    .LottoMinimo = brighetti_Articolo.LottoMinimo,
                    .UltimaModifica = New TipoUltimaModifica With {
                        .Data = DateTime.Now,
                        .Operatore = OpName,
                        .OperatoreID = Opid
                }
                })
                    db.SaveChanges()
                    Dim a = db.Brighetti_Articoli.Where(Function(x) x.CodiceArticolo = brighetti_Articolo.CodiceArticolo).FirstOrDefault
                    If Not IsNothing(a) Then
                        If db.Brighetti_Procedure.Where(Function(x) x.CodiceArticolo = a.CodiceArticolo).Count = 0 Then
                            Select Case True
                            'Case Brocce
                                Case a.CodiceArticolo.StartsWith("G") Or a.CodiceArticolo.StartsWith("GT") Or a.CodiceArticolo.StartsWith("GP") Or a.CodiceArticolo.StartsWith("PCM") Or a.CodiceArticolo.StartsWith("SG") Or a.CodiceArticolo.StartsWith("SGT") Or a.CodiceArticolo.StartsWith("SGP") Or a.CodiceArticolo.StartsWith("SGPCM")
                                    Try
                                        Dim idMacchinaTornitura = db.Brighetti_Macchine.Where(Function(x) x.NomeMacchina = "Mazak 200MSY L").FirstOrDefault
                                        '================================================================= Lavorazione Tornitura
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 1,
                                            .TipoAttivita = TipoAttivitaProcedura.Lavorazione,
                                            .idMacchinaMagazzino = idMacchinaTornitura.IdMacchina,
                                            .idReparto = idMacchinaTornitura.IdReparto,
                                            .NomeAttività = "Tornitura " + a.CodiceArticolo,
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                        }
                                                            })
                                        '================================================================= Versamento da Tornitura a Fresatura
                                        Dim idMagazzinoFresatura = db.Brighetti_Magazzini.Where(Function(x) x.CodiceMagazzino = "Magazzino Fresatura").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 2,
                                            .TipoAttivita = TipoAttivitaProcedura.Versamento,
                                            .idMacchinaMagazzino = idMagazzinoFresatura.Id,
                                            .idReparto = 0,
                                            .NomeAttività = "Versamento da Tornitura a Fresatura",
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                        }
                                                            })
                                        '================================================================= Lavorazione Fresatura
                                        Dim idMacchinaFresatura = db.Brighetti_Macchine.Where(Function(x) x.NomeMacchina = "Mazak VCN-530C").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 3,
                                            .idMacchinaMagazzino = idMacchinaFresatura.IdMacchina,
                                            .TipoAttivita = TipoAttivitaProcedura.Lavorazione,
                                            .idReparto = idMacchinaFresatura.IdReparto,
                                            .NomeAttività = "Fresatura " + a.CodiceArticolo,
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                            }
                                        })
                                        '================================================================= Versamento da Fresatura a Trattamento
                                        Dim idMagazzinoTrattamento = db.Brighetti_Magazzini.Where(Function(x) x.CodiceMagazzino = "Magazzino Tratt. Termico").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 4,
                                            .TipoAttivita = TipoAttivitaProcedura.Versamento,
                                            .idMacchinaMagazzino = idMagazzinoTrattamento.Id,
                                            .idReparto = 0,
                                            .NomeAttività = "Versamento da Fresatura a Tratt. Termico",
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                        }
                                                            })
                                        '================================================================= Lavorazione Trattamento
                                        Dim idMacchinaTrattamentoEsterno = db.Brighetti_Macchine.Where(Function(x) x.NomeMacchina = "Macchina_Trattamento").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 5,
                                            .idMacchinaMagazzino = idMacchinaTrattamentoEsterno.IdMacchina,
                                            .TipoAttivita = TipoAttivitaProcedura.Lavorazione,
                                            .idReparto = idMacchinaTrattamentoEsterno.IdReparto,
                                            .NomeAttività = "Tratt. Termico",
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                            }
                                        })
                                        '================================================================= Versamento da Trattamento a Rettifica
                                        Dim idMagazzinoRettifica = db.Brighetti_Magazzini.Where(Function(x) x.CodiceMagazzino = "Magazzino Rettifica").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 6,
                                            .TipoAttivita = TipoAttivitaProcedura.Versamento,
                                            .idMacchinaMagazzino = idMagazzinoTrattamento.Id,
                                            .idReparto = 0,
                                            .NomeAttività = "Versamento da Trattamento a Rettifica",
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                        }
                                                            })
                                        '================================================================= Lavorazione Rettifica
                                        Dim idMacchinaRettifica = db.Brighetti_Macchine.Where(Function(x) x.NomeMacchina = "Studer S33").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 7,
                                            .idMacchinaMagazzino = idMacchinaRettifica.IdMacchina,
                                            .TipoAttivita = TipoAttivitaProcedura.Lavorazione,
                                            .idReparto = idMacchinaRettifica.IdReparto,
                                            .NomeAttività = "Rettifica " + a.CodiceArticolo,
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                            }
                                        })
                                        '================================================================= Versamento da Rettifica ad Affilatura
                                        Dim idMagazzinoAffilatura = db.Brighetti_Magazzini.Where(Function(x) x.CodiceMagazzino = "Magazzino Affilatura").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 8,
                                            .TipoAttivita = TipoAttivitaProcedura.Versamento,
                                            .idMacchinaMagazzino = idMagazzinoAffilatura.Id,
                                            .idReparto = 0,
                                            .NomeAttività = "Versamento da Rettifica ad Affilatura",
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                        }
                                                            })
                                        '================================================================= Lavorazione Affilatura
                                        Dim idMacchinaAffilatura = db.Brighetti_Macchine.Where(Function(x) x.NomeMacchina = "Schneberger NGM/81").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 9,
                                            .idMacchinaMagazzino = idMacchinaAffilatura.IdMacchina,
                                            .TipoAttivita = TipoAttivitaProcedura.Lavorazione,
                                            .idReparto = idMacchinaAffilatura.IdReparto,
                                            .NomeAttività = "Affilatura " + a.CodiceArticolo,
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                            }
                                        })
                                        '================================================================= Versamento da Affilatura a Finito
                                        Dim idMagazzinoFinito = db.Brighetti_Magazzini.Where(Function(x) x.CodiceMagazzino = "Magazzino Finito").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 10,
                                            .TipoAttivita = TipoAttivitaProcedura.Versamento,
                                            .idMacchinaMagazzino = idMagazzinoFinito.Id,
                                            .idReparto = 0,
                                            .NomeAttività = "Versamento da Affilatura a Finito",
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                        }
                                                            })
                                        db.SaveChanges()
                                    Catch ex As Exception

                                    End Try
                                Case a.CodiceArticolo.Contains("B") Or a.CodiceArticolo.Contains("BA") Or a.CodiceArticolo.Contains("BC") Or a.CodiceArticolo.Contains("BL") Or a.CodiceArticolo.Contains("BG") Or a.CodiceArticolo.Contains("BE") Or a.CodiceArticolo.Contains("SB") Or a.CodiceArticolo.Contains("SBA") Or a.CodiceArticolo.Contains("SBC") Or a.CodiceArticolo.Contains("SBL") Or a.CodiceArticolo.Contains("SBE")
                                    Try
                                        Dim idMacchinaTornitura = db.Brighetti_Macchine.Where(Function(x) x.NomeMacchina = "Mazak 250 MSY").FirstOrDefault
                                        '================================================================= Lavorazione Tornitura
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 1,
                                            .TipoAttivita = TipoAttivitaProcedura.Lavorazione,
                                            .idMacchinaMagazzino = idMacchinaTornitura.IdMacchina,
                                            .idReparto = idMacchinaTornitura.IdReparto,
                                            .NomeAttività = "Tornitura " + a.CodiceArticolo,
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                        }
                                                            })
                                        '================================================================= Versamento da Tornitura a Fresatura
                                        Dim idMagazzinoFresatura = db.Brighetti_Magazzini.Where(Function(x) x.CodiceMagazzino = "Magazzino Fresatura").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 2,
                                            .TipoAttivita = TipoAttivitaProcedura.Versamento,
                                            .idMacchinaMagazzino = idMagazzinoFresatura.Id,
                                            .idReparto = 0,
                                            .NomeAttività = "Versamento da Tornitura a Fresatura",
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                        }
                                                            })
                                        '================================================================= Lavorazione Fresatura
                                        Dim idMacchinaFresatura = db.Brighetti_Macchine.Where(Function(x) x.NomeMacchina = "Mazak VCN-530C").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 3,
                                            .idMacchinaMagazzino = idMacchinaFresatura.IdMacchina,
                                            .TipoAttivita = TipoAttivitaProcedura.Lavorazione,
                                            .idReparto = idMacchinaFresatura.IdReparto,
                                            .NomeAttività = "Fresatura " + a.CodiceArticolo,
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                            }
                                        })
                                        '================================================================= Versamento da Fresatura a Trattamento
                                        Dim idMagazzinoTrattamento = db.Brighetti_Magazzini.Where(Function(x) x.CodiceMagazzino = "Magazzino Tratt. Termico").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 4,
                                            .TipoAttivita = TipoAttivitaProcedura.Versamento,
                                            .idMacchinaMagazzino = idMagazzinoTrattamento.Id,
                                            .idReparto = 0,
                                            .NomeAttività = "Versamento da Fresatura a Tratt. Termico",
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                        }
                                                            })
                                        '================================================================= Lavorazione Trattamento
                                        Dim idMacchinaTrattamentoEsterno = db.Brighetti_Macchine.Where(Function(x) x.NomeMacchina = "Macchina_Trattamento").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 5,
                                            .idMacchinaMagazzino = idMacchinaTrattamentoEsterno.IdMacchina,
                                            .TipoAttivita = TipoAttivitaProcedura.Lavorazione,
                                            .idReparto = idMacchinaTrattamentoEsterno.IdReparto,
                                            .NomeAttività = "Tratt. Termico",
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                            }
                                        })
                                        '================================================================= Versamento da Trattamento a Brunitura
                                        Dim idMagazzinoBrunitura = db.Brighetti_Magazzini.Where(Function(x) x.CodiceMagazzino = "Magazzino Brunitura").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 6,
                                            .TipoAttivita = TipoAttivitaProcedura.Versamento,
                                            .idMacchinaMagazzino = idMagazzinoBrunitura.Id,
                                            .idReparto = 0,
                                            .NomeAttività = "Versamento da Trattamento a Brunitura",
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                        }
                                                            })
                                        '================================================================= Lavorazione Brunitura
                                        Dim idMacchinaBrunitura = db.Brighetti_Macchine.Where(Function(x) x.NomeMacchina = "Macchina_Brunitura").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 7,
                                            .idMacchinaMagazzino = idMacchinaBrunitura.IdMacchina,
                                            .TipoAttivita = TipoAttivitaProcedura.Lavorazione,
                                            .idReparto = idMacchinaBrunitura.IdReparto,
                                            .NomeAttività = "Brunitura",
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                            }
                                        })
                                        '================================================================= Versamento da Brunitura a Rettifica
                                        Dim idMagazzinoRettifica = db.Brighetti_Magazzini.Where(Function(x) x.CodiceMagazzino = "Magazzino Rettifica").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 8,
                                            .TipoAttivita = TipoAttivitaProcedura.Versamento,
                                            .idMacchinaMagazzino = idMagazzinoRettifica.Id,
                                            .idReparto = 0,
                                            .NomeAttività = "Versamento da Brunitura a Rettifica",
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                        }
                                                            })
                                        '================================================================= Lavorazione Rettifica
                                        Dim idMacchinaRettifica = db.Brighetti_Macchine.Where(Function(x) x.NomeMacchina = "Macchina_Rettifica").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 9,
                                            .idMacchinaMagazzino = idMacchinaRettifica.IdMacchina,
                                            .TipoAttivita = TipoAttivitaProcedura.Lavorazione,
                                            .idReparto = idMacchinaRettifica.IdReparto,
                                            .NomeAttività = "Rettifica",
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                            }
                                        })
                                        '================================================================= Versamento da Affilatura a Finito
                                        Dim idMagazzinoFinito = db.Brighetti_Magazzini.Where(Function(x) x.CodiceMagazzino = "Magazzino Finito").FirstOrDefault
                                        db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                            .CodiceArticolo = a.CodiceArticolo,
                                            .IncrementaleProcedura = 10,
                                            .TipoAttivita = TipoAttivitaProcedura.Versamento,
                                            .idMacchinaMagazzino = idMagazzinoFinito.Id,
                                            .idReparto = 0,
                                            .NomeAttività = "Versamento da Rettifica a Finito",
                                            .UltimaModifica = New TipoUltimaModifica With {
                                                .Data = DateTime.Now,
                                                .Operatore = OpName,
                                                .OperatoreID = Opid
                                        }
                                                            })
                                        db.SaveChanges()
                                    Catch ex As Exception

                                    End Try
                            End Select
                        End If
                    End If
                    Return Json(New With {.ok = True, .message = "Articolo correttamente inserito"})
                End If
                Return Json(New With {.ok = False, .message = "Errore nella creazione articolo"})
            Catch ex As Exception
                db.Log.Add(New Log With {
                                 .Livello = TipoLogLivello.Warning,
                                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                                 .Messaggio = "Errore inserimento articolo: " & vbNewLine & ex.Message,
                                 .Dati = "",
                                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = Opid, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore nella creazione articolo"})
            End Try
            Return Json(New With {.ok = False, .message = "Errore nella creazione articolo"})
        End Function

        ' GET: Brighetti_Articolo/Edit/5
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Articolo As Brighetti_Articolo = db.Brighetti_Articoli.Find(id)
            Dim Famiglia = db.Brighetti_FamiglieProdotto.ToList
            Famiglia.Insert(0, New Brighetti_FamiglieProdotto With {.Id = 0, .NomeFamiglia = "Selezionare Voce..."})
            ViewBag.FamigliaId = New SelectList(Famiglia, "Id", "NomeFamiglia")
            If IsNothing(brighetti_Articolo) Then
                Return HttpNotFound()
            End If
            Return PartialView(brighetti_Articolo)
        End Function

        ' POST: Brighetti_Articolo/Edit/5
        'Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        'Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="Id,CodiceArticolo,DescrizioneArticolo,NoteArticolo,FamigliaId,Importanza,LottoMinimo")> ByVal brighetti_Articolo As Brighetti_Articolo) As ActionResult
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Try
                OpID = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim art = db.Brighetti_Articoli.Find(brighetti_Articolo.Id)
                If IsNothing(art) Then
                    Return Json(New With {.ok = False, .message = "Impossibile trovare l'articolo richiesto"})
                    db.SaveChanges()
                End If
                If art.CodiceArticolo <> brighetti_Articolo.CodiceArticolo Then
                    art.CodiceArticolo = brighetti_Articolo.CodiceArticolo
                    db.SaveChanges()
                End If
                If art.DescrizioneArticolo <> brighetti_Articolo.DescrizioneArticolo Then
                    art.DescrizioneArticolo = brighetti_Articolo.DescrizioneArticolo
                    db.SaveChanges()
                End If
                If art.FamigliaId <> brighetti_Articolo.FamigliaId Then
                    art.FamigliaId = brighetti_Articolo.FamigliaId
                    db.SaveChanges()
                End If
                If art.NoteArticolo <> brighetti_Articolo.NoteArticolo Then
                    art.NoteArticolo = brighetti_Articolo.NoteArticolo
                    db.SaveChanges()
                End If
                If art.Importanza <> brighetti_Articolo.Importanza Then
                    art.Importanza = brighetti_Articolo.Importanza
                    db.SaveChanges()
                End If
                If art.LottoMinimo <> brighetti_Articolo.LottoMinimo Then
                    art.LottoMinimo = brighetti_Articolo.LottoMinimo
                    db.SaveChanges()
                End If
                art.UltimaModifica = New TipoUltimaModifica With {
                    .Data = DateTime.Now,
                    .Operatore = OpName,
                    .OperatoreID = OpID
                }
                db.SaveChanges()
            Catch ex As Exception
                db.Log.Add(New Log With {
                 .Livello = TipoLogLivello.Warning,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Errore modifica articolo: " & vbNewLine & ex.Message,
                 .Dati = "",
                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore nella modifica articolo"})
            End Try
            Return Json(New With {.ok = True, .message = "Articolo correttamente aggiornato"})
        End Function

        ' GET: Brighetti_Articolo/Delete/5
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim brighetti_Articolo As Brighetti_Articolo = db.Brighetti_Articoli.Find(id)
            If IsNothing(brighetti_Articolo) Then
                Return HttpNotFound()
            End If
            Return PartialView(brighetti_Articolo)
        End Function

        ' POST: Brighetti_Articolo/Delete/5
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As Integer) As JsonResult
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Try
                OpID = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim brighetti_Articolo As Brighetti_Articolo = db.Brighetti_Articoli.Find(id)
                db.Brighetti_Articoli.Remove(brighetti_Articolo)
                db.SaveChanges()
                Return Json(New With {.ok = True, .message = "Cancellazione articolo confermata correttamente"})
            Catch ex As Exception
                db.Log.Add(New Log With {
                 .Livello = TipoLogLivello.Warning,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Errore cancellazione articolo: " & vbNewLine & ex.Message,
                 .Dati = "",
                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore nella cancellazione articolo"})
            End Try
        End Function

        Function ImbarazzanteCreazioneArticoli()
            For Each a In db.Brighetti_Articoli.ToList
                Try
                    Dim OpID = ""
                    Dim OpName = "Sistema"
                    If db.Brighetti_Procedure.Where(Function(x) x.CodiceArticolo = a.CodiceArticolo).Count = 0 Then
                        Select Case True
                            Case a.CodiceArticolo.Contains("")
                                Try
                                    Dim idMacchinaTornitura = db.Brighetti_Macchine.Where(Function(x) x.NomeMacchina = "Macchina_Tornitura").FirstOrDefault
                                    Dim idMagazzinoTrattamento = db.Brighetti_Magazzini.Where(Function(x) x.CodiceMagazzino = "Magazzino Tratt. Termico").FirstOrDefault
                                    '================================================================= Lavorazione Tornitura
                                    db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                        .CodiceArticolo = a.CodiceArticolo,
                                        .IncrementaleProcedura = 1,
                                        .TipoAttivita = TipoAttivitaProcedura.Lavorazione,
                                        .idMacchinaMagazzino = idMacchinaTornitura.IdMacchina,
                                        .idReparto = idMacchinaTornitura.IdReparto,
                                        .NomeAttività = "Tornitura",
                                        .UltimaModifica = New TipoUltimaModifica With {
                                            .Data = DateTime.Now,
                                            .Operatore = OpName,
                                            .OperatoreID = OpID
                                    }
                                                        })
                                    '================================================================= Versamento da Tornitura a C/O
                                    db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                        .CodiceArticolo = a.CodiceArticolo,
                                        .IncrementaleProcedura = 2,
                                        .TipoAttivita = TipoAttivitaProcedura.Versamento,
                                        .idMacchinaMagazzino = idMagazzinoTrattamento.Id,
                                        .idReparto = 0,
                                        .NomeAttività = "Versamento da Tornitura a C/O",
                                        .UltimaModifica = New TipoUltimaModifica With {
                                            .Data = DateTime.Now,
                                            .Operatore = OpName,
                                            .OperatoreID = OpID
                                    }
                                                        })
                                    '================================================================= Lavorazione C/O
                                    Dim idMacchinaTrattamentoEsterno = db.Brighetti_Macchine.Where(Function(x) x.NomeMacchina = "Macchina_Trattamento").FirstOrDefault
                                    db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                        .CodiceArticolo = a.CodiceArticolo,
                                        .IncrementaleProcedura = 3,
                                        .idMacchinaMagazzino = idMacchinaTrattamentoEsterno.IdMacchina,
                                        .TipoAttivita = TipoAttivitaProcedura.Lavorazione,
                                        .idReparto = idMacchinaTrattamentoEsterno.IdReparto,
                                        .NomeAttività = "Tratt. Termico",
                                        .UltimaModifica = New TipoUltimaModifica With {
                                            .Data = DateTime.Now,
                                            .Operatore = OpName,
                                            .OperatoreID = OpID
                                        }
                                    })
                                    '================================================================= Versamento da C/O a Rettifica
                                    Dim idMagazzinoRettifica = db.Brighetti_Magazzini.Where(Function(x) x.CodiceMagazzino = "Magazzino Rettifica").FirstOrDefault
                                    db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                        .CodiceArticolo = a.CodiceArticolo,
                                        .IncrementaleProcedura = 4,
                                        .TipoAttivita = TipoAttivitaProcedura.Versamento,
                                        .idMacchinaMagazzino = idMagazzinoRettifica.Id,
                                        .idReparto = 0,
                                        .NomeAttività = "Versamento da C/O a Rettifica",
                                        .UltimaModifica = New TipoUltimaModifica With {
                                            .Data = DateTime.Now,
                                            .Operatore = OpName,
                                            .OperatoreID = OpID
                                        }
                                    })
                                    '================================================================= Lavorazione Rettifica
                                    Dim idMacchinaRettifica = db.Brighetti_Macchine.Where(Function(x) x.NomeMacchina = "Macchina_Rettifica").FirstOrDefault
                                    db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                        .CodiceArticolo = a.CodiceArticolo,
                                        .IncrementaleProcedura = 5,
                                        .TipoAttivita = TipoAttivitaProcedura.Lavorazione,
                                        .idMacchinaMagazzino = idMacchinaRettifica.IdMacchina,
                                        .idReparto = idMacchinaRettifica.IdReparto,
                                        .NomeAttività = "Rettifica",
                                        .UltimaModifica = New TipoUltimaModifica With {
                                            .Data = DateTime.Now,
                                            .Operatore = OpName,
                                            .OperatoreID = OpID
                                    }
                                                        })
                                    '================================================================= Versamento da Rettifica a Affilatura
                                    Dim idMagazzinoAffilatura = db.Brighetti_Magazzini.Where(Function(x) x.CodiceMagazzino = "Magazzino Affilatura").FirstOrDefault
                                    db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                        .CodiceArticolo = a.CodiceArticolo,
                                        .IncrementaleProcedura = 6,
                                        .TipoAttivita = TipoAttivitaProcedura.Versamento,
                                        .idMacchinaMagazzino = idMagazzinoAffilatura.Id,
                                        .idReparto = 0,
                                        .NomeAttività = "Versamento Affilatura",
                                        .UltimaModifica = New TipoUltimaModifica With {
                                            .Data = DateTime.Now,
                                            .Operatore = OpName,
                                            .OperatoreID = OpID
                                    }
                                                        })
                                    '================================================================= Lavorazione Affilatura
                                    Dim idMacchinaAffilatura = db.Brighetti_Macchine.Where(Function(x) x.NomeMacchina = "Macchina_Affilatura").FirstOrDefault
                                    db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                        .CodiceArticolo = a.CodiceArticolo,
                                        .IncrementaleProcedura = 7,
                                        .TipoAttivita = TipoAttivitaProcedura.Lavorazione,
                                        .idMacchinaMagazzino = idMacchinaAffilatura.IdMacchina,
                                        .idReparto = idMacchinaAffilatura.IdReparto,
                                        .NomeAttività = "Affilatura",
                                        .UltimaModifica = New TipoUltimaModifica With {
                                            .Data = DateTime.Now,
                                            .Operatore = OpName,
                                            .OperatoreID = OpID
                                    }
                                                        })
                                    '================================================================= Versamento da Rettifica a Affilatura
                                    Dim idMagazzinoFiniti = db.Brighetti_Magazzini.Where(Function(x) x.CodiceMagazzino = "Magazzino Finito").FirstOrDefault
                                    db.Brighetti_Procedure.Add(New Brighetti_Procedura With {
                                        .CodiceArticolo = a.CodiceArticolo,
                                        .IncrementaleProcedura = 8,
                                        .TipoAttivita = TipoAttivitaProcedura.Versamento,
                                        .idMacchinaMagazzino = idMagazzinoFiniti.Id,
                                        .idReparto = 0,
                                        .NomeAttività = "Versamento Finito",
                                        .UltimaModifica = New TipoUltimaModifica With {
                                            .Data = DateTime.Now,
                                            .Operatore = OpName,
                                            .OperatoreID = OpID
                                    }
                                                        })
                                    db.SaveChanges()
                                Catch ex As Exception

                                End Try
                            Case a.CodiceArticolo.Contains("")
                                'Crea procedura brocce std e speciale
                                'Crea procedure brocce std e speciale con refrigereazione

                        End Select
                    End If
                Catch ex As Exception

                End Try
            Next
        End Function
        Private Function MakeWhereExpression(Search As String) As Expressions.Expression(Of Func(Of Brighetti_Articolo, Boolean))
            Return Function(x) x.CodiceArticolo.Contains(Search) Or
                               x.DescrizioneArticolo.Contains(Search)
        End Function
        Private Function MakeOrderExpression(Column As Integer) As Expressions.Expression(Of Func(Of Brighetti_Articolo, String))
            Select Case Column
                Case Nothing : Return Function(x) x.CodiceArticolo
                Case 1 : Return Function(x) x.CodiceArticolo
                Case 2 : Return Function(x) x.DescrizioneArticolo
                Case 4 : Return Function(x) x.Importanza
                Case 5 : Return Function(x) x.FamigliaId
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
