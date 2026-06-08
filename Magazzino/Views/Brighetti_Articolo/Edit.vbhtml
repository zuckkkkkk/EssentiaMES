@ModelType Brighetti.Brighetti_Articolo
@Code
    ViewData("Title") = "Edit"
End Code

@Using (Html.BeginForm("Edit", "Brighetti_Articolo", Nothing, FormMethod.Post, New With {.class = "ModalForm"}))
    @Html.AntiForgeryToken()

    @<div class="form-horizontal">
    @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
    <div class="row">
        <div class="col-6">
            @Html.LabelFor(Function(model) model.CodiceArticolo, htmlAttributes:=New With {.class = "control-label col-md-2"})
            @Html.EditorFor(Function(model) model.CodiceArticolo, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.CodiceArticolo, "", New With {.class = "text-danger"})
        </div>
        <div class="col-6">
            @Html.LabelFor(Function(model) model.DescrizioneArticolo, htmlAttributes:=New With {.class = "control-label col-md-2"})
            @Html.EditorFor(Function(model) model.DescrizioneArticolo, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.DescrizioneArticolo, "", New With {.class = "text-danger"})
        </div>
    </div>
    <div class="row">
        <div class="col-6">
            @Html.LabelFor(Function(model) model.Importanza, htmlAttributes:=New With {.class = "control-label col-md-12"})
            @Html.EnumDropDownListFor(Function(model) model.Importanza, New With {.class = "form-select"})
            @Html.ValidationMessageFor(Function(model) model.Importanza, "", New With {.class = "text-danger"})
        </div>
        <div class="col-6">
            @Html.LabelFor(Function(model) model.FamigliaId, htmlAttributes:=New With {.class = "control-label col-md-12"})
            @Html.DropDownList("FamigliaId", Nothing, htmlAttributes:=New With {.class = "form-select", .required = "required", .style = "max-width:none!important;"})
            @Html.ValidationMessageFor(Function(model) model.FamigliaId, "", New With {.class = "text-danger"})
        </div>
    </div>
    <div class="row">
        <div class="col-6">
            @Html.LabelFor(Function(model) model.LottoMinimo, htmlAttributes:=New With {.class = "control-label col-md-12"})
            @Html.EditorFor(Function(model) model.LottoMinimo, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.LottoMinimo, "", New With {.class = "text-danger"})
        </div>
    </div>
    <div class="row">
        <div class="col-12">
            @Html.LabelFor(Function(model) model.NoteArticolo, htmlAttributes:=New With {.class = "control-label col-md-2"})
            @Html.TextAreaFor(Function(model) model.NoteArticolo, New With {.class = "form-control", .rows = "6"})
            @Html.ValidationMessageFor(Function(model) model.NoteArticolo, "", New With {.class = "text-danger"})
        </div>
    </div>


</div>
End Using
