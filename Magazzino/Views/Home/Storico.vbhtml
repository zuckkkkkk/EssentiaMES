@ModelType List(Of StoricoViewModel)
@Code
    ViewData("Title") = "Storico"
End Code

<!-- Modal -->

<div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-xl">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLabel">Modal title</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                ...
            </div>
            <div class="modal-footer" style="border-top: none!important;">
                <Button type="button" class="BtnAggiungiModal btn btn-primary Add ModalSubmit">Aggiungi</Button>
                <Button type="button" id="Send_Btn" class="btn btn-primary Send ModalSubmit">Invia</Button>
                <Button type="button" class="btn btn-danger Delete ModalSubmit">Elimina</Button>
                <Button type="button" class="btn btn-secondary SaveClose ModalSubmit">Salva e Chiudi</Button>
            </div>
        </div>
    </div>
</div>


<div class="HeaderStorico">
    <div class="row MainTitleStorico">
        <div class="col-12">
            <h1 class="TitleStorico">Storico</h1>
        </div>
    </div>
    <ul class="nav nav-pills mb-3" id="pills-tab" role="tablist">
        <li class="nav-item" role="presentation">
            <button class="nav-link active" id="pills-Storico-tab" data-bs-toggle="pill" data-bs-target="#pills-Storico" type="button" role="tab" aria-controls="pills-Storico" aria-selected="true">Storico Attività</button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link" id="pills-Account-tab" data-bs-toggle="pill" data-bs-target="#pills-Account" type="button" role="tab" aria-controls="pills-Account" aria-selected="false">Gest. Account</button>
        </li>
    </ul>
</div>

<div class="tab-content StoricoTabContent" id="pills-tabContent">
    <div class="tab-pane fade show active" id="pills-Storico" role="tabpanel" aria-labelledby="pills-Storico-tab">
        <div Class="container ArticoliContainer">
            @For Each l In Model
                @<div class="row">
                    <div class="col-md-12" style="display: flex; justify-content: center; align-items: center;">
                        <div class="card cardStorico">
                            <div class="row TestoStorico">
                                <div class="col-9">
                                    <h4> Ordine Nr. @l.Id - art: @l.CodiceArticolo</h4>
                                    <p> Effettuato in data @l.DataOrdine.ToString.Split(" ")(0)</p>
                                </div>
                                <div class="col-2">
                                    <btn Class="BtnMostraOrdine btn btn-primary" data-type="show_ordine" data-value="@l.Id" data-bs-toggle="modal" data-bs-target="#exampleModal"><i class="fa-solid fa-magnifying-glass"></i></btn>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            Next
        </div>
    </div>
    <div class="tab-pane fade" id="pills-Account" role="tabpanel" aria-labelledby="pills-Account-tab">
        @Html.Action("Utenti", "Account")
    </div>
</div>
@*<div class="container ArticoliContainer add-bottom-space">
        @If Model.Count = 0 Then
            @<div class="MissingItemsCarrello mt-5 ">
                <i class="fa-solid fa-cart-shopping fa-6x fa-gradient"></i>
            </div>
            @<div class="MissingItemsCarrello">
                <h3>Carrello vuoto</h3>
            </div>
            @<div class="MissingItemsCarrello">
                <p>Pare che il carrello sia vuoto! Inserisci prima degli articoli per poter inviare la richiesta di materiali.</p>
            </div>
        End If
        @For Each l In Model
            @<div Class="row">
                <div Class="col-md-12" style="display: flex; justify-content: center; align-items: center;">
                    <div Class="card cardArticolo">
                        <div Class="row TestoArticolo">
                            <div Class="col-9">
                                <h4> @l.NomeArticolo</h4>
                                <p> @l.DescrizioneArticolo</p>
                            </div>
                            <div Class="col-2">
                                <btn Class="BtnCancella btn btn-primary" onclick="PopupCancellaMateriali(@l.Id)"><i class="fa-solid fa-plus"></i></btn>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        Next

    </div>
    @If Model.Count = 0 Then
        @<div Class="BtnRichiestaContainer">
            <Button disabled type="button" Class="btn btnRichiestaDisabled" aria-label="" style="display: inline-block;" onclick="PopupInviaRichiesta()">Invia richiesta</Button>
        </div>
    Else
        @<div Class="BtnRichiestaContainer">
            <Button type="button" Class="btn btnRichiesta" aria-label="" style="display: inline-block;" onclick="PopupInviaRichiesta()">Invia richiesta</Button>
        </div>
    End If*@
