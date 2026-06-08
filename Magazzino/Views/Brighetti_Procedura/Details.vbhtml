@ModelType Brighetti.Brighetti_Procedura
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<div>
    <h4>Brighetti_Procedura</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.CodiceArticolo)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.CodiceArticolo)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.IncrementaleProcedura)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.IncrementaleProcedura)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.NomeAttività)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.NomeAttività)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.idReparto)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.idReparto)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.idMacchina)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.idMacchina)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.UltimaModifica)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.UltimaModifica)
        </dd>

    </dl>
</div>
<p>
    @Html.ActionLink("Edit", "Edit", New With { .id = Model.IdProcedura }) |
    @Html.ActionLink("Back to List", "Index")
</p>
