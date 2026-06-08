@ModelType Brighetti.CreaOdpViewModel
@Using (Html.BeginForm("CreateOdp", "Brighetti_Giacenza", Nothing, FormMethod.Post, New With {.class = "ModalForm"}))
    @Html.AntiForgeryToken()

    @<div class="form-horizontal">
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        <div class="row">
            <div class="col-md-6">
                @Html.LabelFor(Function(model) model.IdArticolo, htmlAttributes:=New With {.class = "control-label col-md-4"})
                @Html.DropDownList("IdArticolo", Nothing, htmlAttributes:=New With {.class = "form-control", .required = "required", .style = "max-width:none!important;"})
                @Html.ValidationMessageFor(Function(model) model.IdArticolo, "", New With {.class = "text-danger"})
            </div>
            <div class="col-md-6">
            @Html.LabelFor(Function(model) model.Qta, htmlAttributes:=New With {.class = "control-label col-md-6"})
                @Html.EditorFor(Function(model) model.Qta, New With {.htmlAttributes = New With {.class = "form-control", .type = "number"}})
                @Html.ValidationMessageFor(Function(model) model.Qta, "", New With {.class = "text-danger"})
            </div>
        </div>
    </div>
End Using
