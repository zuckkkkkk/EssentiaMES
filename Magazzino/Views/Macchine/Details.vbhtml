@ModelType Brighetti.Macchine
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<div>
    <h4>Macchine</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.NomeMacchina)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.NomeMacchina)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.DescrizioneMacchina)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.DescrizioneMacchina)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.IndirizzoMacchina)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.IndirizzoMacchina)
        </dd>

    </dl>
</div>
<p>
    @Html.ActionLink("Edit", "Edit", New With { .id = Model.Id }) |
    @Html.ActionLink("Back to List", "Index")
</p>
