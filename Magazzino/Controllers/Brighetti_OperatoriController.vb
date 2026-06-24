Imports System.Reflection
Imports System.Web.Mvc
Imports Microsoft.AspNet.Identity
Imports Newtonsoft.Json

Namespace Controllers
    Public Class Brighetti_OperatoriController
        Inherits Controller

        Private db As New BrighettiModels
        Private appctx As New ApplicationDbContext

        ' GET: Brighetti_Operatori
        Function AvvioAttivita() As ActionResult
            Dim Opid = vbNullString
            Dim OpName = vbNullString
            Try
                Opid = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim userAsp = appctx.MacchineUsers.Where(Function(X) X.IdUtente = Opid).FirstOrDefault
                Dim attivitàinCorso = db.Brighetti_Attività.Where(Function(x) x.idMacchinaMagazzino = userAsp.IdMacchina And x.StatoAttività = TipoStatoAttività.In_Lavorazione).FirstOrDefault
                If Not IsNothing(attivitàinCorso) Then
                    Return RedirectToAction("VisualizzaAttività", New With {.id = attivitàinCorso.IdAttività})
                End If
                If Not IsNothing(userAsp) Then
                    If userAsp.IdReparto <> 0 Then
                        Return RedirectToAction("AvvioReparto")
                    ElseIf userAsp.IdMacchina <> 0 Then
                        Return RedirectToAction("AvvioMacchina")
                    End If
                End If
            Catch ex As Exception

            End Try
            Return View()
        End Function
        <Authorize>
        Function AvvioMacchina(Optional idmacchina As Integer = 0) As ActionResult
            Dim Opid = vbNullString
            Dim OpName = vbNullString
            Try
                Opid = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim userAsp = appctx.MacchineUsers.Where(Function(X) X.IdUtente = Opid).FirstOrDefault
                Dim attivitàInCorso
                If idmacchina = 0 Then
                    attivitàInCorso = db.Brighetti_Attività.Where(Function(x) x.idMacchinaMagazzino = userAsp.IdMacchina And (x.StatoAttività = TipoStatoAttività.In_Lavorazione Or x.StatoAttività = TipoStatoAttività.In_Attrezzaggio)).FirstOrDefault
                Else
                    attivitàInCorso = db.Brighetti_Attività.Where(Function(x) x.idMacchinaMagazzino = idmacchina And (x.StatoAttività = TipoStatoAttività.In_Lavorazione Or x.StatoAttività = TipoStatoAttività.In_Attrezzaggio)).FirstOrDefault
                End If
                If Not IsNothing(attivitàInCorso) And idmacchina = 0 Then
                    Return RedirectToAction("VisualizzaAttività", New With {.id = attivitàInCorso.IdAttività})
                End If
                Dim userAspOfficial = appctx.Users.Where(Function(X) X.Id = Opid).FirstOrDefault
                Dim macchina As New Brighetti_Macchina
                Dim listOfActivities
                If idmacchina = 0 Then
                    macchina = db.Brighetti_Macchine.Find(userAsp.IdMacchina)
                    listOfActivities = db.Brighetti_Attività.Where(Function(x) x.idMacchinaMagazzino = macchina.IdMacchina And (x.StatoAttività = TipoStatoAttività.In_attesa Or x.StatoAttività = TipoStatoAttività.StandBy)).ToList
                    ViewBag.listaNote = db.Brighetti_Note.Where(Function(x) x.IdEsterno = idmacchina And x.TypeElem = TipoElemento.Lavorazione).Count
                    ViewBag.listaDocumenti = db.Brighetti_Documenti.Where(Function(x) x.IdEsterno = idmacchina And x.TypeElem = TipoElemento.Lavorazione).Count
                Else
                    macchina = db.Brighetti_Macchine.Find(idmacchina)
                    listOfActivities = db.Brighetti_Attività.Where(Function(x) x.idMacchinaMagazzino = idmacchina And (x.StatoAttività = TipoStatoAttività.In_attesa Or x.StatoAttività = TipoStatoAttività.StandBy)).ToList
                    ViewBag.listaNote = db.Brighetti_Note.Where(Function(x) x.IdEsterno = idmacchina And x.TypeElem = TipoElemento.Lavorazione).Count
                    ViewBag.listaDocumenti = db.Brighetti_Documenti.Where(Function(x) x.IdEsterno = idmacchina And x.TypeElem = TipoElemento.Lavorazione).Count
                End If
                If idmacchina <> 0 Then
                    Return PartialView(New AttivitaMacchineViewModel With {
                .ListaAttivita = listOfActivities,
                .Macchina = macchina,
                .Utente = userAspOfficial
            })
                Else
                    Return View(New AttivitaMacchineViewModel With {
                    .ListaAttivita = listOfActivities,
                    .Macchina = macchina,
                    .Utente = userAspOfficial
                })
                End If
            Catch ex As Exception
                db.Log.Add(New Log With {
                               .Livello = TipoLogLivello.Warning,
                               .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                               .Messaggio = "Errore ricerca attività macchina: " & vbNewLine & ex.Message,
                               .Dati = "",
                               .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = Opid, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
            End Try
        End Function
        <Authorize>
        Function AvvioReparto() As ActionResult

        End Function

        <Authorize>
        Function VisualizzaAttività(ByVal id As Integer) As ActionResult
            Dim Opid = vbNullString
            Dim OpName = vbNullString
            Try
                Opid = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim attivita = db.Brighetti_Attività.Find(id)
                Dim userAsp = appctx.MacchineUsers.Where(Function(X) X.IdUtente = Opid).FirstOrDefault
                Dim attivitàinCorso = db.Brighetti_Attività.Where(Function(x) x.idMacchinaMagazzino = userAsp.IdMacchina And (x.StatoAttività = TipoStatoAttività.In_Lavorazione Or x.StatoAttività = TipoStatoAttività.In_Attrezzaggio)).FirstOrDefault
                If IsNothing(attivitàinCorso) Then
                    Return RedirectToAction("AvvioMacchina")
                End If
                Return View(attivita)
            Catch ex As Exception
                db.Log.Add(New Log With {
                               .Livello = TipoLogLivello.Warning,
                               .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                               .Messaggio = "Errore apertura attività macchina: " & vbNewLine & ex.Message,
                               .Dati = "",
                               .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = Opid, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()

            End Try
        End Function
        ' POST: Brighetti_Articolo/Edit/5
        'Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        'Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function ApriAttività(<Bind(Include:="IdAttività,StatoAttivita")> ByVal brighetti_Attività As Brighetti_Attività, ByVal StatoAttivita As Integer) As ActionResult
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Try
                OpID = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim attività = db.Brighetti_Attività.Find(brighetti_Attività.IdAttività)
                attività.UltimaModifica = New TipoUltimaModifica With {
                    .Data = DateTime.Now,
                    .Operatore = OpName,
                    .OperatoreID = OpID
                }
                db.SaveChanges()
                Select Case StatoAttivita
                    Case 0
                        attività.StatoAttività = TipoStatoAttività.In_Attrezzaggio
                        db.SaveChanges()
                    Case 1
                        attività.StatoAttività = TipoStatoAttività.In_Lavorazione
                        db.SaveChanges()
                End Select
                db.SaveChanges()
                db.Audit.Add(New Audit With {
                             .Livello = TipoLogLivello.Warning,
                             .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                             .Messaggio = "Attività aperta correttamente: " & brighetti_Attività.IdAttività.ToString,
                             .Dati = JsonConvert.SerializeObject(brighetti_Attività),
                             .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
            Catch ex As Exception
                db.Log.Add(New Log With {
                 .Livello = TipoLogLivello.Warning,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Errore apertura attività: " & vbNewLine & ex.Message,
                 .Dati = "",
                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore nella apertura attività"})
            End Try
            Return Json(New With {.ok = True, .message = "Attività correttamente aperta", .pathRedirect = "/Brighetti_Operatori/AvvioMacchina"})
        End Function
        '<HttpPost()>
        '<ValidateAntiForgeryToken()>
        Function ApriAttrezzaggio(<Bind(Include:="IdAttività")> ByVal brighetti_Attività As Brighetti_Attività) As ActionResult
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Try
                OpID = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim attività = db.Brighetti_Attività.Find(brighetti_Attività.IdAttività)
                attività.UltimaModifica = New TipoUltimaModifica With {
                    .Data = DateTime.Now,
                    .Operatore = OpName,
                    .OperatoreID = OpID
                }
                db.SaveChanges()
                attività.StatoAttività = TipoStatoAttività.In_Attrezzaggio
                db.SaveChanges()
                db.Audit.Add(New Audit With {
                             .Livello = TipoLogLivello.Warning,
                             .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                             .Messaggio = "Attività di attrezzaggio aperta correttamente: " & brighetti_Attività.IdAttività.ToString,
                             .Dati = JsonConvert.SerializeObject(brighetti_Attività),
                             .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
            Catch ex As Exception
                db.Log.Add(New Log With {
                 .Livello = TipoLogLivello.Warning,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Errore apertura attrezzaggio attività: " & vbNewLine & ex.Message,
                 .Dati = "",
                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore nell'attrezzaggio attività"})
            End Try
            Return Json(New With {.ok = True, .message = "Attività attrezzaggio correttamente aperta", .pathRedirect = "/Brighetti_Operatori/AvvioMacchina"})
        End Function
        <Authorize>
        Function ApriAttività(ByVal id As Integer) As ActionResult
            Dim Opid = vbNullString
            Dim OpName = vbNullString
            Try
                Opid = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim userAsp = appctx.MacchineUsers.Where(Function(X) X.IdUtente = Opid).FirstOrDefault
                Dim attivitàinCorso = db.Brighetti_Attività.Where(Function(x) x.idMacchinaMagazzino = userAsp.IdMacchina And x.StatoAttività = TipoStatoAttività.In_Lavorazione).FirstOrDefault
                ViewBag.listaNote = db.Brighetti_Note.Where(Function(x) x.IdEsterno = userAsp.IdMacchina And x.TypeElem = TipoElemento.Macchina).ToList
                ViewBag.listaDocumenti = db.Brighetti_Documenti.Where(Function(x) x.IdEsterno = userAsp.IdMacchina And x.TypeElem = TipoElemento.Macchina).ToList
                If Not IsNothing(attivitàinCorso) Then
                    Return RedirectToAction("VisualizzaAttività", New With {.id = attivitàinCorso.IdAttività})
                End If
                Dim listItem As New List(Of SelectListItem)
                listItem.Add(New SelectListItem With {.Value = 0, .Text = "Attrezzaggio"})
                listItem.Add(New SelectListItem With {.Value = 1, .Text = "Lavorazione"})
                ViewBag.StatoAttivita = New SelectList(listItem, "Value", "Text")
                Dim attivita = db.Brighetti_Attività.Find(id)
                Return PartialView(attivita)
            Catch ex As Exception
                db.Log.Add(New Log With {
                               .Livello = TipoLogLivello.Warning,
                               .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                               .Messaggio = "Errore apertura attività macchina: " & vbNewLine & ex.Message,
                               .Dati = "",
                               .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = Opid, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()

            End Try
        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function SospendiAttività(<Bind(Include:="IdAttività,QuantitàProdotta,QuantitàScartata")> ByVal brighetti_Attività As Brighetti_Attività) As ActionResult
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Dim NowDate = DateTime.Now
            Try
                OpID = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim attività = db.Brighetti_Attività.Find(brighetti_Attività.IdAttività)
                If attività.QuantitàProdotta <> brighetti_Attività.QuantitàProdotta Then
                    attività.QuantitàProdotta = brighetti_Attività.QuantitàProdotta
                    db.SaveChanges()
                End If
                If attività.QuantitàScartata <> brighetti_Attività.QuantitàScartata Then
                    attività.QuantitàScartata = brighetti_Attività.QuantitàScartata
                    db.SaveChanges()
                End If
                Select Case attività.StatoAttività
                    Case TipoStatoAttività.In_Attrezzaggio
                        db.Brighetti_Tempi.Add(New Brighetti_Tempi With {
                    .DataInizio = attività.UltimaModifica.Data,
                    .DataFine = NowDate,
                    .IdAttività = attività.IdAttività,
                    .Note = "",
                    .TipoAttività = TipoTempoAttività.Attrezzaggio
                })
                        db.SaveChanges()
                    Case TipoStatoAttività.In_Lavorazione
                        db.Brighetti_Tempi.Add(New Brighetti_Tempi With {
                        .DataInizio = attività.UltimaModifica.Data,
                        .DataFine = NowDate,
                        .IdAttività = attività.IdAttività,
                        .Note = "",
                        .TipoAttività = TipoTempoAttività.Lavorazione
                })
                        db.SaveChanges()
                End Select
                attività.UltimaModifica = New TipoUltimaModifica With {
                    .Data = NowDate,
                    .Operatore = OpName,
                    .OperatoreID = OpID
                }
                db.SaveChanges()
                attività.StatoAttività = TipoStatoAttività.StandBy
                db.SaveChanges()
                db.Audit.Add(New Audit With {
                              .Livello = TipoLogLivello.Warning,
                              .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                              .Messaggio = "Attività sospesa correttamente: " & brighetti_Attività.IdAttività.ToString,
                              .Dati = JsonConvert.SerializeObject(brighetti_Attività),
                              .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
            Catch ex As Exception
                db.Log.Add(New Log With {
                 .Livello = TipoLogLivello.Warning,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Errore apertura attività: " & vbNewLine & ex.Message,
                 .Dati = "",
                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore nella apertura attività"})
            End Try
            Return Json(New With {.ok = True, .message = "Attività correttamente aperta", .pathRedirect = "/Brighetti_Operatori/AvvioMacchina"})
        End Function
        <Authorize>
        Function SospendiAttività(ByVal id As Integer) As ActionResult
            Dim Opid = vbNullString
            Dim OpName = vbNullString
            Try
                Opid = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim userAsp = appctx.MacchineUsers.Where(Function(X) X.IdUtente = Opid).FirstOrDefault
                Dim attivita = db.Brighetti_Attività.Find(id)
                Return PartialView(attivita)
            Catch ex As Exception
                db.Log.Add(New Log With {
                               .Livello = TipoLogLivello.Warning,
                               .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                               .Messaggio = "Errore sospensione attività macchina: " & vbNewLine & ex.Message,
                               .Dati = "",
                               .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = Opid, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()

            End Try
        End Function
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function ConcludiAttività(<Bind(Include:="IdAttività,QuantitàProdotta,QuantitàScartata")> ByVal brighetti_Attività As Brighetti_Attività) As ActionResult
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Dim NowDate = DateTime.Now
            Try
                OpID = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim attività = db.Brighetti_Attività.Find(brighetti_Attività.IdAttività)
                If attività.QuantitàProdotta <> brighetti_Attività.QuantitàProdotta Then
                    attività.QuantitàProdotta = brighetti_Attività.QuantitàProdotta
                    db.SaveChanges()
                End If
                If attività.QuantitàScartata <> brighetti_Attività.QuantitàScartata Then
                    attività.QuantitàScartata = brighetti_Attività.QuantitàScartata
                    db.SaveChanges()
                End If
                '======================================= CASE STATO IN ATTREZZAGGIO
                Select Case attività.StatoAttività
                    Case TipoStatoAttività.In_Attrezzaggio
                        db.Brighetti_Tempi.Add(New Brighetti_Tempi With {
                    .DataInizio = attività.UltimaModifica.Data,
                    .DataFine = NowDate,
                    .IdAttività = attività.IdAttività,
                    .Note = "",
                    .TipoAttività = TipoTempoAttività.Attrezzaggio
                })
                        db.SaveChanges()
                        attività.StatoAttività = TipoStatoAttività.In_Lavorazione
                        db.SaveChanges()
                        '======================================= CASE STATO IN LAVORAZIONE
                    Case TipoStatoAttività.In_Lavorazione
                        db.Brighetti_Tempi.Add(New Brighetti_Tempi With {
                        .DataInizio = attività.UltimaModifica.Data,
                        .DataFine = NowDate,
                        .IdAttività = attività.IdAttività,
                        .Note = "",
                        .TipoAttività = TipoTempoAttività.Lavorazione
                })
                        db.SaveChanges()
                        'Cancellare giacenza del magazzino precedente
                        Dim attivitaprecedente = db.Brighetti_Procedure.Where(Function(x) x.CodiceArticolo = attività.CodiceArticolo And x.IncrementaleProcedura = attività.IncrementaleProcedura - 1).FirstOrDefault
                        If Not IsNothing(attivitaprecedente) Then
                            Dim giacenza = db.Brighetti_Giacenze.Where(Function(x) x.CodiceArticolo = attività.CodiceArticolo And x.CodiceMagazzino = attivitaprecedente.idMacchinaMagazzino).FirstOrDefault
                            If Not IsNothing(giacenza) Then
                                Dim elemGiacenza = giacenza.ListaQuantità.Where(Function(X) X.idProcedura = attivitaprecedente.IdProcedura).FirstOrDefault
                                If Not IsNothing(elemGiacenza) Then
                                    db.Brighetti_DettagliGiacenza.Remove(elemGiacenza)
                                    db.SaveChanges()
                                End If
                                giacenza.QuantitàGiacenza = giacenza.QuantitàGiacenza - attività.QuantitàProdotta
                                If giacenza.QuantitàGiacenza < 0 Then
                                    giacenza.QuantitàGiacenza = 0
                                End If
                                db.SaveChanges()
                            End If
                        End If
                        'Definire chiusa la fase
                        attività.StatoAttività = TipoStatoAttività.Completato
                        db.SaveChanges()
                        'Se la giacenza non esiste, crearla
                        Dim idArticolo = db.Brighetti_Articoli.Where(Function(X) X.CodiceArticolo = attività.CodiceArticolo).First.Id
                        Dim attivitaSeguente = db.Brighetti_Procedure.Where(Function(x) x.CodiceArticolo = attività.CodiceArticolo And x.IncrementaleProcedura = attività.IncrementaleProcedura + 1).FirstOrDefault
                        If Not IsNothing(attivitaSeguente) Then
                            Dim giacenza = db.Brighetti_Giacenze.Where(Function(x) x.CodiceArticolo = idArticolo.ToString And x.CodiceMagazzino = attivitaSeguente.idMacchinaMagazzino.ToString).FirstOrDefault
                            If IsNothing(giacenza) Then
                                'Creare giacenza nel magazzino nuovo
                                giacenza = db.Brighetti_Giacenze.Add(New Brighetti_Giacenza With {
                                    .CodiceArticolo = idArticolo,
                                    .CodiceMagazzino = attivitaSeguente.idMacchinaMagazzino,
                                    .InPrevisioneEntrata = 0,
                                    .QuantitàGiacenza = attività.QuantitàProdotta,
                                    .QuantitàSottoscorta = attività.QuantitàProdotta,
                                    .UltimaModifica = New TipoUltimaModifica With {
                                        .OperatoreID = OpID,
                                        .Operatore = OpName,
                                        .Data = DateTime.Now
                                    }
                                })
                                db.SaveChanges()
                            End If
                            'Aggiungere poi il dettaglio giacenza, necessario per i lotti
                            Dim dettGiac = db.Brighetti_DettagliGiacenza.Add(New Brighetti_DettagliGiacenza With {
                            .idGiacenza = giacenza.Id,
                            .idProcedura = attivitaSeguente.IdProcedura,
                            .QuantitàProdotta = attività.QuantitàProdotta
                                })
                            db.SaveChanges()
                            If Not IsNothing(giacenza.ListaQuantità) Then
                                giacenza.ListaQuantità.Add(dettGiac)
                                db.SaveChanges()
                            Else
                                giacenza.ListaQuantità = New List(Of Brighetti_DettagliGiacenza)
                                giacenza.ListaQuantità.Add(dettGiac)
                                db.SaveChanges()
                            End If
                        End If
                        'Chiudere quindi la fase di versamento
                        Dim faseVersamento = db.Brighetti_Attività.Where(Function(X) X.IdAttività = attività.IdAttività + 1).FirstOrDefault
                        If Not IsNothing(faseVersamento) Then
                            faseVersamento.StatoAttività = TipoStatoAttività.Completato
                            faseVersamento.QuantitàProdotta = attività.QuantitàProdotta
                            faseVersamento.QuantitàScartata = attività.QuantitàScartata
                            db.SaveChanges()
                        End If
                        'Aprire la fase nuova (solo se esiste una fase successiva: evita
                        'NullReference sull'ultima fase, p.es. nei cicli speciali "S")
                        If Not IsNothing(attivitaSeguente) Then
                            Dim NextAct = db.Brighetti_Attività.Where(Function(x) x.CodiceArticolo = attività.CodiceArticolo And x.IncrementaleProcedura = attivitaSeguente.IncrementaleProcedura + 1).FirstOrDefault
                            If Not IsNothing(NextAct) Then
                                NextAct.StatoAttività = TipoStatoAttività.In_attesa
                                db.SaveChanges()
                            End If
                        End If
                        'Creazione automatica del lotto alla chiusura della fase (se l'automatismo è attivo)
                        Try
                            LottiAutomaticiService.CreaLottoDaVersamento(attività.CodiceArticolo, attività.QuantitàProdotta, OpID, OpName)
                        Catch exLotto As Exception
                            'Un errore nella creazione del lotto non deve bloccare la chiusura dell'attività.
                        End Try
                End Select

            Catch ex As Exception
                db.Log.Add(New Log With {
                 .Livello = TipoLogLivello.Warning,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Errore apertura attività: " & vbNewLine & ex.Message,
                 .Dati = "",
                 .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = NowDate}})
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore nella conclusione attività"})
            End Try
            Return Json(New With {.ok = True, .message = "Attività conclusa correttamente", .pathRedirect = "/Brighetti_Operatori/AvvioMacchina"})
        End Function
        <Authorize>
        Function ConcludiAttività(ByVal id As Integer) As ActionResult
            Dim Opid = vbNullString
            Dim OpName = vbNullString
            Try
                Opid = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim userAsp = appctx.MacchineUsers.Where(Function(X) X.IdUtente = Opid).FirstOrDefault
                Dim attivita = db.Brighetti_Attività.Find(id)
                Return PartialView(attivita)
            Catch ex As Exception
                db.Log.Add(New Log With {
                               .Livello = TipoLogLivello.Warning,
                               .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                               .Messaggio = "Errore conclusione attività macchina: " & vbNewLine & ex.Message,
                               .Dati = "",
                               .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = Opid, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()

            End Try
        End Function

        ' GET: Brighetti_Operatori/Details/5
        Function Details(ByVal id As Integer) As ActionResult
            Return View()
        End Function

        ' GET: Brighetti_Operatori/Create
        Function Create() As ActionResult
            Return View()
        End Function

        ' POST: Brighetti_Operatori/Create
        <HttpPost()>
        Function Create(ByVal collection As FormCollection) As ActionResult
            Try
                ' TODO: Add insert logic here

                Return RedirectToAction("Index")
            Catch
                Return View()
            End Try
        End Function

        ' GET: Brighetti_Operatori/Edit/5
        Function Edit(ByVal id As Integer) As ActionResult
            Return View()
        End Function

        ' POST: Brighetti_Operatori/Edit/5
        <HttpPost()>
        Function Edit(ByVal id As Integer, ByVal collection As FormCollection) As ActionResult
            Try
                ' TODO: Add update logic here

                Return RedirectToAction("Index")
            Catch
                Return View()
            End Try
        End Function

        ' GET: Brighetti_Operatori/Delete/5
        Function Delete(ByVal id As Integer) As ActionResult
            Return View()
        End Function

        ' POST: Brighetti_Operatori/Delete/5
        <HttpPost()>
        Function Delete(ByVal id As Integer, ByVal collection As FormCollection) As ActionResult
            Try
                ' TODO: Add delete logic here

                Return RedirectToAction("Index")
            Catch
                Return View()
            End Try
        End Function
    End Class
End Namespace