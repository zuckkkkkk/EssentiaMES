@ModelType Brighetti.Brighetti_Procedura
@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
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
    @Using (Html.BeginForm())
        @Html.AntiForgeryToken()

        @<div class="form-actions no-color">
            <input type="submit" value="Delete" class="btn btn-default" /> |
            @Html.ActionLink("Back to List", "Index")
        </div>
    End Using
</div>
