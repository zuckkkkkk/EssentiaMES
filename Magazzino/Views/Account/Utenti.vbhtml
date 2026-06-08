@ModelType List(Of UtentiViewModel)
<div Class="container ArticoliContainer">
@For Each l In Model
    @<div class="row">
        <div class="col-md-12" style="display: flex; justify-content: center; align-items: center;">
            <div class="card cardUtenti">
                <div class="row TestoUtenti">
                    <div class="col-9">
                        <h4>@l.Username</h4>
                        <p> @l.Email</p>
                    </div>
                    <div class="col-2">
                        <btn Class="BtnMostraUtenti btn btn-primary" onclick="MostraUtenti('@l.Id')"><i class="fa-solid fa-user-gear"></i></btn>
                    </div>
                </div>
            </div>
        </div>
    </div>
Next
</div>
<div Class="BtnUtenteContainer">
    <Button type="button" Class="btn btnAggiungiUtente" aria-label="" style="display: inline-block;" onclick="CreaUtente()">Crea Utente</Button>
</div>