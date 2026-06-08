@ModelType Brighetti.Brighetti_Reparto

@Using (Html.BeginForm("DeleteConfirmed", "Brighetti_Reparto", Nothing, FormMethod.Post, New With {.class = "ModalForm"}))
    @Html.AntiForgeryToken()
    @Html.HiddenFor(Function(model) model.IdReparto)
    @<div>
        <dl Class="dl-horizontal">
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
End Using
