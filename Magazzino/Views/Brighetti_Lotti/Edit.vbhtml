@ModelType Brighetti.Brighetti_Lotti_Edit_ViewModel

@Using (Html.BeginForm("Edit", "Brighetti_Lotti", Nothing, FormMethod.Post, New With {.class = "ModalForm"}))
    @Html.AntiForgeryToken()
    @<ul class="nav nav-pills mb-3" id="pills-tab" role="tablist">
        <li class="nav-item" role="presentation">
            <button class="nav-link active" id="pills-anagrafica-tab" data-bs-toggle="pill" data-bs-target="#pills-anagrafica" type="button" role="tab" aria-controls="pills-anagrafica" aria-selected="true">Anagrafica</button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link" id="pills-articoli-tab" data-bs-toggle="pill" data-bs-target="#pills-articoli" type="button" role="tab" aria-controls="pills-articoli" aria-selected="false">Articoli</button>
        </li>
    </ul>
    @<div Class="tab-content" id="pills-tabContent">
         <div Class="tab-pane fade show active" id="pills-anagrafica" role="tabpanel" aria-labelledby="pills-anagrafica-tab">
             <div class="form-horizontal">
                 @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
                 @Html.HiddenFor(Function(model) model.IdLotto)

                 <div class="row">
                     <div class="col-md-6">
                         @Html.LabelFor(Function(model) model.NomeLotto, htmlAttributes:=New With {.class = "control-label col-md-12"})
                         @Html.EditorFor(Function(model) model.NomeLotto, New With {.htmlAttributes = New With {.class = "form-control"}})
                         @Html.ValidationMessageFor(Function(model) model.NomeLotto, "", New With {.class = "text-danger"})
                     </div>
                     <div class="col-md-6">
                         @Html.LabelFor(Function(model) model.Fornitore, htmlAttributes:=New With {.class = "control-label col-md-12"})
                         @Html.EditorFor(Function(model) model.Fornitore, New With {.htmlAttributes = New With {.class = "form-control"}})
                         @Html.ValidationMessageFor(Function(model) model.Fornitore, "", New With {.class = "text-danger"})
                     </div>
                 </div>
                 <div class="row">
                     <div class="col-md-12">
                         @Html.LabelFor(Function(model) model.StatoLotto, htmlAttributes:=New With {.class = "control-label col-md-12"})
                         @Html.EnumDropDownListFor(Function(model) model.StatoLotto, New With {.class = "form-control", .rows = "6"})
                         @Html.ValidationMessageFor(Function(model) model.StatoLotto, "", New With {.class = "text-danger"})
                     </div>
                 </div>
                 <div class="row">
                     <div class="col-md-12">
                         @Html.LabelFor(Function(model) model.DescrizioneLotto, htmlAttributes:=New With {.class = "control-label col-md-12"})
                         @Html.TextAreaFor(Function(model) model.DescrizioneLotto, New With {.class = "form-control", .rows = "6"})
                         @Html.ValidationMessageFor(Function(model) model.DescrizioneLotto, "", New With {.class = "text-danger"})
                     </div>
                 </div>
             </div>
         </div>
         <div Class="tab-pane fade" id="pills-articoli" role="tabpanel" aria-labelledby="pills-articoli-tab">
             <div class="row">
                 <div class="col-md-3">
                     Cod. Art.
                 </div>
                 <div class="col-md-3">
                     Q.tà
                 </div>
                 <div class="col-md-3">
                     Stato
                 </div>
                 <div class="col-md-3">
                     Note
                 </div>
             </div>
             @For Each art In Model.ListaArticoli
                 @<div class="row mb-1">
                      <div class="col-md-3">
                          @Html.EditorFor(Function(model) art.NomeArticolo, New With {.htmlAttributes = New With {.class = "form-control", .id = Convert.ToString(art.IdLottoArticolo) + "_NomeArticolo", .Name = Convert.ToString(art.IdLottoArticolo) + "_NomeArticolo", .readonly = "readonly"}})
                      </div>
                     <div class="col-md-3">
                         @Html.HiddenFor(Function(model) art.IdLottoArticolo, New With {.Name = Convert.ToString(art.IdLottoArticolo) + "_id"})
                         @Html.EditorFor(Function(model) art.QuantitàArticolo, New With {.htmlAttributes = New With {.class = "form-control", .id = Convert.ToString(art.IdLottoArticolo) + "_qta", .Name = Convert.ToString(art.IdLottoArticolo) + "_qta"}})
                     </div>
                     <div class="col-md-3">
                         @Html.EnumDropDownListFor(Function(model) art.StatoArticoloLotto, New With {.class = "form-select", .id = Convert.ToString(art.IdLottoArticolo) + "_Stato", .Name = Convert.ToString(art.IdLottoArticolo) + "_stato"})
                     </div>
                     <div class="col-md-3">
                         @Html.EditorFor(Function(model) art.NoteArticolo, New With {.htmlAttributes = New With {.class = "form-control", .id = Convert.ToString(art.IdLottoArticolo) + "_nota", .Name = Convert.ToString(art.IdLottoArticolo) + "_nota"}})

                     </div>
                 </div>
             Next

         </div>
    </div>

End Using
