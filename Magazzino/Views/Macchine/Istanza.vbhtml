@ModelType Brighetti.Brighetti_Macchina_Viewmodel
@Code
    ViewData("Title") = "Index"
End Code
<script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/3.7.0/chart.js"></script>
<script src="https://cdn.jsdelivr.net/npm/chartjs-adapter-date-fns/dist/chartjs-adapter-date-fns.bundle.min.js"></script>
<div class="row MainTitleAccount">
    <div class="col-12">
        <h1 class="TitleAccount">Macchina @Model.NomeMacchina</h1>
        <h4>@Model.DescrizioneMacchina</h4>
    </div>
</div>
@If Model.ListaDatiMacchina.Count > 0 Then
    @<div class="row" style="margin-left:10px; margin-bottom:32px;">
        <div Class="col-md-6">
            Programma: @Model.ListaDatiMacchina.Last.Programma (@Model.ListaDatiMacchina.Last.ProgrammaDescrizione)<br />
            Stato: @Model.ListaDatiMacchina.Last.StatoOperativoMacchina
        </div>
        <div class="col-md-6" style="text-align: right;">
            Ultimi dati disp.: @Model.ListaDatiMacchina.Last.DataRilevazione
            <br />
            Ultimi agg.: @DateTime.Now
        </div>
    </div>
Else
    @<div class="row" style="margin-left:10px; margin-bottom:32px;">
        <div Class="col-md-6">
            Programma: - (-)<br />
            Stato: -
        </div>
        <div class="col-md-6" style="text-align: right;">
            Ultimi dati disp.: <b>Nessun dato nell'ultima settimana</b>
            <br />
            Ultimi agg.: @DateTime.Now
        </div>
    </div>
End If

<div class="row" style="margin-left: 10px;">
    <div class="col-md-6">
        <h4>Note e Documenti</h4>
        <div class="row">
            <div class="col-md-12">
                @For Each card In Model.ListaNote
                    @<div Class="card cardUtenti mb-3">
                        <div Class="row TestoUtenti">
                            <div Class="col-9">
                                <h4> Nota @card.Data_Nota</h4>
                                <p> @card.Operatore_Nome - @card.Contenuto_Nota</p>
                            </div>
                        </div>
                    </div>
                Next
                @For Each card In Model.ListaDocumenti
                    @<div Class="card cardUtenti mb-3">
                        <div Class="row TestoUtenti">
                            <div Class="col-9">
                                <h4> Documento @card.Nome_File</h4>
                                <p> @card.Operatore_Nome - @card.DataCreazioneFile</p>
                            </div>
                            <div Class="col-2">
                                <a Class="BtnMostraUtenti btn btn-primary" href="@Url.Action("DownloadFile", "Home", New With {.id = card.Id})"><i class="fa-solid fa-download"></i></a>
                            </div>
                        </div>
                    </div>
                Next
                <div Class="row">
                    <div Class="col-md-12 text-center" style=" display: flex; justify-content: center;">
                        <btn Class="BtnMostraUtenti btn btn-primary" style="width: auto !important; margin-right: 16px;" onclick="AddFile('@Model.IdMacchina', 6);" type="button" data-value="@Model.IdMacchina">Aggiungi Documento</btn>
                        <btn Class="BtnMostraUtenti btn btn-primary" style="width: auto !important; margin-right: 16px;" onclick="AddNota('@Model.IdMacchina', 6);" type="button" data-value="@Model.IdMacchina">Aggiungi Nota</btn>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div Class="col-md-6">
        <div class="row">
            <div class="col-md-12">
                <h4> Storico Macchina</h4>
                <div class="wrapper" style="width:512px; height:512px;">
                    @If Not ViewBag.DatiMacchina = "[]" Then
                        @<canvas id = "myChart" ></canvas>
                    Else
                        @<div style="display: flex; justify-content: center; align-items: center;">
                            <p>Nessun dato macchina disponibile.</p>
                        </div>
                    End If
                </div>
            </div>
        </div>
    </div>
</div>
<div class="row" style="min-height: 50vh;">
    <div class="col-md-12">
        @Html.Action("AvvioMacchina", "Brighetti_Operatori", New With {.idMacchina = Model.IdMacchina})
    </div>
</div>

<script defer>
    var par = '@ViewBag.DatiMacchina'
    var data = JSON.parse(par.replace(/&quot;/g, '"'));
    if (data != "[]") {
        var labels = data.map(function (e) {
            return e.Stato;
        });
        var data = data.map(function (e) {
            return e.Number;
        });
        var weeklyHistoryChartEl = document.getElementById("myChart").getContext("2d");
        var weeklyHistoryChart = new Chart(weeklyHistoryChartEl, {
            type: "pie",
            data: {
                labels: labels,
                datasets: [{
                    label: 'Stato Macchina',
                    data: data,
                    backgroundColor: [
                        'rgba(255, 99, 132, 1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(153, 102, 255, 1)',
                        'rgba(255, 159, 64, 1)'
                    ],
                    borderColor: [
                        'rgba(255,99,132,1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(153, 102, 255, 1)',
                        'rgba(255, 159, 64, 1)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }

        });
    }
</script>
