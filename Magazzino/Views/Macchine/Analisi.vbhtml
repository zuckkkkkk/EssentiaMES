@ModelType Brighetti.AnalisiViewModel

<script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/3.7.0/chart.js"></script>
<script src="https://cdn.jsdelivr.net/npm/chartjs-adapter-date-fns/dist/chartjs-adapter-date-fns.bundle.min.js"></script>


<div class="row MainTitleAccount">
    <div class="col-6">
        <h1 class="TitleAccount">Macchina "@Model.NomeMacchina"</h1>
        <h4>Analisi </h4>
    </div>
    <div class="col-6">
        <div class="row">
            <div class="col-4">
                <input autocomplete="off" class="form-control " id="StartDate" name="StartDate" placeholder="Data Inizio" type="text" value="">
            </div>
            <div class="col-4">
                <input autocomplete="off" class="form-control " id="EndDate" name="EndDate" placeholder="Data Fine" type="text" value="">
            </div>
            <div class="col-4">
                <button class="BtnAggiungiModal  btn  w-auto" style="width:90%; color: white;" id="SearchDati">Aggiorna dati</button>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-6">
        @Html.HiddenFor(Function(model) model.IdMacchina)
        <div class="card cardAnalisi" style="overflow: hidden!important;">
            <div class="card-header">
                Grafico Efficienza
            </div>
            <canvas id="Eff" style="padding-bottom: 16px;"></canvas>
        </div>
    </div>
    <div class="col-md-6">
        <div class="card cardAnalisi" style="height:100%; overflow-y: scroll">
            <div class="card-header title-card-fixed">
                Lista Produzione
            </div>
            <table class="table" id="mainDataTableProduzione" style="margin-bottom: 2rem!important;">
                <thead class="header-table-fixed">
                    <tr>
                        <th>
                            Articolo
                        </th>
                        <th>
                            Quantità Prodotta
                        </th>
                        <th>
                            Quantità Scartata
                        </th>
                        <th>
                            Tempo Prod.
                        </th>
                    </tr>
                </thead>
                <tbody>
                    @For Each item In Model.ListaProduzione
                        @<tr>
                            <td>
                                @Html.DisplayFor(Function(modelItem) item.Articolo)
                            </td>
                            <td>
                                @Html.DisplayFor(Function(modelItem) item.QuantitàProdotta) pz
                            </td>
                            <td>
                                @Html.DisplayFor(Function(modelItem) item.QuantitàScartata) pz
                            </td>
                            <td>
                                @Html.DisplayFor(Function(modelItem) item.TempoDiProduzione) minuti
                            </td>
                        </tr>
                    Next
                </tbody>
            </table>
        </div>
    </div>
</div>

<div Class="row mt-3">
    <div Class="col-md-6">
        <div class="card cardAnalisi" style="height:100%; overflow-y: scroll">
            <div class="card-header title-card-fixed">
                Attività in previsione
            </div>
            <table class="table" id="mainDataTablePrevisione" style="margin-bottom: 2rem!important;">
                <thead class="header-table-fixed">
                    <tr>
                        <th>
                            Articolo
                        </th>
                        <th>
                            Quantità Da Produrre
                        </th>
                        <th>
                            Data Creazione
                        </th>
                    </tr>
                </thead>
                <tbody>
                    @For Each item In Model.ListaPrevisione
                        @<tr>
                            <td>
                                @Html.DisplayFor(Function(modelItem) item.Articolo)
                            </td>
                            <td>
                                @Html.DisplayFor(Function(modelItem) item.qtaProdurre)
                            </td>
                            <td>
                                @Html.DisplayFor(Function(modelItem) item.DataCreazione)
                            </td>
                        </tr>
                    Next
                </tbody>
            </table>
        </div>
    </div>
    <div Class="col-md-6">
        <div class="row">
            <div class="col-md-4">
                <div class="card cardAnalisi" style="height:100%; overflow-y: scroll">
                    <div class="card-header text-center">
                        Coda di Produzione
                    </div>
                    <canvas id="gauge"></canvas>
                    <div class="card-footer text-center">
                        @Model.GaugeCoda in coda
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card cardAnalisi" style="height:100%; overflow-y: scroll">
                    <div class="card-header text-center">
                        Produzione Totale
                    </div>
                    <canvas id="gauge2"></canvas>
                    <div class="card-footer text-center">
                        @Model.GaugeProdotti pz prodotti
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card cardAnalisi" style="height:100%; overflow-y: scroll">
                    <div class="card-header text-center">
                        Scarto Totale
                    </div>
                    <canvas id="gauge3"></canvas>
                    <div class="card-footer text-center">
                        @Model.GaugeScartati pz scartati
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<script defer>
    var par = '@Model.ListaEff'
    var data = JSON.parse(par.replace(/&quot;/g, '"'));
    if (data != "[]") {
        var labels = data.map(function (e) {
            return e.Data;
        });
        var data1 = data.map(function (e) {
            return e.PercEfficienza;
        });
        var data2 = data.map(function (e) {
            return e.PercTotale;
        });
        var weeklyHistoryChartEl = document.getElementById("Eff").getContext("2d");
        var weeklyHistoryChart = new Chart(weeklyHistoryChartEl, {
            type: "bar",
            data: {
                labels: labels,
                datasets: [{
                    label: 'In attività',
                    data: data1,
                    backgroundColor: [
                        'rgba(46, 191, 0, 0.79)',
                        'rgba(46, 191, 0, 0.79)',
                        'rgba(46, 191, 0, 0.79)',
                        'rgba(46, 191, 0, 0.79)',
                        'rgba(46, 191, 0, 0.79)',
                        'rgba(46, 191, 0, 0.79)'
                    ],
                    borderColor: [
                        'rgba(46, 191, 0, 0.79)',
                        'rgba(46, 191, 0, 0.79)',
                        'rgba(46, 191, 0, 0.79)',
                        'rgba(46, 191, 0, 0.79)',
                        'rgba(46, 191, 0, 0.79)',
                        'rgba(46, 191, 0, 0.79)'
                    ],

                    borderWidth: 1
                }, {
                        label: 'In Fermo',
                        data: data2,
                    backgroundColor: [
                        'rgba(255, 35, 0, 0.79)',
                        'rgba(255, 35, 0, 0.79)',
                        'rgba(255, 35, 0, 0.79)',
                        'rgba(255, 35, 0, 0.79)',
                        'rgba(255, 35, 0, 0.79)',
                        'rgba(255, 35, 0, 0.79)'
                    ],
                    borderColor: [
                        'rgba(255, 35, 0, 0.79)',
                        'rgba(255, 35, 0, 0.79)',
                        'rgba(255, 35, 0, 0.79)',
                        'rgba(255, 35, 0, 0.79)',
                        'rgba(255, 35, 0, 0.79)',
                        'rgba(255, 35, 0, 0.79)'
                    ],
                        borderWidth: 1
                    }]
            },
            options: {
                plugins: {
                    legend: {
                        display: false
                    },
                },
                devicePixelRatio: 4,
                responsive: true,
                scales: {
                    x: {
                        stacked: true,
                        grid: {
                            display: false,
                        }
                    },
                    y: {
                        stacked: true,
                        display: false
                    }
                }
            }

        });
    }
    var gauge1 = document.getElementById("gauge").getContext("2d");
    var gaugeChart = new Chart(gauge1, {
        type: 'doughnut',
        plugins: [{
            afterDraw: chart => {
                var needleValue = chart.config.data.datasets[0].needleValue;
                var dataTotal = chart.config.data.datasets[0].data.reduce((a, b) => a + b, 0);
                var angle = Math.PI + (1 / dataTotal * needleValue * Math.PI);
                var ctx = chart.ctx;
                var cw = chart.canvas.offsetWidth;
                var ch = chart.canvas.offsetHeight;
                var cx = cw / 2;
                var cy = ch - 45;
                ctx.translate(cx, cy);
                ctx.rotate(angle);
                ctx.beginPath();
                ctx.moveTo(0, -3);
                ctx.lineTo(ch - 125, 0);
                ctx.lineTo(0, 3);
                ctx.fillStyle = 'rgb(0, 0, 0)';
                ctx.fill();
                ctx.rotate(-angle);
                ctx.translate(-cx, -cy);
                ctx.beginPath();
                ctx.arc(cx, cy, 5, 0, Math.PI * 2);
                ctx.fill();
            }
        }],
        data: {
            labels: [],
            datasets: [{
                data: [35, 35, 35],
                needleValue: @Model.GaugeCoda,
                backgroundColor: [
                    'rgba(255, 35, 0, 0.79)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(46, 191, 0, 0.79)'
                ]
            }]
        },
        options: {
            responsive: false,
            aspectRatio: 1,
            layout: {
                padding: {
                    bottom: 3,
                    left: 3,
                    right: 3
                }
            },
            rotation: -90,
            cutout: '70%',
            circumference: 180,
            legend: {
                display: false
            },
            animation: {
                animateRotate: false,
                animateScale: true
            }
        }
    });

    var gauge2 = document.getElementById("gauge2").getContext("2d");
    var gaugeChart2 = new Chart(gauge2, {
        type: 'doughnut',
        plugins: [{
            afterDraw: chart => {
                var needleValue = chart.config.data.datasets[0].needleValue;
                var dataTotal = chart.config.data.datasets[0].data.reduce((a, b) => a + b, 0);
                var angle = Math.PI + (1 / dataTotal * needleValue * Math.PI);
                var ctx = chart.ctx;
                var cw = chart.canvas.offsetWidth;
                var ch = chart.canvas.offsetHeight;
                var cx = cw / 2;
                var cy = ch - 45;
                ctx.translate(cx, cy);
                ctx.rotate(angle);
                ctx.beginPath();
                ctx.moveTo(0, -3);
                ctx.lineTo(ch - 125, 0);
                ctx.lineTo(0, 3);
                ctx.fillStyle = 'rgb(0, 0, 0)';
                ctx.fill();
                ctx.rotate(-angle);
                ctx.translate(-cx, -cy);
                ctx.beginPath();
                ctx.arc(cx, cy, 5, 0, Math.PI * 2);
                ctx.fill();
            }
        }],
        data: {
            labels: [],
            datasets: [{
                data: [35, 35, 35],
                needleValue: @Model.GaugeProdotti,
                backgroundColor: [
                    'rgba(255, 35, 0, 0.79)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(46, 191, 0, 0.79)'
                ]
            }]
        },
        options: {
            responsive: false,
            aspectRatio: 1,
            layout: {
                padding: {
                    bottom: 3,
                    left: 3,
                    right: 3
                }
            },
            rotation: -90,
            cutout: '70%',
            circumference: 180,
            legend: {
                display: false
            },
            animation: {
                animateRotate: false,
                animateScale: true
            }
        }
    });
    var gauge3 = document.getElementById("gauge3").getContext("2d");
    var gaugeChart3 = new Chart(gauge3, {
        type: 'doughnut',
        plugins: [{
            afterDraw: chart => {
                var needleValue = chart.config.data.datasets[0].needleValue;
                var dataTotal = chart.config.data.datasets[0].data.reduce((a, b) => a + b, 0);
                var angle = Math.PI + (1 / dataTotal * needleValue * Math.PI);
                var ctx = chart.ctx;
                var cw = chart.canvas.offsetWidth;
                var ch = chart.canvas.offsetHeight;
                var cx = cw / 2;
                var cy = ch - 45;
                ctx.translate(cx, cy);
                ctx.rotate(angle);
                ctx.beginPath();
                ctx.moveTo(0, -3);
                ctx.lineTo(ch - 125, 0);
                ctx.lineTo(0, 3);
                ctx.fillStyle = 'rgb(0, 0, 0)';
                ctx.fill();
                ctx.rotate(-angle);
                ctx.translate(-cx, -cy);
                ctx.beginPath();
                ctx.arc(cx, cy, 5, 0, Math.PI * 2);
                ctx.fill();
            }
        }],
        data: {
            labels: [],
            datasets: [{
                data: [35, 35, 35],
                needleValue: 100-@Model.GaugeScartati,
                backgroundColor: [
                    'rgba(255, 35, 0, 0.79)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(46, 191, 0, 0.79)'
                ]
            }]
        },
        options: {
            responsive: false,
            aspectRatio: 1,
            layout: {
                padding: {
                    bottom: 3,
                    left: 3,
                    right: 3
                }
            },
            rotation: -90,
            cutout: '70%',
            circumference: 180,
            legend: {
                display: false
            },
            animation: {
                animateRotate: false,
                animateScale: true
            }
        }
    });
   
</script>
<script defer>
    var ds = datepicker("#StartDate", {
        onSelect: instance => {
            // Show which date was selected.
            console.log(instance.dateSelected)
        },
        id: 1,
        formatter: (input, date, instance) => {
            // This will display the date as `1/1/2019`.
            var arr = date.toString().split(" ");
            switch (arr[1]) {
                case 'Jan':
                    arr[1] = "01"
                    break;
                case 'Feb':
                    arr[1] = "02"
                    break;
                case 'Mar':
                    arr[1] = "03"
                    break;
                case 'Apr':
                    arr[1] = "04"
                    break;
                case 'May':
                    arr[1] = "05"
                    break;
                case 'Jun':
                    arr[1] = "06"
                    break;
                case 'Jul':
                    arr[1] = "07"
                    break;
                case 'Aug':
                    arr[1] = "08"
                    break;
                case 'Sep':
                    arr[1] = "09"
                    break;
                case 'Oct':
                    arr[1] = "10"
                    break;
                case 'Nov':
                    arr[1] = "11"
                    break;
                case 'Dec':
                    arr[1] = "12"
                    break;

                default:
                    console.log(`Sorry, we are out of ${expr}.`);
            }

            input.value = arr[3] + "" + arr[1] + "" + arr[2]
        },
        startDay: 1,
        customDays: ['Dom', 'Lun', 'Mar', 'Mer', 'Gio', 'Ven', 'Sab'],
        customMonths: ['Gennaio', 'Febbraio', 'Marzo', 'Aprile', 'Maggio', 'Giugno', 'Luglio', 'Agosto', 'Settembre', 'Ottobre', 'Novembre', 'Dicembre']
    });
    var es = datepicker("#EndDate", {
        id: 2,
        formatter: (input, date, instance) => {
            // This will display the date as `1/1/2019`.
            var arr = date.toString().split(" ");
            switch (arr[1]) {
                case 'Jan':
                    arr[1] = "01"
                    break;
                case 'Feb':
                    arr[1] = "02"
                    break;
                case 'Mar':
                    arr[1] = "03"
                    break;
                case 'Apr':
                    arr[1] = "04"
                    break;
                case 'May':
                    arr[1] = "05"
                    break;
                case 'Jun':
                    arr[1] = "06"
                    break;
                case 'Jul':
                    arr[1] = "07"
                    break;
                case 'Aug':
                    arr[1] = "08"
                    break;
                case 'Sep':
                    arr[1] = "09"
                    break;
                case 'Oct':
                    arr[1] = "10"
                    break;
                case 'Nov':
                    arr[1] = "11"
                    break;
                case 'Dec':
                    arr[1] = "12"
                    break;

                default:
                    console.log(`Sorry, we are out of ${expr}.`);
            }

            input.value = arr[3] + "" + arr[1] + "" + arr[2]
        },
        startDay: 31,
        customDays: ['Dom', 'Lun', 'Mar', 'Mer', 'Gio', 'Ven', 'Sab'],
        customMonths: ['Gennaio', 'Febbraio', 'Marzo', 'Aprile', 'Maggio', 'Giugno', 'Luglio', 'Agosto', 'Settembre', 'Ottobre', 'Novembre', 'Dicembre']
    });
</script>

