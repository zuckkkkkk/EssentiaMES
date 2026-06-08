@ModelType Brighetti.Brighetti_Articolo
@Code
    ViewData("Title") = "Delete"
End Code

@Using (Html.BeginForm("DeleteConfirmed", "Brighetti_Articolo", Nothing, FormMethod.Post, New With {.class = "ModalForm"}))
    @Html.AntiForgeryToken()
    @<div>
    @Html.HiddenFor(Function(x) x.Id)
        <dl class="dl-horizontal">
            <dt>
                @Html.DisplayNameFor(Function(model) model.CodiceArticolo)
            </dt>

            <dd>
                @Html.DisplayFor(Function(model) model.CodiceArticolo)
            </dd>

            <dt>
                @Html.DisplayNameFor(Function(model) model.DescrizioneArticolo)
            </dt>

            <dd>
                @Html.DisplayFor(Function(model) model.DescrizioneArticolo)
            </dd>

            <dt>
                @Html.DisplayNameFor(Function(model) model.NoteArticolo)
            </dt>

            <dd>
                @Html.DisplayFor(Function(model) model.NoteArticolo)
            </dd>

            <dt>
                @Html.DisplayNameFor(Function(model) model.UltimaModifica.Operatore)
            </dt>

            <dd>
                @Html.DisplayFor(Function(model) model.UltimaModifica.Operatore)
            </dd>
            <dt>
                @Html.DisplayNameFor(Function(model) model.UltimaModifica.Data)
            </dt>

            <dd>
                @Html.DisplayFor(Function(model) model.UltimaModifica.Data)
            </dd>
        </dl>
    </div>


End Using