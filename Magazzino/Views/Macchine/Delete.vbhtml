@ModelType Brighetti.Macchine
@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
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
    @Using (Html.BeginForm())
        @Html.AntiForgeryToken()

        @<div class="form-actions no-color">
            <input type="submit" value="Delete" class="btn btn-default" /> |
            @Html.ActionLink("Back to List", "Index")
        </div>
    End Using
</div>
