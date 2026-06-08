Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Entity
Imports System.Globalization
Imports System.Linq
Imports System.Net
Imports System.Web
Imports System.Web.Mvc
Imports Brighetti
Imports Microsoft.AspNet.Identity
Imports Newtonsoft.Json

Namespace Controllers
    Public Class MacchineController
        Inherits System.Web.Mvc.Controller

        Private db As New BrighettiModels
        Private appctx As New ApplicationDbContext

        ' GET: Macchine
        <Authorize>
        Function Dashboard() As ActionResult
            Dim OpID = vbNullString
            Try
                OpID = User.Identity.GetUserId()
                Dim MacchineProfile = appctx.MacchineUsers.Where(Function(x) x.IdUtente = OpID).ToList
                Dim ListaMacchine As New List(Of Macchine)
                For Each m In MacchineProfile
                    Dim macc = db.Macchine.Where(Function(x) x.Id = m.IdMacchina).FirstOrDefault
                    If Not IsNothing(macc) Then
                        ListaMacchine.Add(macc)
                    End If
                Next
                Return View(ListaMacchine)
            Catch ex As Exception

            End Try
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End Function
        <Authorize>
        Function Istanza(ByVal id As Integer) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim macchina = db.Brighetti_Macchine.Find(id)
            Dim datenow = DateTime.Now
            Dim dateAWeek = datenow.AddDays(-7)
            Dim listaDatiMacchina = db.Brighetti_Dati_Macchina.Where(Function(x) x.idMacchina = id And x.DataRilevazione < datenow And x.DataRilevazione > dateAWeek).OrderByDescending(Function(y) y.id).ToList
            Dim listaNote = db.Brighetti_Note.Where(Function(x) x.IdEsterno = macchina.IdMacchina And x.TypeElem = TipoElemento.Macchina).OrderByDescending(Function(y) y.Data_Nota).ToList
            Dim listaDocumenti = db.Brighetti_Documenti.Where(Function(x) x.IdEsterno = macchina.IdMacchina And x.TypeElem = TipoElemento.Macchina).OrderByDescending(Function(y) y.DataCreazioneFile).ToList
            Dim datimacchina = db.Brighetti_Dati_Macchina.Where(Function(w) w.idMacchina = id).GroupBy(Function(p) p.EsecuzioneMacchina).Select(Function(p) New With {.Stato = p.Key, .Number = p.Count}).ToList
            ViewBag.DatiMacchina = JsonConvert.SerializeObject(datimacchina)
            Return View(New Brighetti_Macchina_Viewmodel With {
                .CollegamentoMacchina = macchina.CollegamentoMacchina,
                .DescrizioneMacchina = macchina.DescrizioneMacchina.ToString,
                .IdMacchina = macchina.IdMacchina,
                .IdReparto = macchina.IdReparto,
                .IdUtente = macchina.IdUtente,
                .ListaDatiMacchina = listaDatiMacchina,
                .ListaDocumenti = listaDocumenti,
                .ListaNote = listaNote,
                .NomeMacchina = macchina.NomeMacchina,
                .TipologiaMacchina = macchina.TipologiaMacchina,
                .UltimaModifica = macchina.UltimaModifica
            })
        End Function
        <Authorize()>
        Function AdminDashboard() As ActionResult
            Dim OpID = vbNullString
            Try
                OpID = User.Identity.GetUserId()
                Dim listBrighettiMacchine = db.Brighetti_Macchine.ToList
                Dim retList As New List(Of Brighetti_Macchina_Viewmodel)
                For Each l In listBrighettiMacchine
                    Dim accesa = db.Brighetti_Attività.Where(Function(x) x.idMacchinaMagazzino = l.IdMacchina And (x.StatoAttività = TipoStatoAttività.In_Lavorazione Or x.StatoAttività = TipoStatoAttività.In_Attrezzaggio)).Count
                    Dim pinpoint = db.Brighetti_Planimetria.Where(Function(x) x.IdEsternoMacchina = l.IdMacchina).FirstOrDefault
                    If Not IsNothing(pinpoint) Then
                        retList.Add(New Brighetti_Macchina_Viewmodel With {
                        .DescrizioneMacchina = l.DescrizioneMacchina,
                        .IdMacchina = l.IdMacchina,
                        .IdReparto = l.IdReparto,
                        .IdUtente = l.IdUtente,
                        .NomeMacchina = l.NomeMacchina,
                        .UltimaModifica = l.UltimaModifica,
                        .MacchinaAccesa = IIf(accesa > 0, True, False),
                        .posX = pinpoint.posX,
                        .posY = pinpoint.posY
                    })
                    End If
                Next

                Return View(retList)
            Catch ex As Exception

            End Try
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End Function
        ' GET: Macchine/Details/5
        <Authorize>
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim macchine As Macchine = db.Macchine.Find(id)
            If IsNothing(macchine) Then
                Return HttpNotFound()
            End If
            Return View(macchine)
        End Function

        ' GET: Macchine/Create
        <Authorize(Roles:="Admin")>
        Function Create() As ActionResult
            Return View()
        End Function
        Function MacchineStati() As ActionResult
            Dim listaMacchine As New List(Of MacchineStatiViewModel)
            Dim DataOggi = DateTime.Now.Date
            Dim DataIeri = DataOggi.AddDays(-1S)
            For Each m In db.Brighetti_Macchine.ToList
                Select Case m.TipologiaMacchina
                    Case TipoMacchina.mtconnect
                        Dim ultimiDatiMacchina = db.Brighetti_Dati_Macchina.Where(Function(x) x.idMacchina = m.IdMacchina).ToList.LastOrDefault
                        If Not IsNothing(ultimiDatiMacchina) Then
                            Dim tempoLavorazioneEffetivo = 0
                            Dim FirstLettura = True
                            Dim prevLettura As New Brighetti_Dati_Macchina
                            For Each stato In db.Brighetti_Dati_Macchina.Where(Function(x) x.idMacchina = m.IdMacchina And x.DataRilevazione >= DataIeri And x.DataRilevazione <= DataOggi And x.StatoOperativoMacchina = "AUTOMATIC").ToList
                                If FirstLettura Then
                                    FirstLettura = False
                                    prevLettura = stato
                                Else
                                    tempoLavorazioneEffetivo = tempoLavorazioneEffetivo + DateDiff(DateInterval.Minute, prevLettura.DataRilevazione, stato.DataRilevazione)
                                    prevLettura = stato
                                End If
                            Next
                            Dim percentualetempoEffetivoLavorazione = 0
                            Dim percentualeDifftempoEffetivoLavorazione = 100
                            If tempoLavorazioneEffetivo <> 0 Then
                                percentualetempoEffetivoLavorazione = (tempoLavorazioneEffetivo * 100) / 480
                                percentualeDifftempoEffetivoLavorazione = 100 - percentualetempoEffetivoLavorazione
                            End If
                            Dim retGrafico = "<div class='progress'>
                                  <div class='progress-bar bg-success' role='progressbar' style='width: " + percentualetempoEffetivoLavorazione.ToString + "%' aria-valuenow='" + percentualetempoEffetivoLavorazione.ToString + "' aria-valuemin='0' aria-valuemax='100'> " + percentualetempoEffetivoLavorazione.ToString + "%</div>
                                  <div class='progress-bar bg-danger' role='progressbar' style='width: " + percentualeDifftempoEffetivoLavorazione.ToString + "%' aria-valuenow='" + percentualeDifftempoEffetivoLavorazione.ToString + "' aria-valuemin='0' aria-valuemax='100'></div>
                                </div>
                                "
                            listaMacchine.Add(New MacchineStatiViewModel With {
                                .NomeMacchina = m.NomeMacchina,
                                .IdMacchina = m.IdMacchina,
                                .StatoMacchina = ultimiDatiMacchina.StatoOperativoMacchina,
                                .UltimiDatiRilevatia = ultimiDatiMacchina.DataRilevazione.ToString.Split(" ")(0),
                                .UltimoProgrammaMacchina = ultimiDatiMacchina.ProgrammaDescrizione,
                                .GraficoAndamento = retGrafico
                            })
                        Else
                            listaMacchine.Add(New MacchineStatiViewModel With {
                                .NomeMacchina = m.NomeMacchina,
                                .IdMacchina = m.IdMacchina,
                                .StatoMacchina = "-",
                                .UltimiDatiRilevatia = "-",
                                .UltimoProgrammaMacchina = "-"
                            })
                        End If
                    Case TipoMacchina.Scheenberger
                        Dim ultimiDatiMacchina = db.Brighetti_Dati_Macchina_Csv.Where(Function(x) x.idMacchina = m.IdMacchina).ToList.LastOrDefault
                        If Not IsNothing(ultimiDatiMacchina) Then
                            Dim tempoLavorazioneEffetivo = 0
                            Dim prevLettura As New Brighetti_Dati_Macchina_Csv
                            For Each stato In db.Brighetti_Dati_Macchina_Csv.Where(Function(x) x.idMacchina = m.IdMacchina And x.DataRilevazione >= DataIeri And x.DataRilevazione <= DataOggi).ToList
                                tempoLavorazioneEffetivo = tempoLavorazioneEffetivo + DateDiff(DateInterval.Minute, stato.DataInizioLavorazione, stato.DataFineLavorazione)
                            Next
                            Dim percentualetempoEffetivoLavorazione = 0
                            Dim percentualeDifftempoEffetivoLavorazione = 100
                            If tempoLavorazioneEffetivo <> 0 Then
                                percentualetempoEffetivoLavorazione = (tempoLavorazioneEffetivo * 100) / 480
                                percentualeDifftempoEffetivoLavorazione = 100 - percentualetempoEffetivoLavorazione
                            End If
                            Dim retGrafico = "<div class='progress'>
                                  <div class='progress-bar bg-success' role='progressbar' style='width: " + percentualetempoEffetivoLavorazione.ToString + "%' aria-valuenow='" + percentualetempoEffetivoLavorazione.ToString + "' aria-valuemin='0' aria-valuemax='100'> " + percentualetempoEffetivoLavorazione.ToString + "%</div>
                                  <div class='progress-bar bg-danger' role='progressbar' style='width: " + percentualeDifftempoEffetivoLavorazione.ToString + "%' aria-valuenow='" + percentualeDifftempoEffetivoLavorazione.ToString + "' aria-valuemin='0' aria-valuemax='100'></div>
                                </div>
                                "
                            listaMacchine.Add(New MacchineStatiViewModel With {
                                .NomeMacchina = m.NomeMacchina,
                                .IdMacchina = m.IdMacchina,
                                .StatoMacchina = "Lavorazione",
                                .UltimiDatiRilevatia = ultimiDatiMacchina.DataRilevazione.ToString.Split(" ")(0),
                                .UltimoProgrammaMacchina = ultimiDatiMacchina.ProgrammaDescrizione,
                                .GraficoAndamento = retGrafico
                            })
                        Else
                            listaMacchine.Add(New MacchineStatiViewModel With {
                                .NomeMacchina = m.NomeMacchina,
                                .IdMacchina = m.IdMacchina,
                                .StatoMacchina = "-",
                                .UltimiDatiRilevatia = "-",
                                .UltimoProgrammaMacchina = "-"
                            })
                        End If
                    Case Else
                        listaMacchine.Add(New MacchineStatiViewModel With {
                                .NomeMacchina = m.NomeMacchina,
                                .IdMacchina = m.IdMacchina,
                                .StatoMacchina = "-",
                                .UltimiDatiRilevatia = "-",
                                .UltimoProgrammaMacchina = "-"
                            })
                End Select
            Next
            Return PartialView(listaMacchine)
        End Function
        <Authorize(Roles:="Admin")>
        Function Analisi(ByVal id As Integer, dateTimeUser As String) As ActionResult
            Dim DataOggiProd = DateTime.Now.Date
            Dim DataIeriProd = DateTime.Now.Date.AddDays(-7)
            Dim ret As New AnalisiViewModel
            Dim OpID = vbNullString
            Dim OpName = vbNullString
            Dim provider As New CultureInfo("it-IT")
            Try
                If dateTimeUser <> "" Then
                    Try
                        DataIeriProd = Date.ParseExact(dateTimeUser.Split("-")(0), "yyyyMMdd", provider)
                        DataOggiProd = Date.ParseExact(dateTimeUser.Split("-")(1), "yyyyMMdd", provider)
                    Catch ex As Exception

                    End Try
                End If
                OpID = User.Identity.GetUserId
                OpName = User.Identity.GetUserName
                Dim m = db.Brighetti_Macchine.Find(id)
                ret.NomeMacchina = m.NomeMacchina
                'Grafico Efficienza
                Select Case m.TipologiaMacchina
                    Case TipoMacchina.mtconnect
                        Dim listaLett As New List(Of EfficienzaViewModel)
                        For i = 1 To 7
                            Dim tempoLavorazioneEffetivo = 0
                            Dim DataOggi = DateTime.Now.Date.AddDays(-i + 1)
                            Dim DataIeri = DateTime.Now.Date.AddDays(-i)
                            Dim FirstLettura = True
                            Dim prevLettura As New Brighetti_Dati_Macchina
                            For Each stato In db.Brighetti_Dati_Macchina.Where(Function(x) x.idMacchina = m.IdMacchina And x.DataRilevazione >= DataIeri And x.DataRilevazione <= DataOggi And x.StatoOperativoMacchina = "AUTOMATIC").ToList
                                If FirstLettura Then
                                    FirstLettura = False
                                    prevLettura = stato
                                Else
                                    tempoLavorazioneEffetivo = tempoLavorazioneEffetivo + DateDiff(DateInterval.Minute, prevLettura.DataRilevazione, stato.DataRilevazione)
                                    prevLettura = stato
                                End If
                            Next
                            Dim percentualetempoEffetivoLavorazione = 0
                            Dim percentualeDifftempoEffetivoLavorazione = 100
                            If tempoLavorazioneEffetivo <> 0 Then
                                percentualetempoEffetivoLavorazione = (tempoLavorazioneEffetivo * 100) / 480
                                percentualeDifftempoEffetivoLavorazione = 100 - percentualetempoEffetivoLavorazione
                            End If
                            listaLett.Add(New EfficienzaViewModel With {
                                .Data = DataIeri.ToString.Split(" ")(0),
                                .PercEfficienza = percentualetempoEffetivoLavorazione,
                                .PercTotale = percentualeDifftempoEffetivoLavorazione
                            })
                        Next
                        ret.ListaEff = JsonConvert.SerializeObject(listaLett)
                        'Datatable con roba prodotta
                        Dim listaProduzione As New List(Of ProduzioneViewModel)
                        For Each att In db.Brighetti_Attività.Where(Function(x) x.idMacchinaMagazzino = id And x.TipoAttivitaProcedura = TipoAttivitaProcedura.Lavorazione And x.UltimaModifica.Data >= DataIeriProd And x.UltimaModifica.Data <= DataOggiProd).ToList
                            Dim totaleTempo = 0
                            For Each tempo In db.Brighetti_Tempi.Where(Function(x) x.IdAttività = att.IdAttività).ToList
                                totaleTempo = totaleTempo + DateDiff(DateInterval.Minute, tempo.DataInizio, tempo.DataFine)
                            Next
                            listaProduzione.Add(New ProduzioneViewModel With {
                                .Articolo = att.CodiceArticolo,
                                .QuantitàProdotta = att.QuantitàProdotta,
                                .QuantitàScartata = att.QuantitàScartata,
                                .TempoDiProduzione = totaleTempo
                            })
                        Next
                        ret.ListaProduzione = listaProduzione
                    Case TipoMacchina.Scheenberger
                        Dim listaLett As New List(Of EfficienzaViewModel)
                        For i = 1 To 7
                            Dim tempoLavorazioneEffetivo = 0
                            Dim DataOggi = DateTime.Now.Date.AddDays(-i + 1)
                            Dim DataIeri = DateTime.Now.Date.AddDays(-i)
                            Dim FirstLettura = True
                            Dim prevLettura As New Brighetti_Dati_Macchina
                            For Each stato In db.Brighetti_Dati_Macchina_Csv.Where(Function(x) x.idMacchina = m.IdMacchina And x.DataRilevazione >= DataIeri And x.DataRilevazione <= DataOggi).ToList
                                tempoLavorazioneEffetivo = tempoLavorazioneEffetivo + DateDiff(DateInterval.Minute, stato.DataInizioLavorazione, stato.DataFineLavorazione)
                            Next
                            Dim percentualetempoEffetivoLavorazione = 0
                            Dim percentualeDifftempoEffetivoLavorazione = 100
                            If tempoLavorazioneEffetivo <> 0 Then
                                percentualetempoEffetivoLavorazione = (tempoLavorazioneEffetivo * 100) / 480
                                percentualeDifftempoEffetivoLavorazione = 100 - percentualetempoEffetivoLavorazione
                            End If
                            listaLett.Add(New EfficienzaViewModel With {
                                .Data = DataIeri.ToString.Split(" ")(0),
                                .PercEfficienza = percentualetempoEffetivoLavorazione,
                                .PercTotale = percentualeDifftempoEffetivoLavorazione
                            })
                        Next
                        ret.ListaEff = JsonConvert.SerializeObject(listaLett)
                        'Datatable con roba prodotta
                        Dim listaProduzione As New List(Of ProduzioneViewModel)
                        For Each att In db.Brighetti_Attività.Where(Function(x) x.idMacchinaMagazzino = id And x.TipoAttivitaProcedura = TipoAttivitaProcedura.Lavorazione And x.UltimaModifica.Data >= DataIeriProd And x.UltimaModifica.Data <= DataOggiProd).ToList
                            Dim totaleTempo = 0
                            For Each tempo In db.Brighetti_Tempi.Where(Function(x) x.IdAttività = att.IdAttività).ToList
                                totaleTempo = totaleTempo + DateDiff(DateInterval.Minute, tempo.DataInizio, tempo.DataFine)
                            Next
                            listaProduzione.Add(New ProduzioneViewModel With {
                                .Articolo = att.CodiceArticolo,
                                .QuantitàProdotta = att.QuantitàProdotta,
                                .QuantitàScartata = att.QuantitàScartata,
                                .TempoDiProduzione = totaleTempo
                            })
                        Next
                        ret.ListaProduzione = listaProduzione
                End Select
            Catch ex As Exception
                db.Log.Add(New Log With {
                              .Livello = TipoLogLivello.Warning,
                              .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                              .Messaggio = "Errore creazione modello Analisi: " & vbNewLine & ex.Message,
                              .Dati = "",
                              .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
            End Try
            Try
                'Lista attività in corso o da cominciare
                Dim listOfActivities = db.Brighetti_Attività.Where(Function(x) x.idMacchinaMagazzino = id And (x.StatoAttività = TipoStatoAttività.In_attesa Or x.StatoAttività = TipoStatoAttività.StandBy Or x.StatoAttività = TipoStatoAttività.BloccoDaAttivitàPrecedente)).ToList
                Dim ListaPrevisione As New List(Of PrevisioneViewModel)
                For Each l In listOfActivities
                    ListaPrevisione.Add(New PrevisioneViewModel With {
                        .Articolo = l.CodiceArticolo,
                        .DataCreazione = l.UltimaModifica.Data.ToString.Split(" ")(0),
                        .qtaProdurre = l.QuantitàdaProdurre
                    })
                Next
                ret.ListaPrevisione = ListaPrevisione
                'Gauge vari
                Dim countCoda = db.Brighetti_Attività.Where(Function(x) x.idMacchinaMagazzino = id And (x.StatoAttività = TipoStatoAttività.In_attesa Or x.StatoAttività = TipoStatoAttività.StandBy Or x.StatoAttività = TipoStatoAttività.BloccoDaAttivitàPrecedente)).Count
                Dim listPezziProdotti = db.Brighetti_Attività.Where(Function(x) x.idMacchinaMagazzino = id And x.UltimaModifica.Data >= DataIeriProd And x.UltimaModifica.Data <= DataOggiProd And x.StatoAttività = TipoStatoAttività.Completato).ToList
                Dim countPezziProdotti = 0
                Dim countPezziScartati = 0
                For Each l In listPezziProdotti
                    countPezziProdotti += l.QuantitàProdotta
                    countPezziScartati += l.QuantitàScartata
                Next
                ret.GaugeCoda = countCoda
                ret.GaugeProdotti = countPezziProdotti
                ret.GaugeScartati = countPezziScartati
            Catch ex As Exception
                db.Log.Add(New Log With {
                            .Livello = TipoLogLivello.Warning,
                            .Indirizzo = ControllerContext.RouteData.Values("controller") & "/" & ControllerContext.RouteData.Values("action"),
                            .Messaggio = "Errore creazione modello Analisi gauge: " & vbNewLine & ex.Message,
                            .Dati = "",
                            .UltimaModifica = New TipoUltimaModifica With {.OperatoreID = OpID, .Operatore = OpName, .Data = DateTime.Now}})
                db.SaveChanges()
            End Try
            ret.IdMacchina = id
            Return View(ret)
        End Function
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Admin")>
        Function Create(<Bind(Include:="NomeMacchina,DescrizioneMacchina,IndirizzoMacchina")> ByVal macchine As Macchine) As ActionResult
            If ModelState.IsValid Then
                db.Macchine.Add(macchine)
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(macchine)
        End Function

        ' GET: Macchine/Edit/5
        <Authorize(Roles:="Admin")>
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim macchine As Macchine = db.Macchine.Find(id)
            If IsNothing(macchine) Then
                Return HttpNotFound()
            End If
            Dim retValue As New MacchineViewModel With {
            .DescrizioneMacchina = macchine.DescrizioneMacchina,
            .Id = macchine.Id,
            .IndirizzoMacchina = macchine.IndirizzoMacchina,
            .NomeMacchina = macchine.NomeMacchina
            }
            Dim listUsers = appctx.Users.ToList
            Dim userListFin As New List(Of UtentiMacchineViewModel)
            For Each l In listUsers
                userListFin.Add(New UtentiMacchineViewModel With {
                    .IdUtente = l.Id,
                    .Username = l.UserName
                })
            Next
            ViewBag.IdUtente = New SelectList(userListFin, "IdUtente", "Username")
            Return View(retValue)
        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="Id,NomeMacchina,DescrizioneMacchina,IndirizzoMacchina,IdUtente")> ByVal macchine As MacchineViewModel) As ActionResult
            If ModelState.IsValid Then
                Dim macchina = db.Macchine.Where(Function(x) x.Id = macchine.Id).FirstOrDefault
                If macchina.IndirizzoMacchina <> macchine.IndirizzoMacchina Then
                    macchina.IndirizzoMacchina = macchine.IndirizzoMacchina
                    db.SaveChanges()
                End If
                If macchina.NomeMacchina <> macchine.NomeMacchina Then
                    macchina.NomeMacchina = macchine.NomeMacchina
                    db.SaveChanges()
                End If
                If macchina.DescrizioneMacchina <> macchine.DescrizioneMacchina Then
                    macchina.DescrizioneMacchina = macchine.DescrizioneMacchina
                    db.SaveChanges()
                End If

                If appctx.MacchineUsers.Where(Function(x) x.IdUtente = macchine.IdUtente And x.IdMacchina = macchine.Id).Count = 0 Then
                    Try
                        appctx.MacchineUsers.Add(New MacchineUsers With {
                                .IdMacchina = macchine.Id,
                                .IdUtente = macchine.IdUtente
                            })
                        appctx.SaveChanges()
                    Catch ex As Exception

                    End Try
                End If
            End If
            Return RedirectToAction("AdminDashboard", "Macchine")
        End Function

        ' GET: Macchine/Delete/5
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim macchine As Macchine = db.Macchine.Find(id)
            If IsNothing(macchine) Then
                Return HttpNotFound()
            End If
            Return View(macchine)
        End Function

        ' POST: Macchine/Delete/5
        <HttpPost()>
        <ActionName("Delete")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As Integer) As ActionResult
            Dim macchine As Macchine = db.Macchine.Find(id)
            db.Macchine.Remove(macchine)
            db.SaveChanges()
            Return RedirectToAction("Index")
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Public Function getDatiMacchina(ByVal id As Integer) As JsonResult
            Dim macchina = db.Brighetti_Macchine.Find(id)
            Dim programmaDesc = ""
            Dim dataRilevazione = ""
            Dim attivitaMacchina = db.Brighetti_Attività.Where(Function(x) x.idMacchinaMagazzino = id And (x.StatoAttività = TipoStatoAttività.In_Lavorazione Or x.StatoAttività = TipoStatoAttività.In_Attrezzaggio)).FirstOrDefault
            Dim attivita = ""
            If Not IsNothing(attivitaMacchina) Then
                Dim tempotot = DateDiff(DateInterval.Minute, Convert.ToDateTime(attivitaMacchina.UltimaModifica.Data), DateTime.Now)
                attivita = attivitaMacchina.NomeAttività + " - in lavorazione da: " + tempotot.ToString.ToString + " min."
            End If
            Select Case macchina.TipologiaMacchina
                Case TipoMacchina.mtconnect
                    Dim datiMac = db.Brighetti_Dati_Macchina.Where(Function(x) x.idMacchina = id).ToList.Last
                    programmaDesc = datiMac.ProgrammaDescrizione
                    dataRilevazione = datiMac.DataRilevazione
                Case TipoMacchina.Scheenberger
                    Dim datiMac = db.Brighetti_Dati_Macchina_Csv.Where(Function(x) x.idMacchina = id).ToList.Last
                    programmaDesc = datiMac.ProgrammaDescrizione
                    dataRilevazione = datiMac.DataRilevazione
            End Select
            If programmaDesc = "" Then
                programmaDesc = attivita
                dataRilevazione = DateTime.Now.Date.ToString.Split(" ")(0)
            End If
            Return Json(New With {.ok = True, .programmaDesc = programmaDesc, .dataRilevazione = dataRilevazione})
        End Function
    End Class
End Namespace
