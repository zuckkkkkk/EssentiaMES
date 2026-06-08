@ModelType Brighetti.Brighetti_Magazzino
@Code
    ViewData("Title") = "Details"
End Code

<div>
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.CodiceMagazzino)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.CodiceMagazzino)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.DescrizioneMagazzino)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.DescrizioneMagazzino)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.UltimaModifica)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.UltimaModifica)
        </dd>

    </dl>
</div>
