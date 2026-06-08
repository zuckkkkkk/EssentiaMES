@ModelType Brighetti.Brighetti_Attività

@Using (Html.BeginForm("ConcludiAttività", "Brighetti_Operatori", Nothing, FormMethod.Post, New With {.class = "ModalForm"}))
    @Html.AntiForgeryToken()

    @<div class="form-horizontal">
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        @Html.HiddenFor(Function(model) model.IdAttività)
        <p style="text-align: center;">Vuoi concludere la seguente attività "<b>@Model.NomeAttività</b>"? Clicca su Salva per concludere l'attività.</p>
        <p style="text-align: center;">
            Articolo in lavorazione: <b>@Model.CodiceArticolo</b>
            Obiettivo di produzione: <b>@Model.QuantitàdaProdurre</b>
            <br />
            <div class="row">
                <div class="col-6">
                    @Html.LabelFor(Function(model) model.QuantitàProdotta, htmlAttributes:=New With {.class = "control-label col-md-4"})
                    @Html.EditorFor(Function(model) model.QuantitàProdotta, New With {.htmlAttributes = New With {.class = "form-control", .type = "number", .step = ".01", .value = "0.00"}})
                    @Html.ValidationMessageFor(Function(model) model.QuantitàProdotta, "", New With {.class = "text-danger"})
                </div>
                <div class="col-6">
                    @Html.LabelFor(Function(model) model.QuantitàScartata, htmlAttributes:=New With {.class = "control-label col-md-4"})
                    @Html.EditorFor(Function(model) model.QuantitàScartata, New With {.htmlAttributes = New With {.class = "form-control", .type = "number", .step = ".01", .value = "0.00"}})
                    @Html.ValidationMessageFor(Function(model) model.QuantitàScartata, "", New With {.class = "text-danger"})
                </div>
            </div></div>
End Using

