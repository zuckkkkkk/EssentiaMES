@ModelType Brighetti.Brighetti_Articoli_Viewmodel
@Code
    ViewData("Title") = "Details"
End Code
<ul class="nav nav-pills mb-3" id="pills-tab" role="tablist">
    <li class="nav-item" role="presentation">
        <button class="nav-link active" id="pills-home-tab" data-bs-toggle="pill" data-bs-target="#pills-home" type="button" role="tab" aria-controls="pills-home" aria-selected="true">Dettagli</button>
    </li>
    <li class="nav-item" role="presentation">
        <button class="nav-link position-relative" id="pills-note-tab" data-bs-toggle="pill" data-bs-target="#pills-note" type="button" role="tab" aria-controls="pills-profile" aria-selected="true">
            Note
            <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger" style="-webkit-text-fill-color: white !important;">
                @Model.ListaNote.Count
            </span>
        </button>
    </li>
    <li class="nav-item" role="presentation">
        <button class="nav-link position-relative" id="pills-documenti-tab" data-bs-toggle="pill" data-bs-target="#pills-documenti" type="button" role="tab" aria-controls="pills-contact" aria-selected="false">
            Documenti
            <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger" style="-webkit-text-fill-color: white !important;">
                @Model.ListaDocumenti.Count
            </span>
        </button>
    </li>
    <li class="nav-item" role="presentation">
        <button class="nav-link" id="pills-procedura-tab" data-bs-toggle="pill" data-bs-target="#pills-procedura" type="button" role="tab" aria-controls="pills-procedura" aria-selected="true">Ciclo</button>
    </li>
</ul>
<div class="tab-content" id="pills-tabContent">
    <div class="tab-pane fade show active" id="pills-home" role="tabpanel" aria-labelledby="pills-home-tab">
        <dl class="dl-horizontal">
            <dt>
                @Html.DisplayNameFor(Function(model) model.CodiceArticolo)
            </dt>

            <dd>
                @Html.DisplayFor(Function(model) model.CodiceArticolo)
            </dd>
            <dt>
                @Html.DisplayNameFor(Function(model) model.Importanza)
            </dt>
            <dd>
                @Html.DisplayFor(Function(model) model.Importanza)
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

        </dl>
    </div>
    <div class="tab-pane fade position-relative" id="pills-note" role="tabpanel" aria-labelledby="pills-note-tab">

        @For Each n In Model.ListaNote
            @<div class="card mb-2">

                <div class="card-body">
                    <div class="row">
                        <div class="col-11">
                            @n.Contenuto_Nota
                        </div>
                        <div class="col-1">
                            <i class="fa-solid fa-xmark fa-xl" style="color: #ff0000;" onclick="DeleteNota(@n.Id)"></i>
                        </div>
                    </div>
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

                    <div class="row">
                        <div class="col-11">
                            <a href="@Url.Action("DownloadFile", "Home", New With {.id = n.Id})">@n.Nome_File</a>
                        </div>
                        <div class="col-1">
                            <i class="fa-solid fa-xmark fa-xl" style="color: #ff0000;" onclick="DeleteFile(@n.Id)"></i>
                        </div>
                    </div>

                </div>

                <div class="card-footer">
                    @n.DataCreazioneFile - @n.Operatore_Nome
                </div>
            </div>
        Next
    </div>
    <div Class="tab-pane fade" id="pills-procedura" role="tabpanel" aria-labelledby="pills-procedura-tab">
        <ul id="listWithHandle" Class="list-group">
            @For Each n In Model.ListaProcedura
                @<li Class="list-group-item" value="@n.IdProcedura">
                    <i class="fa-solid fa-hand"></i>
                    @n.NomeAttività
                </li>
            Next
        </ul>
    </div>
</div>

