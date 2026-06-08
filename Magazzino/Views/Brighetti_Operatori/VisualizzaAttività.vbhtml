@ModelType Brighetti.Brighetti_Attività

<!-- Modal -->

<div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
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
                <Button type="button" class="BtnAggiungiModal btn btn-primary Add ModalSubmit">Aggiungi</Button>
                <Button type="button" id="Send_Btn" class="btn btn-primary Send ModalSubmit">Invia</Button>
                <Button type="button" class="btn btn-danger Delete ModalSubmit">Elimina</Button>
                <Button type="button" class="btn btn-secondary SaveClose ModalSubmit">Salva e Chiudi</Button>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-12">
        <h2>Attività in corso</h2>
        <h4>
            <svg height="25" width="30" class="blinking">
                <circle cx="15" cy="10" r="10" fill="red" />
                Mmmh, pare che il tuo browser non supporti gli SBG inline.
            </svg>
            @Select Case Model.StatoAttività
                Case TipoStatoAttività.In_Attrezzaggio
                    @Html.Raw("In Attrezzaggio")
                Case TipoStatoAttività.In_Lavorazione
                    @Html.Raw("In Lavorazione")
            End Select
        </h4>
    </div>
</div>
<div Class="form-horizontal">
    @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
    @Html.HiddenFor(Function(model) model.IdAttività)
    @Select Case Model.StatoAttività
        Case TipoStatoAttività.In_Attrezzaggio
            @<p style="text-align: center;"> Attrezzaggio dell'attività "<b>@Model.NomeAttività</b>" In corso. Vuoi concludere l'attività'? </p>
        Case TipoStatoAttività.In_Lavorazione
            @<p style="text-align: center;"> Attività "<b>@Model.NomeAttività</b>" In corso. Vuoi concludere l'attività'? </p>
    End Select
    <p style="text-align: center;"> Tempo trascorso dall'inizio dell'attività:</p>
    <div Class="row">
        <div Class="col-12">
            <p id="timer" Class="centerDiv"></p>
        </div>
    </div>
    <div Class="row">
        <div Class="col-6 centerDiv">
            <Button data-type="sospendi_attivita" data-value="@Model.IdAttività" Class="BtnConcludiAttività  btn  w-auto" style="color:white;" data-bs-toggle="modal" data-bs-target="#exampleModal">
                Sospendi attività
            </Button>
        </div>
        <div Class="col-6 centerDiv">
            <Button data-type="concludi_attivita" data-value="@Model.IdAttività" Class="BtnSospendiAttività  btn  w-auto" style="color:white;" data-bs-toggle="modal" data-bs-target="#exampleModal">
                Concludi attività
            </Button>
        </div>
    </div>


</div>

<script>
    var countDownDate = new Date('@DateTime.ParseExact(Model.UltimaModifica.Data, "dd/MM/yyyy HH:mm:ss", Nothing).ToString("MM/dd/yyyy HH:mm:ss")').getTime();
    var x = setInterval(function () {
        var now = new Date().getTime();
        var distance = now - countDownDate;
        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);
        document.getElementById("timer").innerHTML = hours + "h " + minutes + "m " + seconds + "s ";
    }, 1000);
</script>