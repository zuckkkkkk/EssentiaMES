@ModelType Brighetti.Brighetti_Reparto
@Code
    ViewData("Title") = "Details"
End Code


<div>
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.NomeReparto)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.NomeReparto)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.DescrizioneReparto)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.DescrizioneReparto)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.UltimaModifica)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.UltimaModifica)
        </dd>

    </dl>
</div>
