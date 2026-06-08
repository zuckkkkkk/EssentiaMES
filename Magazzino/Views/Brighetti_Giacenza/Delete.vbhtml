@ModelType Brighetti.Brighetti_Giacenza
@Code
    ViewData("Title") = "Delete"
End Code
@Using (Html.BeginForm("DeleteConfirmed", "Brighetti_Giacenza", Nothing, FormMethod.Post, New With {.class = "ModalForm"}))
    @Html.AntiForgeryToken()
    @<div>
    @Html.HiddenFor(Function(x) x.Id)

    <dl Class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.CodiceArticolo)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.CodiceArticolo)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.CodiceMagazzino)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.CodiceMagazzino)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.QuantitàGiacenza)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.QuantitàGiacenza)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.QuantitàSottoscorta)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.QuantitàSottoscorta)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.UltimaModifica)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.UltimaModifica)
        </dd>

    </dl>
</div>
End Using
