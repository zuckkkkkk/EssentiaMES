@ModelType Brighetti.Brighetti_Lotti
<div>
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.NomeLotto)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.NomeLotto)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.DescrizioneLotto)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.DescrizioneLotto)
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
