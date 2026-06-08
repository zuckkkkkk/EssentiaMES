@ModelType Brighetti.Brighetti_Attività

<ul class="nav nav-pills mb-3" id="pills-tab" role="tablist">
    <li class="nav-item" role="presentation">
        <button class="nav-link active" id="pills-home-tab" data-bs-toggle="pill" data-bs-target="#pills-home" type="button" role="tab" aria-controls="pills-home" aria-selected="true">Dettagli</button>
    </li>
    <li class="nav-item" role="presentation">
        <button class="nav-link position-relative" id="pills-note-tab" data-bs-toggle="pill" data-bs-target="#pills-note" type="button" role="tab" aria-controls="pills-profile" aria-selected="false">Note
            <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger" style="-webkit-text-fill-color: white !important;">
                @ViewBag.listaNote.Count
            </span>
        </button>
    </li>
    <li class="nav-item" role="presentation">
        <button class="nav-link position-relative" id="pills-documenti-tab" data-bs-toggle="pill" data-bs-target="#pills-documenti" type="button" role="tab" aria-controls="pills-contact" aria-selected="false">Documenti
            <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger" style="-webkit-text-fill-color: white !important;">
                @ViewBag.listaDocumenti.Count
            </span>
        </button>
    </li>
</ul>
<div class="tab-content" id="pills-tabContent">
    <div class="tab-pane fade show active" id="pills-home" role="tabpanel" aria-labelledby="pills-home-tab">

        @Using (Html.BeginForm("ApriAttività", "Brighetti_Operatori", Nothing, FormMethod.Post, New With {.class = "ModalForm"}))
            @Html.AntiForgeryToken()

            @<div class="form-horizontal">
    @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
    @Html.HiddenFor(Function(model) model.IdAttività)
    <p style="text-align: center;">Vuoi iniziare la seguente attività "<b>@Model.NomeAttività</b>"? <br />Clicca su "Avvia Attività" per attivare la procedura di inizio attività.</p>
    <p style="text-align: center;">
        Articolo da lavorare: <b>@Model.CodiceArticolo</b>
        <br />
        Quantità da produrre: <b>@Model.QuantitàdaProdurre</b>
    </p>
    @If Model.StatoAttività = TipoStatoAttività.StandBy Then
        @<p style="text-align: center;">Quantità prodotta precedentemente: <b>@Model.QuantitàProdotta</b></p>
        @<p style="text-align: center;">Quantità scartata precedentemente: <b>@Model.QuantitàScartata</b></p>
    End If
       
    <div class="row">
        <div class="col-md-12">
            @Html.LabelFor(Function(model) model.StatoAttività, htmlAttributes:=New With {.class = "control-label col-md-12"})
            @Html.DropDownList("StatoAttivita", Nothing, htmlAttributes:=New With {.class = "form-control", .required = "required", .style = "max-width:none!important;"})
            @Html.ValidationMessageFor(Function(model) model.StatoAttività, "", New With {.class = "text-danger"})
        </div>
    </div>
</div>
        End Using
    <div style="text-align: center !important; display: flex; justify-content: center;margin-top:8px; margin-bottom:8px;">
        <button onclick="AddNota('@Model.idMacchinaMagazzino', 6);" type="button" data-value="@Model.idMacchinaMagazzino" Class="btn btn-primary BtnAggiungiModal">
            Inserisci note <i Class="fa-solid fa-file-arrow-up"></i>
        </button>
        <Button onclick="AddFile('@Model.idMacchinaMagazzino', 6);" type="button" data-value="@Model.idMacchinaMagazzino" Class="btn btn-primary BtnAggiungiModal">
            Inserisci documenti <i Class="fa-solid fa-note-sticky"></i>
        </Button>
    </div>
    </div>
    <div class="tab-pane fade" id="pills-note" role="tabpanel" aria-labelledby="pills-note-tab">

        @For Each n In ViewBag.listaNote
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
        @For Each n In ViewBag.listaDocumenti
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
</div>


