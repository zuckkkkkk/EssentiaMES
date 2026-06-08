@ModelType List(Of OrdiniInCorsoViewModel)
@Code
    ViewData("Title") = "Home Page"
End Code

<div class="row MainTitleCarrello">
    <div class="col-12">
        <h1 class="TitleCarrello">Carrello</h1>
    </div>
</div>
<div class="container ArticoliContainer add-bottom-space">
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
End If
