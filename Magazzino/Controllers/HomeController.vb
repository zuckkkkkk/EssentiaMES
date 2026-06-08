Imports System.IO
Imports System.Net.Mail
Imports Microsoft.AspNet.Identity
Imports NPOI.SS.Formula.Functions

Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Private db As New BrighettiModels
    Private appctx As New ApplicationDbContext

    <Authorize>
    Function Index(ByVal q As String) As ActionResult
        If User.IsInRole("Operatore") Then
            Return RedirectToAction("AvvioAttivita", "Brighetti_Operatori")
        Else
            Return RedirectToAction("AdminDashboard", "Macchine")
        End If
        Dim qRes = IIf(Not IsNothing(q), db.Articoli.Where(Function(x) x.Nome_Articolo.Contains(q) And x.ArticoloCancellato = 0).ToList, db.Articoli.Where(Function(x) x.ArticoloCancellato = 0).ToList)
        ViewBag.query = q
        Return View(qRes)
    End Function
    <Authorize(Roles:="Admin")>
    Function CancellaArticolo(ByVal id As Integer) As JsonResult
        Dim art = db.Articoli.Find(id)
        art.ArticoloCancellato = 1
        db.SaveChanges()
        Return Json(New With {.ok = True})
    End Function
    <Authorize(Roles:="Admin")>
    Function CreaArticolo(ByVal CodiceArticolo As String, ByVal DescrizioneArticolo As String) As JsonResult
        Dim OpID = vbNullString
        Dim OpName = vbNullString
        Try
            OpID = User.Identity.GetUserId()
            OpName = User.Identity.GetUserName()
            db.Articoli.Add(New Articoli With {
                .Descrizione_Articolo = DescrizioneArticolo,
                .Nome_Articolo = CodiceArticolo,
                .MagazzinoId = 1
            })
            db.SaveChanges()
            Return Json(New With {.ok = True})
        Catch ex As Exception

        End Try
        Return Json(New With {.ok = True})
    End Function
    <Authorize(Roles:="Admin")>
    Function MostraOrdine(ByVal id As String) As ActionResult
        Dim listAct = db.Brighetti_Attività.Where(Function(x) x.OrdineDiProduzione = id And x.TipoAttivitaProcedura = TipoAttivitaProcedura.Lavorazione).ToList
        Dim countTotact = db.Brighetti_Attività.Where(Function(x) x.OrdineDiProduzione = id).Count
        Dim countCompletateact = db.Brighetti_Attività.Where(Function(x) x.OrdineDiProduzione = id And x.StatoAttività = TipoStatoAttività.Completato).Count
        Dim percentageact = 0
        If countCompletateact <> 0 Then
            percentageact = (countCompletateact * 100) / countTotact
        End If

        Dim parlist As New List(Of AttivitàviewModel)

        For Each l In listAct
            Dim listaNote = db.Brighetti_Note.Where(Function(x) x.IdEsterno = l.IdAttività And x.TypeElem = TipoElemento.Lavorazione).Count
            Dim listaDocumenti = db.Brighetti_Documenti.Where(Function(x) x.IdEsterno = l.IdAttività And x.TypeElem = TipoElemento.Lavorazione).Count

            Dim tempolavorazione = 0
            Dim tempoattrezzaggio = 0
            For Each tempo In db.Brighetti_Tempi.Where(Function(x) x.IdAttività = l.IdAttività).ToList
                If tempo.TipoAttività = 1 Then
                    tempolavorazione = DateDiff(DateInterval.Minute, tempo.DataInizio, tempo.DataFine)
                Else
                    tempoattrezzaggio = DateDiff(DateInterval.Minute, tempo.DataInizio, tempo.DataFine)
                End If
            Next
            parlist.Add(New AttivitàviewModel With {
                .IncrementaleAttività = l.IncrementaleProcedura,
                .NomeAttività = l.NomeAttività,
                .Quantitàprodotta = l.QuantitàProdotta,
                .QuantitàScartata = l.QuantitàScartata,
                .TempoAttrezzaggio = tempoattrezzaggio,
                .TempoLavorazione = tempolavorazione,
                .PercCompletamento = percentageact,
                .CountAllegati = listaDocumenti,
                .CountNote = listaNote
            })
        Next
        Return PartialView(parlist)
    End Function
    <Authorize(Roles:="Admin")>
    Function Storico() As ActionResult
        Dim ret As New List(Of StoricoViewModel)
        Dim dataora = DateTime.Now
        Dim dataweek = DateTime.Now.Date.AddDays(-7)
        For Each a In db.Brighetti_Attività.Where(Function(x) x.UltimaModifica.Data >= dataweek And x.UltimaModifica.Data <= dataora).Select(Function(x) x.OrdineDiProduzione).Distinct.ToList
            Dim o = db.Brighetti_Attività.Where(Function(x) x.OrdineDiProduzione = a).FirstOrDefault
            ret.Add(New StoricoViewModel With {
                    .Id = o.OrdineDiProduzione,
                    .CodiceArticolo = o.CodiceArticolo,
                    .DataOrdine = o.UltimaModifica.Data,
                    .OpName = If(o.UltimaModifica.Operatore.Length > 20, o.UltimaModifica.Operatore.Substring(0, 15) + "...", o.UltimaModifica.Operatore)
                })
        Next
        Return View(ret)
    End Function
    <Authorize>
    Function Carrello() As ActionResult
        Dim OpID = User.Identity.GetUserId()
        Dim listaOrdiniInCorso = db.OrdiniInCorso.Where(Function(x) x.UltimaModifica.OperatoreID = OpID).ToList
        Dim listaOrdiniViewModel As New List(Of OrdiniInCorsoViewModel)
        For Each l In listaOrdiniInCorso
            Dim Articolo = db.Articoli.Find(l.IdArticolo)
            listaOrdiniViewModel.Add(New OrdiniInCorsoViewModel With {
                .IdArticolo = l.IdArticolo,
                .NomeArticolo = Articolo.Nome_Articolo,
                .DescrizioneArticolo = Articolo.Descrizione_Articolo,
                .QtaArticolo = l.QtaArticolo,
                .UltimaModifica = l.UltimaModifica,
                .Id = l.Id
            })
        Next
        Return View(listaOrdiniViewModel)
    End Function
    <Authorize>
    Function About() As ActionResult
        ViewData("Message") = "Your application description page."
        Dim macchine = db.Macchine.ToList
        Return View()
    End Function
    <Authorize>
    Function Contact() As ActionResult
        ViewData("Message") = "Your contact page."
        Return View()
    End Function

    '======================================================================= AJAX
    <Authorize>
    Function InserisciArticoloDaOrdinare(ByVal id As Integer, ByVal qta As Integer) As JsonResult
        Dim OpID = vbNullString
        Dim OpName = vbNullString
        Try
            OpID = User.Identity.GetUserId()
            OpName = User.Identity.GetUserName()
            Dim listaArticoliOperatore = db.OrdiniInCorso.Where(Function(x) x.UltimaModifica.OperatoreID = OpID).ToList

            If Not IsNothing(listaArticoliOperatore) Then
                Dim Articolo = db.Articoli.Find(id)
                If listaArticoliOperatore.Where(Function(x) x.IdArticolo = id).Count = 1 Then
                    listaArticoliOperatore.Where(Function(x) x.IdArticolo = id).First.QtaArticolo = listaArticoliOperatore.Where(Function(x) x.IdArticolo = id).First.QtaArticolo + qta
                    db.SaveChanges()
                    Return Json(New With {.ok = True})
                Else
                    db.OrdiniInCorso.Add(New OrdiniInCorso With {
                        .UltimaModifica = New TipoUltimaModifica With {
                            .Data = DateTime.Now,
                            .Operatore = OpName,
                            .OperatoreID = OpID
                        },
                        .IdArticolo = id,
                        .QtaArticolo = qta
                    })
                    db.SaveChanges()
                    Return Json(New With {.ok = True})
                End If
            Else
                db.OrdiniInCorso.Add(New OrdiniInCorso With {
                        .UltimaModifica = New TipoUltimaModifica With {
                            .Data = DateTime.Now,
                            .Operatore = OpName,
                            .OperatoreID = OpID
                        },
                        .IdArticolo = id,
                        .QtaArticolo = qta
                    })
                db.SaveChanges()
                Return Json(New With {.ok = True})
            End If
        Catch ex As Exception

        End Try

        Return Json(New With {.ok = True})
    End Function
    <Authorize>
    Function CancellaArticoloInOrdine(ByVal id As Integer) As JsonResult
        Dim ordineDaRimuovere = db.OrdiniInCorso.Find(id)
        db.OrdiniInCorso.Remove(ordineDaRimuovere)
        db.SaveChanges()
        Return Json(New With {.ok = True})
    End Function
    <Authorize>
    Function InviaRichiestaAmministratore() As JsonResult
        Dim OpID = vbNullString
        Dim OpName = vbNullString
        Dim listArticoli = "<ul>"
        Try
            OpID = User.Identity.GetUserId()
            OpName = User.Identity.GetUserName()
            Dim listaRichiesteMateriali = db.OrdiniInCorso.Where(Function(x) x.UltimaModifica.OperatoreID = OpID).ToList
            Dim newIdOrdine = 0
            If db.Ordini.Count > 0 Then
                newIdOrdine = db.Ordini.OrderByDescending(Function(x) x.Id).First.IdOrdine + 1
            End If
            For Each l In listaRichiesteMateriali
                Dim art = db.Articoli.Find(l.IdArticolo)
                db.Ordini.Add(New Ordini With {
                    .IdArticolo = l.IdArticolo,
                    .IdOrdine = newIdOrdine,
                    .QtaArticolo = l.QtaArticolo,
                    .UltimaModifica = New TipoUltimaModifica With {
                        .Data = DateTime.Now,
                        .Operatore = OpName,
                        .OperatoreID = OpID
                    }
                })
                db.SaveChanges()
                db.OrdiniInCorso.Remove(l)
                db.SaveChanges()
                listArticoli = listArticoli + "<li>" + art.Nome_Articolo + " - q.tà " + l.QtaArticolo.ToString + "</li>"
            Next
            listArticoli = listArticoli + "</ul>"
            Dim mySmtp As New SmtpClient
            Dim myMail As New MailMessage()
            mySmtp.UseDefaultCredentials = False
            mySmtp.Credentials = New System.Net.NetworkCredential("hello@chefly.it", "Chefly2022!")
            mySmtp.Host = "chefly.it"
            myMail = New MailMessage()
            myMail.From = New MailAddress("hello@chefly.it")
            mySmtp.EnableSsl = False
            Dim StrContent = ""
            myMail.To.Add("mattiazucchini0601@gmail.com")
            myMail.Subject = "Richiesta Materiali"
            StrContent = "Ciao admin, <br> <br> qui di seguito trovi tutti gli articoli richiesti da '" + OpName + "'.<br>" + listArticoli + "Buon lavoro, <br>Mattia"
            myMail.Body = StrContent.ToString
            myMail.IsBodyHtml = True
            mySmtp.Send(myMail)
            Return Json(New With {.ok = True})
        Catch ex As Exception

        End Try
        Return Json(New With {.ok = True})
    End Function
    Public Function InvioMail(typemail As Boolean)
        Dim mySmtp As New SmtpClient
        Dim myMail As New MailMessage()
        mySmtp.UseDefaultCredentials = False
        mySmtp.Credentials = New System.Net.NetworkCredential("no-reply@chefly.it", "egz3391C_")
        mySmtp.Host = "mail.chefly.it"
        myMail = New MailMessage()
        myMail.From = New MailAddress("no-reply@chefly.it")
        Dim StrContent = ""
        myMail.To.Add("mattiazucchini0601@gmail.com")
        myMail.Subject = "Richiesta Materiali"
        Using reader = New StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Views/Shared/NuovoAccount.html")
            Dim readFile As String = reader.ReadToEnd()
            StrContent = readFile
            StrContent = StrContent.Replace("[Username]", "Claudia")
            StrContent = StrContent.Replace("[Motivo]", "E' stato inserito a sistema un nuovo ordine con acconto.")
        End Using
        myMail.Body = StrContent.ToString
        myMail.IsBodyHtml = True
        mySmtp.Send(myMail)
        Return 1
    End Function

    <HttpPost()>
    <Authorize>
    Function AddNota(Id As String, Nota As String, Type As Integer) As JsonResult '
        If IsNothing(Id) Then
            Return Json(New With {.ok = False, .message = "Errore: aggiunta nota -> " & Id & ". Impossibile recuperare l'elemento."})
        End If
        Dim OpID As String = vbNullString
        Dim OpName As String = vbNullString
        Try
            OpID = User.Identity.GetUserId()
            OpName = User.Identity.GetUserName()
            Dim n = db.Brighetti_Note.Add(New Brighetti_Note With {
                .TypeElem = Type,
                .Data_Nota = DateTime.Now,
                .Contenuto_Nota = Nota,
                .Operatore_Id = OpID,
                .Operatore_Nome = OpName,
                .IdEsterno = Id
            })
            db.SaveChanges()
            db.Audit.Add(New Audit With {
                                         .Livello = TipoAuditLivello.Info,
                                         .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                                         .Messaggio = "Aggiunta nota",
                                         .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(n),
                                        .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}
                           })
            db.SaveChanges()
            Return Json(New With {.ok = True, .message = "Nota correttamente aggiunta -> " & Id & ".", .id = Id}, JsonRequestBehavior.AllowGet)
        Catch ex As SystemException
            db.Log.Add(New Log With {
                 .UltimaModifica = New TipoUltimaModifica With {.Data = DateTime.Now, .OperatoreID = OpID, .Operatore = OpName},
                 .Livello = TipoLogLivello.Errors,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Errore aggiunta nota -> " & ex.Message,
                 .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {.id = Id})
                 })
            db.SaveChanges()
            Return Json(New With {.ok = False, .message = "Errore: Dettagli Aggiunta note -> " & Id & ";" & ex.Message})
        End Try


    End Function

    <HttpPost()>
    <Authorize>
    Function DeleteNota(ByVal id As Integer)
        If IsNothing(id) Then
            Return Json(New With {.ok = False, .message = "Errore: Dettagli File -> " & id & ". Impossibile recuperare l'elemento."})
        End If
        Dim OpID As String = vbNullString
        Dim OpName As String = vbNullString
        Try
            OpID = User.Identity.GetUserId()
            OpName = User.Identity.GetUserName()

            Dim nota = db.Brighetti_Note.Find(id)
            db.Brighetti_Note.Remove(nota)
            db.SaveChanges()

            db.Audit.Add(New Audit With {
                .Livello = TipoAuditLivello.Info,
                .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                .Messaggio = "Eliminata nota",
                .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(id),
                .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}
            })
            db.SaveChanges()
            Return Json(New With {.ok = True, .message = "Nota correttamente eliminata -> " & id & ".", .id = id}, JsonRequestBehavior.AllowGet)
        Catch ex As SystemException
            db.Log.Add(New Log With {
                 .UltimaModifica = New TipoUltimaModifica With {.Data = DateTime.Now, .OperatoreID = OpID, .Operatore = OpName},
                 .Livello = TipoLogLivello.Errors,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Errore elimina nota -> " & ex.Message,
                 .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {.id = id})
                 })
            db.SaveChanges()
            Return Json(New With {.ok = False, .message = "Errore: Dettagli Elimina note -> " & id & ";" & ex.Message})
        End Try
    End Function
    ' GET: Brighetti_Articolo/Details/5
    <HttpPost()>
    <Authorize>
    Function AddFile(id As String, File As HttpPostedFileBase, Type As Integer) As JsonResult
        If IsNothing(id) Then
            Return Json(New With {.ok = False, .message = "Errore: Dettagli File -> " & id & ". Impossibile recuperare l'elemento."})
        End If
        Dim OpID As String = vbNullString
        Dim OpName As String = vbNullString
        Try
            OpID = User.Identity.GetUserId()
            OpName = User.Identity.GetUserName()
            Try
                If Not IsNothing(File) Then
                    Dim UploadedFile As HttpPostedFileBase = File
                    If UploadedFile IsNot Nothing AndAlso UploadedFile.ContentLength > 0 Then
                        Dim pathTMP = Path.Combine(Server.MapPath("~/Content/upload_documenti"), UploadedFile.FileName.ToString.Replace(" ", String.Empty))
                        UploadedFile.SaveAs(pathTMP)
                        db.Brighetti_Documenti.Add(New Brighetti_Documenti With {
                                .DataCreazioneFile = DateTime.Now,
                                .Nome_File = UploadedFile.FileName,
                                .TypeElem = Type,
                                .Operatore_Id = OpID,
                                .Operatore_Nome = OpName,
                                .Percorso_File = pathTMP,
                                .IdEsterno = id
                            })
                        db.SaveChanges()
                    End If
                End If
            Catch ex As SystemException
                db.Log.Add(New Log With {
                .UltimaModifica = New TipoUltimaModifica With {.Data = DateTime.Now, .OperatoreID = OpID, .Operatore = OpName},
                .Livello = TipoLogLivello.Errors,
                .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                .Messaggio = "Errore lettura file -> " & ex.Message,
                .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {.id = id})
                })
                db.SaveChanges()
                Return Json(New With {.ok = False, .message = "Errore: lettura file -> " & id & ";" & ex.Message})
            End Try
            db.SaveChanges()
            db.Audit.Add(New Audit With {
                .Livello = TipoAuditLivello.Info,
                .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                .Messaggio = "Aggiunto File",
                .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {.id = id, .file = id}),
                .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}
            })
            db.SaveChanges()
            Return Json(New With {.ok = True, .message = "File correttamente aggiunto -> " & id & ".", .id = id}, JsonRequestBehavior.AllowGet)
        Catch ex As SystemException
            db.Log.Add(New Log With {
                 .UltimaModifica = New TipoUltimaModifica With {.Data = DateTime.Now, .OperatoreID = OpID, .Operatore = OpName},
                 .Livello = TipoLogLivello.Errors,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Errore aggiunta file -> " & ex.Message,
                 .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {.id = id})
                 })
            db.SaveChanges()
            Return Json(New With {.ok = False, .message = "Errore: Dettagli Aggiunta file -> " & id & ";" & ex.Message})
        End Try


    End Function

    <HttpPost()>
    <Authorize>
    Function DeleteFile(ByVal id As Integer)
        If IsNothing(id) Then
            Return Json(New With {.ok = False, .message = "Errore: Dettagli File -> " & id & ". Impossibile recuperare l'elemento."})
        End If
        Dim OpID As String = vbNullString
        Dim OpName As String = vbNullString
        Try
            OpID = User.Identity.GetUserId()
            OpName = User.Identity.GetUserName()

            Dim file = db.Brighetti_Documenti.Find(id)
            If (Not IsNothing(file)) Then
                System.IO.File.Delete(file.Percorso_File)
                db.Brighetti_Documenti.Remove(file)
                db.SaveChanges()
            End If

            db.Audit.Add(New Audit With {
                 .UltimaModifica = New TipoUltimaModifica With {.Data = DateTime.Now, .OperatoreID = OpID, .Operatore = OpName},
                 .Livello = TipoLogLivello.Errors,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "File rimosso correttamente",
                 .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {.id = id})
                 })
            db.SaveChanges()
            Return Json(New With {.ok = True, .message = "File rimosso correttamente -> " & id & ".", .id = id}, JsonRequestBehavior.AllowGet)

        Catch ex As SystemException
            db.Log.Add(New Log With {
                 .UltimaModifica = New TipoUltimaModifica With {.Data = DateTime.Now, .OperatoreID = OpID, .Operatore = OpName},
                 .Livello = TipoLogLivello.Errors,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Errore eliminazione file -> " & ex.Message,
                 .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {.id = id})
                 })
            db.SaveChanges()
            Return Json(New With {.ok = False, .message = "Errore: Dettagli Eliminazione file -> " & id & ";" & ex.Message})
        End Try

    End Function
    Function DownloadFile(ByVal id As Integer) As FileResult
        Dim OpID As String = vbNullString
        Dim OpName As String = vbNullString
        Try
            OpID = User.Identity.GetUserId()
            OpName = User.Identity.GetUserName()
            Dim fileDoc = db.Brighetti_Documenti.Where(Function(x) x.Id = id).First
            db.Audit.Add(New Audit With {
                                         .Livello = TipoAuditLivello.Info,
                                         .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                                         .Messaggio = "Scaricato File",
                                         .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {.id = id}),
                                        .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}
                           })
            db.SaveChanges()
            Return File(IO.File.ReadAllBytes(fileDoc.Percorso_File), "application/octet-stream", fileDoc.Nome_File)

        Catch ex As SystemException
            db.Log.Add(New Log With {
                                              .Livello = TipoLogLivello.Errors,
                                              .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                                              .Messaggio = "Errore: " + ex.Message,
                                              .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {.Disegno = "errore"}),
                                             .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}
                                })
            db.SaveChanges()
        End Try
        Return Nothing
    End Function

    Public Function ListaMacchine() As JsonResult
        Dim result As New List(Of Brighetti_Macchina)
        For Each m In db.Brighetti_Macchine.ToList
            Dim pin = db.Brighetti_Planimetria.Where(Function(x) x.IdEsternoMacchina = m.IdMacchina).FirstOrDefault

            If IsNothing(pin) Then
                result.Add(m)
            End If
        Next

        Dim final = New SelectList(result, "IdMacchina", "NomeMacchina")

        Return Json(New With {.listaMacchine = final, .ok = True}, JsonRequestBehavior.AllowGet)
    End Function

    Public Function SalvaPinpoint(ByVal id As String, ByVal posX As String, ByVal posY As String) As JsonResult
        Dim opID = vbNullString
        Dim opName = vbNullString
        Try
            opID = User.Identity.GetUserId
            opName = User.Identity.Name

            If Not IsNothing(id) And Not IsNothing(posX) And Not IsNothing(posY) Then
                db.Brighetti_Planimetria.Add(New Brighetti_Planimetria With {
                    .IdEsternoMacchina = id,
                    .posX = posX,
                    .posY = posY,
                    .UltimaModifica = New TipoUltimaModifica With {
                        .Data = DateTime.Now(),
                        .OperatoreID = opID,
                        .Operatore = opName
                    }
                })
                db.SaveChanges()
                Return Json(New With {.message = "Pinpoint aggiunto", .ok = True}, JsonRequestBehavior.AllowGet)
            End If
        Catch ex As Exception

        End Try
    End Function

    Public Function getDataPinPoint(ByVal id As Integer) As JsonResult
        If Not IsNothing(id) Then
            Dim macchina = db.Brighetti_Macchine.Find(id)

            Return Json(New With {.descMacchina = macchina, .ok = True}, JsonRequestBehavior.AllowGet)
        End If
    End Function

    Public Function deletePinPoint(ByVal id As Integer)
        Dim OpID As String = vbNullString
        Dim OpName As String = vbNullString
        Try
            OpID = User.Identity.GetUserId()
            OpName = User.Identity.GetUserName()

            If Not IsNothing(id) Then
                Dim pinpointMacchina = db.Brighetti_Planimetria.Where(Function(x) x.IdEsternoMacchina = id).FirstOrDefault

                db.Brighetti_Planimetria.Remove(pinpointMacchina)
                db.SaveChanges()
            End If

            db.Audit.Add(New Audit With {
                 .UltimaModifica = New TipoUltimaModifica With {.Data = DateTime.Now, .OperatoreID = OpID, .Operatore = OpName},
                 .Livello = TipoLogLivello.Info,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Pinpoint rimosso correttamente",
                 .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {.id = id})
            })
            db.SaveChanges()
            Return Json(New With {.ok = True, .message = "Pinpoint rimosso correttamente -> " & id & ".", .id = id}, JsonRequestBehavior.AllowGet)

        Catch ex As SystemException
            db.Log.Add(New Log With {
                 .UltimaModifica = New TipoUltimaModifica With {.Data = DateTime.Now, .OperatoreID = OpID, .Operatore = OpName},
                 .Livello = TipoLogLivello.Errors,
                 .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                 .Messaggio = "Errore eliminazione pinpoint -> " & ex.Message,
                .Dati = Newtonsoft.Json.JsonConvert.SerializeObject(New With {.id = id})
            })
            db.SaveChanges()
            Return Json(New With {.ok = False, .message = "Errore: Dettagli Eliminazione pinpoint -> " & id & ";" & ex.Message})
        End Try
    End Function

End Class
