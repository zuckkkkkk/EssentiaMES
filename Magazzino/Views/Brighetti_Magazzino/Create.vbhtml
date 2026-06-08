@ModelType Brighetti.Brighetti_Magazzino

@Using (Html.BeginForm("Create", "Brighetti_Magazzino", Nothing, FormMethod.Post, New With {.class = "ModalForm"}))
    @Html.AntiForgeryToken()

    @<div class="form-horizontal">
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        <div class="row">
            <div class="col-md-12">
                @Html.LabelFor(Function(model) model.CodiceMagazzino, htmlAttributes:=New With {.class = "control-label col-md-4"})
                @Html.EditorFor(Function(model) model.CodiceMagazzino, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.CodiceMagazzino, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                @Html.LabelFor(Function(model) model.DescrizioneMagazzino, htmlAttributes:=New With {.class = "control-label col-md-4"})
                @Html.TextAreaFor(Function(model) model.DescrizioneMagazzino, New With {.class = "form-control", .rows = "6"})
                @Html.ValidationMessageFor(Function(model) model.DescrizioneMagazzino, "", New With {.class = "text-danger"})
            </div>
        </div>
    </div>
End Using


