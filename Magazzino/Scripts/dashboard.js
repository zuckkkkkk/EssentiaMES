//========================================================================== START ADMINDASHBOARD
$(document).ready(function () {
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    })
    //Seleziona di default visualizza
    document.getElementById("visualizzaButton").click();
})
//Mostra popover

function ajaxAddPinPoint(x, y) {
    return new Promise((resolve, reject) => {
        try {
            $.ajax({
                type: 'POST',
                url: "/Home/ListaMacchine",
                success: function (data) {
                    if (data.ok) {
                        var inputOptions = {};
                        data.listaMacchine.forEach(function (m) {
                            inputOptions[m.Value] = m.Text;
                        });

                        Swal.fire({
                            title: 'Nuovo PinPoint',
                            input: 'select',
                            inputOptions: inputOptions,
                            icon: 'question',
                            text: "Scegliere una tra le seguenti macchine disponibili:",
                            showCancelButton: false,
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'Perfetto!'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                $.ajax({
                                    type: 'POST',
                                    url: "/Home/SalvaPinpoint",
                                    data: { id: result.value, posX: x.toString(), posY: y.toString() },
                                    dataType: "json",
                                    success: function (response) {
                                        if (response.ok) {
                                            $.notify({ message: response.message }, { type: 'success' });
                                            resolve(result.value); //restituisce idMacchina
                                        }
                                    },
                                    error: function (xhr) {
                                        console.log(xhr.responseText)
                                    }
                                });
                            }
                        })
                    }
                }
            });
        } catch (error) {
            console.error("Errore durante l'inserimento del nome macchina:", error);
            return ""; // Gestione degli errori
        }
    });
}

function getDataPinPointAjax(id) {
    return new Promise((resolve, reject) => {
        try {
            $.ajax({
                type: 'POST',
                url: "/Home/getDataPinPoint",
                data: { id: id },
                success: function (data) {
                    if (data.ok) {
                        resolve(data);
                    } else {
                        reject("Errore nella risposta del server getDataPinPointAjax");
                    }
                },
                error: function (error) {
                    reject("Errore durante la chiamata AJAX getDataPinPointAjax");
                }
            });
        } catch (error) {
            reject("Errore durante getDataPinPointAjax:" + error);
        }
    });
}

function getDatiMacchinaAjax(id) {
    return new Promise((resolve, reject) => {
        try {
            $.ajax({
                type: 'POST',
                url: "/Macchine/getDatiMacchina",
                data: { id: id },
                success: function (data) {
                    if (data.ok) {
                        resolve(data);
                    } else {
                        reject("Errore nella risposta del server getDatiMacchinaAjax");
                    }
                },
                error: function (error) {
                    reject("Errore durante la chiamata AJAX getDatiMacchinaAjax");
                }
            });
        } catch (error) {
            reject("Errore durante getDatiMacchinaAjax:" + error);
        }
    });
}

async function createPinPoint() {
    // Posizione del pinpoint
    var div = event.target;
    var offset = div.getBoundingClientRect();
    var partialDistanceX = event.clientX - offset.x
    var partialDistanceY = event.clientY - offset.y
    var leftPerc = (partialDistanceX * 100) / div.clientWidth
    var topPerc = (partialDistanceY * 100) / div.clientHeight

    var id = await ajaxAddPinPoint(leftPerc, topPerc);

    try {
        var datiPinPoint = await getDataPinPointAjax(id);

        if (datiPinPoint) {
            var image = new Image();
            image.src = "/Content/assets/PinpointImage.png";
            image.setAttribute("data-bs-toggle", "popover");
            image.setAttribute("data-bs-placement", "top");
            image.setAttribute("data-bs-html", "true");
            image.setAttribute("id", id);
            image.classList.add("classPinPoint");
            
            image.setAttribute("title", id.title);
            image.style.position = "absolute";
            image.style.width = "50px";
            image.style.height = "50px";
            image.style.left = leftPerc + "%";
            image.style.top = topPerc + "%";

            image.title = datiPinPoint.descMacchina.DescrizioneMacchina

            //Ricerca nodo in cui attaccare l'elemento
            document.getElementsByClassName("planimetriaCenter")[0].appendChild(image);
            // Inizializza il popover dopo aver creato il pinpoint
            var popoverTrigger = new bootstrap.Popover(image, {
                html: true,
                container: "body",
                content: function () {
                    return $(this).data("bs-content");
                }
            });
        }

        //Reload pagina
        window.location.reload();

    } catch (error) {
        console.error("Errore durante la creazione del pinpoint:", error);
    }
}
function closeOpenPopovers() {
    var openPopovers = document.querySelectorAll(".popover.show");
    openPopovers.forEach(function (popover) {
        var popoverInstance = bootstrap.Popover.getInstance(popover);
        if (popoverInstance) {
            popoverInstance.hide();
        }
    });
}
function removePinpointFromDb(pinpointId) {
    $.ajax({
        type: 'POST',
        url: '/Home/deletePinPoint',
        data: { id: pinpointId },
        success: function (response) {
            console.log("Record rimosso con successo");
        },
        error: function (error) {
            console.error("Errore durante la rimozione del record:", error);
        }
    });

    window.location.reload();
}
function buttonSelect(mode) {
    switch (mode) {
        case 'visualizza':

            //Chiudi i popover aperti
            closeOpenPopovers();

            //Colora bottoni
            var visualizzaButton = document.getElementById("visualizzaButton");
            visualizzaButton.classList.add("buttonSelected");

            var modificaButton = document.getElementById("modificaButton");
            modificaButton.classList.remove("buttonSelected");


            // Disattiva .clickable
            $(document).off("click", ".clickable");


            var elmnts = document.querySelectorAll(".classPinPoint");
            elmnts.forEach(async function (element) {
                var datiMacchina = await getDatiMacchinaAjax(element.id);
                if (datiMacchina.programmaDesc != "") {
                    element.setAttribute(
                        "data-bs-content",
                        "<p>Programma Desc:" + datiMacchina.programmaDesc + "<br>Data Rilevazione:" + datiMacchina.dataRilevazione + "</p> <hr><a class='link' href='/Macchine/Istanza/" + element.id + "'> <i class='fa-solid fa-search fa-xl'></i> </a>"
                    );
                } else {
                    element.setAttribute(
                        "data-bs-content",
                        "<p>Nessun dato macchina</p> <hr><a class='link' href='/Macchine/Istanza/" + element.id + "'> <i class='fa-solid fa-search fa-xl'></i> </a>"
                    );
                }
                
            });
            break;

        case 'modifica':

            //Chiudi i popover aperti
            closeOpenPopovers();

            //Colora bottoni
            var visualizzaButton = document.getElementById("visualizzaButton");
            visualizzaButton.classList.remove("buttonSelected");

            var modificaButton = document.getElementById("modificaButton");
            modificaButton.classList.add("buttonSelected");


            // Attiva .clickable
            $(document).on("click", ".clickable", function (event) {
                event.preventDefault();
                createPinPoint();
            });

            var elmnts = document.querySelectorAll(".classPinPoint");
            elmnts.forEach(function (element) {
                element.setAttribute(
                    "data-bs-content",
                    "<i class='fa-solid fa-trash fa-xl' id='" + element.id + "'></i>"
                );
            });
            break;
    }
}
$(document).on("click", ".fa-trash", function (event) {
    event.preventDefault();
    removePinpointFromDb(event.target.id);
});