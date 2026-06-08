@ModelType Brighetti.Brighetti_Reparto
@Code
    ViewData("Title") = "Create"
End Code


@Using (Html.BeginForm("Create", "Brighetti_Reparto", Nothing, FormMethod.Post, New With {.class = "ModalForm"}))
    @Html.AntiForgeryToken()

    @<div class="form-horizontal">
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        <div class="row">
            <div class="col-md-12">
            @Html.LabelFor(Function(model) model.NomeReparto, htmlAttributes:=New With {.class = "control-label col-md-4"})
                @Html.EditorFor(Function(model) model.NomeReparto, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.NomeReparto, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                @Html.LabelFor(Function(model) model.DescrizioneReparto, htmlAttributes:=New With {.class = "control-label col-md-4"})
                @Html.TextAreaFor(Function(model) model.DescrizioneReparto, New With {.class = "form-control", .rows = "6"})
                @Html.ValidationMessageFor(Function(model) model.DescrizioneReparto, "", New With {.class = "text-danger"})
            </div>
        </div>
    </div>
End Using
