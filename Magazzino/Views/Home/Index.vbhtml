@ModelType List(Of Articoli)
@Code
    ViewData("Title") = "Lista Articoli"
End Code

<div class="row Ricerca">
    <div class="col-11" style="padding: 0!important;">
        @If Not IsNothing(ViewBag.query) Then
            @<input type="text" placeholder="Cerca articolo..." value="@ViewBag.query" id="RicercaArticolo" Class="form-control InputRicerca" name="RicercaArticolo" />
        Else
            @<input type="text" placeholder="Cerca articolo..." id="RicercaArticolo" Class="form-control InputRicerca" name="RicercaArticolo" />
        End If
    </div>
    <div Class="col-1" style="padding: 0!important;">
        <btn Class="BtnRicerca btn btn-primary"><i class="fa-solid fa-magnifying-glass"></i></btn>
    </div>
</div>
<div Class="container ArticoliContainer">
    @For Each l In Model
        @<div class="row">
            <div class="col-md-12" style="display: flex; justify-content: center; align-items: center;">
                <div class="card cardArticolo">
                    <div class="row TestoArticolo">
                        <div class="col-9">
                            <h4> @l.Nome_Articolo</h4>
                            <p> @l.Descrizione_Articolo</p>
                        </div>
                        <div class="col-2">
                            <btn Class="BtnAggiungi btn btn-primary" onclick="PopupRichiestaMateriali(@l.Id)"><i class="fa-solid fa-plus"></i></btn>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    Next
</div>

@If User.IsInRole("Admin") Then
    @<div Class="BtnAggiuntaContainer">
        <Button type="button" Class="btn btnAggiunta" aria-label="" style="display: inline-block;" onclick="PopupAggiungiArticolo()">Aggiungi Articolo</Button>
    </div>
End If