@ModelType Brighetti.Brighetti_Macchina
@Code
    ViewData("Title") = "Edit"
End Code

@Using (Html.BeginForm("Edit", "Brighetti_Macchina", Nothing, FormMethod.Post, New With {.class = "ModalForm"}))
    @Html.AntiForgeryToken()

    @<div class="form-horizontal">
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        @Html.HiddenFor(Function(model) model.IdMacchina)
        <div class="row">
            <div class="col-md-12">
            @Html.LabelFor(Function(model) model.NomeMacchina, htmlAttributes:=New With {.class = "control-label col-md-4"})
                @Html.EditorFor(Function(model) model.NomeMacchina, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.NomeMacchina, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                @Html.LabelFor(Function(model) model.DescrizioneMacchina, htmlAttributes:=New With {.class = "control-label col-md-4"})
                @Html.TextAreaFor(Function(model) model.DescrizioneMacchina, New With {.class = "form-control", .rows = "6"})
                @Html.ValidationMessageFor(Function(model) model.DescrizioneMacchina, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                @Html.LabelFor(Function(model) model.IdReparto, htmlAttributes:=New With {.class = "control-label col-md-4"})
                @Html.DropDownList("Idreparto", Nothing, htmlAttributes:=New With {.class = "form-control", .required = "required", .style = "max-width:none!important;"})
                @Html.ValidationMessageFor(Function(model) model.IdReparto, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                @Html.LabelFor(Function(model) model.IdUtente, htmlAttributes:=New With {.class = "control-label col-md-4"})
                @Html.DropDownList("IdUtente", Nothing, htmlAttributes:=New With {.class = "form-control", .required = "required", .style = "max-width:none!important;"})
                @Html.ValidationMessageFor(Function(model) model.IdUtente, "", New With {.class = "text-danger"})
            </div>
        </div>
    </div>
End Using
