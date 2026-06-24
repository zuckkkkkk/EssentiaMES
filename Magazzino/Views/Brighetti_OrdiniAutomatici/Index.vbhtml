@ModelType IEnumerable(Of Brighetti.Brighetti_OrdineAutomatico)
@Code
    ViewData("Title") = "Ordini Automatici"
End Code

<div class="container" style="margin-top:24px;">
    <h2>Proposte di riordino automatico</h2>

    <p>
        Stato automatismo:
        @If ViewBag.OrdiniAutomaticiAbilitato Then
            @<span class="badge bg-success">ATTIVO</span>
        Else
            @<span class="badge bg-secondary">SPENTO</span>
        End If
        &nbsp; Prefissi codice: <code>@(If(String.IsNullOrEmpty(ViewBag.Prefissi), "tutti", ViewBag.Prefissi))</code>
        &nbsp; <a href="@Url.Action("Index", "Brighetti_Impostazioni")">Configura nel pannello Automazioni</a>
    </p>

    @If TempData("Messaggio") IsNot Nothing Then
        @<div class="alert alert-info">@TempData("Messaggio")</div>
    End If

    @Using (Html.BeginForm("Genera", "Brighetti_OrdiniAutomatici"))
        @Html.AntiForgeryToken()
        @<button type="submit" class="btn btn-primary" style="margin-bottom:14px;">
            <i class="fa-solid fa-rotate"></i> Genera proposte ora
        </button>
    End Using

    <table class="table table-striped">
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
            <th></th>
        </tr>
        @For Each item In Model
            @<tr>
                <td>@item.DataGenerazione.ToString("dd/MM/yy HH:mm")</td>
                <td>@item.CodiceArticolo</td>
                <td>@item.CodiceMagazzino</td>
                <td>@item.QuantitàGiacenza</td>
                <td>@item.ScortaMinima</td>
                <td>@item.ScortaMassima</td>
                <td>@item.LottoMinimo</td>
                <td><strong>@item.QuantitàProposta</strong></td>
                <td>@item.Origine</td>
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
                <td>
                    @If item.Stato = StatoOrdineAutomatico.Proposto Then
                        @<form action="@Url.Action("Conferma", "Brighetti_OrdiniAutomatici")" method="post" style="display:inline">
                            @Html.AntiForgeryToken()
                            <input type="hidden" name="id" value="@item.Id" />
                            <button type="submit" class="btn btn-sm btn-success">Conferma</button>
                        </form>
                        @<form action="@Url.Action("Annulla", "Brighetti_OrdiniAutomatici")" method="post" style="display:inline">
                            @Html.AntiForgeryToken()
                            <input type="hidden" name="id" value="@item.Id" />
                            <button type="submit" class="btn btn-sm btn-outline-danger">Annulla</button>
                        </form>
                    ElseIf item.Stato = StatoOrdineAutomatico.Confermato Then
                        @<form action="@Url.Action("Evaso", "Brighetti_OrdiniAutomatici")" method="post" style="display:inline">
                            @Html.AntiForgeryToken()
                            <input type="hidden" name="id" value="@item.Id" />
                            <button type="submit" class="btn btn-sm btn-outline-secondary">Segna evaso</button>
                        </form>
                    End If
                </td>
            </tr>
        Next
    </table>
</div>
