@ModelType Brighetti.Brighetti_Giacenza
@Using (Html.BeginForm("Edit", "Brighetti_Giacenza", Nothing, FormMethod.Post, New With {.class = "ModalForm"}))
    @Html.AntiForgeryToken()

    @<div class="form-horizontal">
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        @Html.HiddenFor(Function(model) model.Id)

        <div class="row">
            <div class="col-md-6">
                @Html.LabelFor(Function(model) model.CodiceArticolo, htmlAttributes:=New With {.class = "control-label col-md-4"})
                @Html.DropDownList("CodiceArticolo", Nothing, htmlAttributes:=New With {.class = "form-control", .required = "required", .style = "max-width:none!important;"})
                @Html.ValidationMessageFor(Function(model) model.CodiceArticolo, "", New With {.class = "text-danger"})
            </div>
            <div class="col-md-6">
                @Html.LabelFor(Function(model) model.CodiceMagazzino, htmlAttributes:=New With {.class = "control-label col-md-6"})
                @Html.DropDownList("CodiceMagazzino", Nothing, htmlAttributes:=New With {.class = "form-control", .required = "required", .style = "max-width:none!important;"})
                @Html.ValidationMessageFor(Function(model) model.CodiceMagazzino, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="row">
            <div class="col-md-6">
                @Html.LabelFor(Function(model) model.QuantitàGiacenza, htmlAttributes:=New With {.class = "control-label col-md-4"})
                @Html.EditorFor(Function(model) model.QuantitàGiacenza, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.QuantitàGiacenza, "", New With {.class = "text-danger"})
            </div>

            <div class="col-md-6">
                @Html.LabelFor(Function(model) model.QuantitàSottoscorta, htmlAttributes:=New With {.class = "control-label col-md-7"})
                @Html.EditorFor(Function(model) model.QuantitàSottoscorta, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.QuantitàSottoscorta, "", New With {.class = "text-danger"})
            </div>
        </div>
    </div>
End Using
