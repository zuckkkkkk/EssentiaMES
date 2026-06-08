@ModelType Brighetti.Brighetti_Macchina_ViewModel
@Code
    ViewData("Title") = "Details"
End Code
<ul class="nav nav-pills mb-3" id="pills-tab" role="tablist">
    <li class="nav-item" role="presentation">
        <button class="nav-link active" id="pills-home-tab" data-bs-toggle="pill" data-bs-target="#pills-home" type="button" role="tab" aria-controls="pills-home" aria-selected="true">Dettagli</button>
    </li>
    <li class="nav-item" role="presentation">
        <button class="nav-link" id="pills-note-tab" data-bs-toggle="pill" data-bs-target="#pills-note" type="button" role="tab" aria-controls="pills-profile" aria-selected="false">Note</button>
    </li>
    <li class="nav-item" role="presentation">
        <button class="nav-link" id="pills-documenti-tab" data-bs-toggle="pill" data-bs-target="#pills-documenti" type="button" role="tab" aria-controls="pills-contact" aria-selected="false">Documenti</button>
    </li>
</ul>
<div class="tab-content" id="pills-tabContent">
    <div class="tab-pane fade show active" id="pills-home" role="tabpanel" aria-labelledby="pills-home-tab">
        <dl class="dl-horizontal">
                <dt>
                    @Html.DisplayNameFor(Function(model) model.NomeMacchina)
                </dt>

                <dd>
                    @Html.DisplayFor(Function(model) model.NomeMacchina)
                </dd>

                <dt>
                    @Html.DisplayNameFor(Function(model) model.DescrizioneMacchina)
                </dt>

                <dd>
                    @Html.DisplayFor(Function(model) model.DescrizioneMacchina)
                </dd>

                <dt>
                    @Html.DisplayNameFor(Function(model) model.IdReparto)
                </dt>

                <dd>
                    @Html.DisplayFor(Function(model) model.IdReparto)
                </dd>

                <dt>
                    @Html.DisplayNameFor(Function(model) model.IdUtente)
                </dt>

                <dd>
                    @Html.DisplayFor(Function(model) model.IdUtente)
                </dd>

                <dt>
                    @Html.DisplayNameFor(Function(model) model.UltimaModifica)
                </dt>

                <dd>
                    @Html.DisplayFor(Function(model) model.UltimaModifica)
                </dd>
        </dl>
    </div>
    <div class="tab-pane fade" id="pills-note" role="tabpanel" aria-labelledby="pills-note-tab">
        @For Each n In Model.ListaNote
            @<div class="card mb-2">
                <div class="card-body">
                    @n.Contenuto_Nota
                </div>
                <div class="card-footer">
                    @n.Data_Nota - @n.Operatore_Nome
                </div>
            </div>
        Next
    </div>
    <div Class="tab-pane fade" id="pills-documenti" role="tabpanel" aria-labelledby="pills-documenti-tab">
        @For Each n In Model.ListaDocumenti
            @<div class="card mb-2">
                <div class="card-body">
                    <a href="@Url.Action("DownloadFile", "Home", New With {.id = n.Id})">@n.Nome_File</a>
                </div>
                <div class="card-footer">
                    @n.DataCreazioneFile - @n.Operatore_Nome
                </div>
            </div>
        Next
    </div>
</div>
