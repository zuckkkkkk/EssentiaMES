@ModelType Brighetti.Brighetti_Macchina

@Using (Html.BeginForm("DeleteConfirmed", "Brighetti_Macchina", Nothing, FormMethod.Post, New With {.class = "ModalForm"}))
    @Html.AntiForgeryToken()
    @Html.HiddenFor(Function(model) model.IdMacchina)
    @<div>
        <dl Class="dl-horizontal">
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
                @Html.DisplayNameFor(Function(model) model.IdReparto)
            </dt>

            <dd>
                @Html.DisplayFor(Function(model) model.IdReparto)
            </dd>

            <dt>
                @Html.DisplayNameFor(Function(model) model.IdUtente)
            </dt>

            <dd>
                @Html.DisplayFor(Function(model) model.IdUtente)
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
