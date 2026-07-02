@ModelType IEnumerable(Of Brighetti.Brighetti_OrdineAutomatico)
@Code
    ViewData("Title") = "Ordini Automatici"
End Code

<div class="container" style="margin-top:24px; margin-bottom:40px;">
    <div class="d-flex justify-content-between align-items-center flex-wrap mb-3">
        <div>
            <h2 class="mb-1">Proposte di riordino automatico</h2>
            <div class="text-muted">
                Stato automatismo:
                @If ViewBag.OrdiniAutomaticiAbilitato Then
                    @<span class="badge bg-success"><i class="fa-solid fa-circle-check"></i> ATTIVO</span>
                Else
                    @<span class="badge bg-secondary"><i class="fa-solid fa-circle-pause"></i> SPENTO</span>
                End If
                &nbsp;·&nbsp; Prefissi codice: <code>@(If(String.IsNullOrEmpty(ViewBag.Prefissi), "tutti", ViewBag.Prefissi))</code>
                &nbsp;·&nbsp; <a href="@Url.Action("Index", "Brighetti_Impostazioni")">Configura nel pannello Automazioni</a>
            </div>
        </div>
        @Using (Html.BeginForm("Genera", "Brighetti_OrdiniAutomatici"))
            @Html.AntiForgeryToken()
            @<button type="submit" class="btn btn-primary">
                <i class="fa-solid fa-rotate"></i> Genera proposte ora
            </button>
        End Using
    </div>

    @If TempData("Messaggio") IsNot Nothing Then
        @<div class="alert alert-info">@TempData("Messaggio")</div>
    End If

    <div class="card shadow-sm">
        <div class="card-body p-0">
            @If Model.Any() Then
                @<div class="table-responsive">
                    <table class="table table-striped table-hover align-middle mb-0">
                        <thead class="table-light">
                            <tr>
                                <th>Data</th>
                                <th>Codice Articolo</th>
                                <th>Magazzino</th>
                                <th>Giacenza</th>
                                <th>Scorta Min.</th>
                                <th>Scorta Max.</th>
                                <th>Lotto Min.</th>
                                <th>Q.tà Proposta</th>
                                <th>Origine</th>
                                <th>Stato</th>
                                <th class="text-end">Azioni</th>
                            </tr>
                        </thead>
                        <tbody>
                            @For Each item In Model
                                @<tr>
                                    <td>@item.DataGenerazione.ToString("dd/MM/yy HH:mm")</td>
                                    <td><strong>@item.CodiceArticolo</strong></td>
                                    <td>@item.CodiceMagazzino</td>
                                    <td>@item.QuantitàGiacenza</td>
                                    <td>@item.ScortaMinima</td>
                                    <td>@item.ScortaMassima</td>
                                    <td>@item.LottoMinimo</td>
                                    <td><strong>@item.QuantitàProposta</strong></td>
                                    <td><span class="badge bg-light text-dark border">@item.Origine</span></td>
                                    <td>
                                        @Select Case item.Stato
                                            Case StatoOrdineAutomatico.Proposto
                                                @<span class="badge bg-warning text-dark">Proposto</span>
                                            Case StatoOrdineAutomatico.Confermato
                                                @<span class="badge bg-success">Confermato</span>
                                            Case StatoOrdineAutomatico.Annullato
                                                @<span class="badge bg-danger">Annullato</span>
                                            Case StatoOrdineAutomatico.Inviato
                                                @<span class="badge bg-primary">Inviato</span>
                                            Case StatoOrdineAutomatico.Evaso
                                                @<span class="badge bg-secondary">Evaso</span>
                                        End Select
                                    </td>
                                    <td class="text-end">
                                        @If item.Stato = StatoOrdineAutomatico.Proposto Then
                                            @<form action="@Url.Action("Conferma", "Brighetti_OrdiniAutomatici")" method="post" style="display:inline">
                                                @Html.AntiForgeryToken()
                                                <input type="hidden" name="id" value="@item.Id" />
                                                <button type="submit" class="btn btn-sm btn-success"><i class="fa-solid fa-check"></i> Conferma</button>
                                            </form>
                                            @<form action="@Url.Action("Annulla", "Brighetti_OrdiniAutomatici")" method="post" style="display:inline">
                                                @Html.AntiForgeryToken()
                                                <input type="hidden" name="id" value="@item.Id" />
                                                <button type="submit" class="btn btn-sm btn-outline-danger"><i class="fa-solid fa-xmark"></i> Annulla</button>
                                            </form>
                                        ElseIf item.Stato = StatoOrdineAutomatico.Confermato Then
                                            @<form action="@Url.Action("Evaso", "Brighetti_OrdiniAutomatici")" method="post" style="display:inline">
                                                @Html.AntiForgeryToken()
                                                <input type="hidden" name="id" value="@item.Id" />
                                                <button type="submit" class="btn btn-sm btn-outline-secondary"><i class="fa-solid fa-box"></i> Segna evaso</button>
                                            </form>
                                        Else
                                            @<span class="text-muted">—</span>
                                        End If
                                    </td>
                                </tr>
                            Next
                        </tbody>
                    </table>
                </div>
            Else
                @<div class="text-center text-muted py-5">
                    <i class="fa-solid fa-boxes-stacked fa-2x mb-3"></i>
                    <p class="mb-0">Nessuna proposta di riordino al momento.</p>
                    <p class="mb-0">Premi "Genera proposte ora" oppure attendi la generazione pianificata.</p>
                </div>
            End If
        </div>
    </div>
</div>
