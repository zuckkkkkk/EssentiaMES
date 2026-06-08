@ModelType Brighetti.AttivitaMacchineViewModel
@Code
    ViewData("Title") = "Avvio attività"
End Code

<div class="modal fade" id="exampleModal" data-bs-focus="false" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLabel">Modal title</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                ...
            </div>
            <div class="modal-footer" style="border-top: none!important;">
                <Button type="button" id="Send_Btn" class="btn btn-primary Send ModalSubmit">Invia</Button>
                <Button type="button" class="btn btn-danger Delete ModalSubmit">Elimina</Button>
                <Button type="button" class="btn btn-primary Save ModalSubmit BtnAggiungiModal">Avvia Attività</Button>
                <Button type="button" class="btn btn-secondary SaveClose ModalSubmit">Salva e Chiudi</Button>
            </div>
        </div>
    </div>
</div>
@If Not User.IsInRole("Admin") Then
    @<div Class="row">
        <div Class="col-12">
            <h2> Selezione attività</h2>
            <h5> Utente :    @Model.Utente.UserName</h5>
            <h5> Macchina :   @Model.Macchina.NomeMacchina</h5>
        </div>
    </div>
Else
    @<div class="row">
        <div class="col-12">
            <h2>Attività in corso o in previsione</h2>
        </div>
    </div>
End If
@If Model.ListaAttivita.Count = 0 And Not User.IsInRole("Admin") Then
    @<div class="MissingItemsCarrello mt-5 ">
        <i class="fa-solid fa-solid fa-face-laugh fa-6x fa-gradient"></i>
    </div>
    @<div class="MissingItemsCarrello">
        <h3>Nessuna attività!</h3>
    </div>
    @<div class="MissingItemsCarrello">
        <p>Pare che al momento tu non abbia nessuna attività assegnata. Riprova più tardi!</p>
    </div>
End If
@For Each activity In Model.ListaAttivita
    @If Not User.IsInRole("Admin")
        @<div class="row">
            <div class="col-12">
                <button data-type="aziona_attivita" Class="BtnApriAttivitàModal" data-value="@activity.IdAttività" data-bs-toggle="modal" data-bs-target="#exampleModal">
                    <div class="cardArticoloAttività">
                        <div class="card-body">
                            <div class="row" style="width:100%">
                                <div class="col-4" style="text-align: left!important">
                                    <div class="row">
                                        <div class="col-12">
                                            Attività: @activity.NomeAttività
                                            @If activity.StatoAttività = TipoStatoAttività.StandBy Then
                                                @<p>(Attività precedentemente aperta)</p>
                                            End If
                                        </div>
                                    </div>
                                    <div Class="row">
                                        <div Class="col-12">
                                            Data inserimento : @activity.UltimaModifica.Data
                                        </div>
                                    </div>
                                </div>
                                <div class="col-2 centerDiv ">
                                    <h1 class="fa-gradient">@activity.QuantitàdaProdurre</h1>
                                    in giacenza
                                </div>
                                <div class="col-2 centerDiv ">
                                    <i class="fa-solid fa-paperclip fa-3x fa-gradient"></i>
                                    @ViewBag.listaDocumenti allegati
                                </div>
                                <div class="col-2 centerDiv ">
                                    <i class="fa-regular fa-note-sticky fa-3x fa-gradient"></i>
                                    @ViewBag.listaNote note
                                </div>
                                <div class="col-2 RightDiv ">
                                    <i class="fa-solid fa-arrow-right fa-3x fa-gradient"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </button>
            </div>
        </div>
    Else
        @<div class="row">
            <div class="col-12">
                    <div class="cardArticoloAttività">
                        <div class="card-body">
                            <div class="row" style="width:100%">
                                <div class="col-4" style="text-align: left!important">
                                    <div class="row">
                                        <div class="col-12">
                                            Attività: @activity.NomeAttività
                                            @If activity.StatoAttività = TipoStatoAttività.StandBy Then
                                                @<p>(Attività precedentemente aperta)</p>
                                            End If
                                        </div>
                                    </div>
                                    <div Class="row">
                                        <div Class="col-12">
                                            Data inserimento : @activity.UltimaModifica.Data
                                        </div>
                                    </div>
                                </div>
                                <div class="col-2 centerDiv ">
                                    <h1 class="fa-gradient">@activity.QuantitàdaProdurre</h1>
                                    in giacenza
                                </div>
                                <div class="col-2 centerDiv ">
                                    <i class="fa-solid fa-paperclip fa-3x fa-gradient"></i>
                                    @ViewBag.listaDocumenti allegati
                                </div>
                                <div class="col-2 centerDiv ">
                                    <i class="fa-regular fa-note-sticky fa-3x fa-gradient"></i>
                                    @ViewBag.listaNote note
                                </div>
                            </div>
                        </div>
                    </div>
            </div>
        </div>
    End If

Next
