@ModelType Brighetti.Brighetti_Magazzino
@Code
    ViewData("Title") = "Delete"
End Code

@Using (Html.BeginForm("DeleteConfirmed", "Brighetti_Magazzino", Nothing, FormMethod.Post, New With {.class = "ModalForm"}))
    @Html.AntiForgeryToken()
    @<div>
    @Html.HiddenFor(Function(x) x.Id)
    <dl Class="dl-horizontal">
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
End Using
