//================================================================================= Document Ready

$(document).ready(function () {
    //Ricerca Analisi Dati
    var endpointAnalisi = '/Macchine/Analisi?id=';
    $('body').on('click', '#SearchDati', function (e) {
        var start = $("#StartDate")[0].value;
        var end = $("#EndDate")[0].value;
        var id = $("#IdMacchina")[0].value;
        window.open(endpointAnalisi + id + "&dateTimeUser=" + start + "-" + end).focus();
    });
 
    //========================================================================== END ADMINDASHBOARD

    if (document.URL.includes("AvvioMacchina")) {
        setInterval(function () {
            if ($('.modal.in, .modal.show').length == 0) {
                cache_clear()
            }
        }, 5000);
    }
    
    $("#MainDataTableArticoli").DataTable({
        stateSave: true,
        processing: true,
        serverSide: true,
        ajax: { url: '/Brighetti_Articolo/ServerProcessing', type: 'POST' },
        "deferRender": true,
        dom: '<"row  align-items-center"<"col col-auto"f><"col"i><"col col-auto"B>>rt<"row align-items-center"<"col"p><"col col-auto">>', /*Aggiunngere la l se vuoi vedere numero*/
        buttons: [
            {
                extend: 'excel',
                text: '<i class="fas fa-download"></i>',
                filename: 'Lista Articoli •  @DateTime.Now • Essentia',
                sheetName: '@DateTime.Now'
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i>'
            }],
        columns: [
            { data: "CodiceArticolo", orderable: true, },
            { data: "DescrizioneArticolo", searchable: true },
            { data: "Importanza", searchable: true },
            { data: "FamigliaId", searchable: true },
            { data: "UltimaModifica", searchable: false },
            { data: "Azioni", searchable: false, orderable: false },
        ],
        "columnDefs": [
            {
                "targets": [0, 4],
                "searchable": true
            }
        ],
        language: {
            "decimal": ",",
            "emptyTable": "Nessun Articolo Disponibile",
            "info": "Visualizzazione da _START_ a _END_ di _TOTAL_ Articoli",
            "infoEmpty": "Visualizzazione da 0 a 0 di 0 Articoli",
            "infoFiltered": "(Filtrati su _MAX_ Articoli Totali)",
            "infoPostFix": "",
            "thousands": ".",
            "lengthMenu": "Mostra _MENU_",
            "loadingRecords": "Caricamento...",
            "processing": '<i class="fa fa-circle-notch fa-spin fa-3x fa-fw" style="color: purple;"></i><span class="sr-only">Caricamento...</span> ',
            "search": "Cerca articoli <i class='fas fa-search'></i>",
            "zeroRecords": "Nessun Articolo",
            "paginate": {
                "first": "Prima",
                "last": "Ultima",
                "next": "Prossima",
                "previous": "Precedente"
            },
            "aria": {
                "sortAscending": ": ordina in modo ascendente A-Z",
                "sortDescending": ": ordina in modo discendente Z-A"
            }
        },
    });
    $("#MainDataTableGiacenze").DataTable({
        stateSave: true,
        processing: true,
        serverSide: true,
        ajax: { url: '/Brighetti_Giacenza/ServerProcessing', type: 'POST' },
        "deferRender": true,
        dom: '<"row  align-items-center"<"col col-auto"f><"col"i><"col col-auto"B>>rt<"row align-items-center"<"col"p><"col col-auto">>',
        buttons: [
            {
                extend: 'excel',
                text: '<i class="fas fa-download"></i>',
                filename: 'Lista Giacenze •  @DateTime.Now • Essentia',
                sheetName: '@DateTime.Now'
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i>'
            }],
        columns: [
            { data: "CodiceArticolo", orderable: true, },
            { data: "CodiceMagazzino", searchable: true },
            { data: "QuantitàGiacenza", searchable: true },
            { data: "QuantitàSottoscorta", searchable: true },
            { data: "UltimaModifica", searchable: false },
            { data: "Azioni", searchable: false, orderable: false },
        ],
        "columnDefs": [
            {
                "targets": [0, 4],
                "searchable": true
            }
        ],
        language: {
            "decimal": ",",
            "emptyTable": "Nessun Dato Disponibile",
            "info": "Visualizzazione da _START_ a _END_ di _TOTAL_ Giacenze",
            "infoEmpty": "Visualizzazione da 0 a 0 di 0 Giacenze",
            "infoFiltered": "(Filtrati su _MAX_ Giacenze)",
            "infoPostFix": "",
            "thousands": ".",
            "lengthMenu": "Mostra _MENU_",
            "loadingRecords": "Caricamento...",
            "processing": '<i class="fa fa-circle-notch fa-spin fa-3x fa-fw" style="color: purple;"></i><span class="sr-only">Caricamento...</span> ',
            "search": "Cerca giacenza <i class='fas fa-search'></i>",
            "zeroRecords": "Nessuna Giacenza",
            "paginate": {
                "first": "Prima",
                "last": "Ultima",
                "next": "Prossima",
                "previous": "Precedente"
            },
            "aria": {
                "sortAscending": ": ordina in modo ascendente A-Z",
                "sortDescending": ": ordina in modo discendente Z-A"
            }
        },
    });
    $("#MainDataTableMagazzini").DataTable({
        language: {
            "decimal": ",",
            "emptyTable": "Nessun Dato Disponibile",
            "info": "Visualizzazione da _START_ a _END_ di _TOTAL_ Magazzini",
            "infoEmpty": "Visualizzazione da 0 a 0 di 0 Magazzini",
            "infoFiltered": "(Filtrati su _MAX_ Magazzini Totali)",
            "infoPostFix": "",
            "thousands": ".",
            "lengthMenu": "Mostra _MENU_",
            "loadingRecords": "Caricamento...",
            "processing": '<i class="fa fa-circle-notch fa-spin fa-3x fa-fw" style="color: purple;"></i><span class="sr-only">Caricamento...</span> ',
            "search": "Cerca Magazzini <i class='fas fa-search'></i>",
            "zeroRecords": "Nessun Magazzino",
            "paginate": {
                "first": "Prima",
                "last": "Ultima",
                "next": "Prossima",
                "previous": "Precedente"
            },
            "aria": {
                "sortAscending": ": ordina in modo ascendente A-Z",
                "sortDescending": ": ordina in modo discendente Z-A"
            }
        },
    });
    $("#MainDataTableMacchineStati").DataTable({
        language: {
            "decimal": ",",
            "emptyTable": "Nessun Dato Disponibile",
            "info": "Visualizzazione da _START_ a _END_ di _TOTAL_ Macchine",
            "infoEmpty": "Visualizzazione da 0 a 0 di 0 Macchine",
            "infoFiltered": "(Filtrati su _MAX_ Macchine Totali)",
            "infoPostFix": "",
            "thousands": ".",
            "lengthMenu": "Mostra _MENU_",
            "loadingRecords": "Caricamento...",
            "processing": '<i class="fa fa-circle-notch fa-spin fa-3x fa-fw" style="color: purple;"></i><span class="sr-only">Caricamento...</span> ',
            "search": "Cerca Macchina <i class='fas fa-search'></i>",
            "zeroRecords": "Nessuna Macchina",
            "paginate": {
                "first": "Prima",
                "last": "Ultima",
                "next": "Prossima",
                "previous": "Precedente"
            },
            "aria": {
                "sortAscending": ": ordina in modo ascendente A-Z",
                "sortDescending": ": ordina in modo discendente Z-A"
            }
        },
    });
    $("#mainDataTableProduzione").DataTable({
        dom: '<"row align-items-center"<"col col-auto"><"col"><"col col-auto"><"row align-items-center"<"col"><"col col-auto">>',
        language: {
            "decimal": ",",
            "emptyTable": "Nessun Dato Disponibile",
            "info": "Visualizzazione da _START_ a _END_ di _TOTAL_ Articoli",
            "infoEmpty": "Visualizzazione da 0 a 0 di 0 Articoli",
            "infoFiltered": "(Filtrati su _MAX_ Articoli Totali)",
            "infoPostFix": "",
            "thousands": ".",
            "lengthMenu": "Mostra _MENU_",
            "loadingRecords": "Caricamento...",
            "processing": '<i class="fa fa-circle-notch fa-spin fa-3x fa-fw" style="color: purple;"></i><span class="sr-only">Caricamento...</span> ',
            "search": "Cerca Macchina <i class='fas fa-search'></i>",
            "zeroRecords": "Nessun articolo",
            "paginate": {
                "first": "Prima",
                "last": "Ultima",
                "next": "Prossima",
                "previous": "Precedente"
            },
            "aria": {
                "sortAscending": ": ordina in modo ascendente A-Z",
                "sortDescending": ": ordina in modo discendente Z-A"
            }
        },
    });
    $("#mainDataTablePrevisione").DataTable({
        dom: '<"row align-items-center"<"col col-auto"><"col"><"col col-auto"><"row align-items-center"<"col"><"col col-auto">>',
        fixedHeader: true,
        language: {
            "decimal": ",",
            "emptyTable": "Nessun Dato Disponibile",
            "info": "Visualizzazione da _START_ a _END_ di _TOTAL_ Articoli",
            "infoEmpty": "Visualizzazione da 0 a 0 di 0 Articoli",
            "infoFiltered": "(Filtrati su _MAX_ Articoli Totali)",
            "infoPostFix": "",
            "thousands": ".",
            "lengthMenu": "Mostra _MENU_",
            "loadingRecords": "Caricamento...",
            "processing": '<i class="fa fa-circle-notch fa-spin fa-3x fa-fw" style="color: purple;"></i><span class="sr-only">Caricamento...</span> ',
            "search": "Cerca Macchina <i class='fas fa-search'></i>",
            "zeroRecords": "Nessun articolo",
            "paginate": {
                "first": "Prima",
                "last": "Ultima",
                "next": "Prossima",
                "previous": "Precedente"
            },
            "aria": {
                "sortAscending": ": ordina in modo ascendente A-Z",
                "sortDescending": ": ordina in modo discendente Z-A"
            }
        },
    });
    $("#MainDataTableLotti").DataTable({
        language: {
            "decimal": ",",
            "emptyTable": "Nessun Dato Disponibile",
            "info": "Visualizzazione da _START_ a _END_ di _TOTAL_ Lotti",
            "infoEmpty": "Visualizzazione da 0 a 0 di 0 Lotti",
            "infoFiltered": "(Filtrati su _MAX_ Lotti Totali)",
            "infoPostFix": "",
            "thousands": ".",
            "lengthMenu": "Mostra _MENU_",
            "loadingRecords": "Caricamento...",
            "processing": '<i class="fa fa-circle-notch fa-spin fa-3x fa-fw" style="color: purple;"></i><span class="sr-only">Caricamento...</span> ',
            "search": "Cerca Lotti <i class='fas fa-search'></i>",
            "zeroRecords": "Nessun Lotto",
            "paginate": {
                "first": "Prima",
                "last": "Ultima",
                "next": "Prossima",
                "previous": "Precedente"
            },
            "aria": {
                "sortAscending": ": ordina in modo ascendente A-Z",
                "sortDescending": ": ordina in modo discendente Z-A"
            }
        },
    });
   
    $("#MainDataTableReparti").DataTable({
        language: {
            "decimal": ",",
            "emptyTable": "Nessun Dato Disponibile",
            "info": "Visualizzazione da _START_ a _END_ di _TOTAL_ Reparti",
            "infoEmpty": "Visualizzazione da 0 a 0 di 0 Reparti",
            "infoFiltered": "(Filtrati su _MAX_ Reparti Totali)",
            "infoPostFix": "",
            "thousands": ".",
            "lengthMenu": "Mostra _MENU_",
            "loadingRecords": "Caricamento...",
            "processing": '<i class="fa fa-circle-notch fa-spin fa-3x fa-fw" style="color: purple;"></i><span class="sr-only">Caricamento...</span> ',
            "search": "Cerca Reparti <i class='fas fa-search'></i>",
            "zeroRecords": "Nessun Reparto",
            "paginate": {
                "first": "Prima",
                "last": "Ultima",
                "next": "Prossima",
                "previous": "Precedente"
            },
            "aria": {
                "sortAscending": ": ordina in modo ascendente A-Z",
                "sortDescending": ": ordina in modo discendente Z-A"
            }
        },
    });
    $("#MainDataTableMacchine").DataTable({
        language: {
            "decimal": ",",
            "emptyTable": "Nessun Dato Disponibile",
            "info": "Visualizzazione da _START_ a _END_ di _TOTAL_ Macchine",
            "infoEmpty": "Visualizzazione da 0 a 0 di 0 Macchine",
            "infoFiltered": "(Filtrati su _MAX_ Macchine Totali)",
            "infoPostFix": "",
            "thousands": ".",
            "lengthMenu": "Mostra _MENU_",
            "loadingRecords": "Caricamento...",
            "processing": '<i class="fa fa-circle-notch fa-spin fa-3x fa-fw" style="color: purple;"></i><span class="sr-only">Caricamento...</span> ',
            "search": "Cerca Macchine <i class='fas fa-search'></i>",
            "zeroRecords": "Nessuna Macchina",
            "paginate": {
                "first": "Prima",
                "last": "Ultima",
                "next": "Prossima",
                "previous": "Precedente"
            },
            "aria": {
                "sortAscending": ": ordina in modo ascendente A-Z",
                "sortDescending": ": ordina in modo discendente Z-A"
            }
        },
    });

    $.notifyDefaults({
        type: 'success',
        z_index: '5001',
        globalPosition: 'top right',
        allow_dismiss: false,
        style: "metro",
        animate: {
            enter: 'animated fadeInDown ',
            exit: 'animated fadeOutUp'
        }
    });
    $('.BtnRicerca').click(function () {
        var q = $('#RicercaArticolo').val()
        window.location.href = '/Home/Index?q=' + q;
    });
    $("#RicercaArticolo").on('keyup', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            var q = $('#RicercaArticolo').val()
            window.location.href = '/Home/Index?q=' + q;
        }
    });
    document.addEventListener('touchmove', function (event) {
        if (event.scale !== 1) { event.preventDefault(); }
    }, false);
    var lastTouchEnd = 0;
    document.addEventListener('touchend', function (event) {
        var now = (new Date()).getTime();
        if (now - lastTouchEnd <= 300) {
            event.preventDefault();
        }
        lastTouchEnd = now;
    }, false);
});
function cache_clear() {
    window.location.reload(true);
}
//================================================================================= Functions
function PopupInviaRichiesta() {
    Swal.fire({
        title: 'Conferma operazione',
        text: "Sei sicuro di voler inviare questa richiesta di materiali?",
        icon: 'question',
        showCancelButton: false,
        confirmButtonColor: '#3085d6',
        confirmButtonText: 'Si, invia!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'POST',
                url: "/Home/InviaRichiestaAmministratore",
                success: function (data) {
                    if (data.ok) {
                        Swal.fire({
                                title: 'Evviva!',
                                text: "La richiesta dei materiali inoltrata con sucesso.",
                                icon: 'success',
                                showCancelButton: false,
                                confirmButtonColor: '#3085d6',
                                confirmButtonText: 'Perfetto!'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                location.reload();
                            }
                        })
                    }
                }
            });
        }
    })
}
function PopupCancellaMateriali(idOrdine) {
    Swal.fire({
        title: 'Conferma operazione',
        text: "Sei sicuro di voler cancellare questo articolo dal carrello?",
        icon: 'question',
        showCancelButton: false,
        confirmButtonColor: '#3085d6',
        confirmButtonText: 'Si, cancellalo!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'POST',
                url: "/Home/CancellaArticoloInOrdine?id=" + idOrdine,
                success: function (data) {
                    if (data.ok) {
                        Swal.fire({
                            title: 'Cancellato!',
                            text: "Articolo rimosso correttamente.",
                            icon: 'success',
                            showCancelButton: false,
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'Perfetto!'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                location.reload();
                            }
                        })
                    }
                }
            });
        }
    })
}
function MostraUtenti(idUtente) {
    $.ajax({
        type: 'POST',
        url: "/Account/MostraUtente?id=" + idUtente,
        success: function (data) {
            if (data.ok) {
                Swal.fire({
                    title: 'Utente "' + data.utente.UserName+"\"",
                    text: "Vuoi cancellare l'utente dal sistema di Gestione Magazzino?",
                    icon: 'question',
                    showCancelButton: false,
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'Si, cancella!'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            type: 'POST',
                            url: "/Account/CancellaUtente?id=" + idUtente,
                            success: function (data) {
                                if (data.ok) {
                                    Swal.fire({
                                        title: 'Evviva!',
                                        text: "L'utente e' stato cancellato correttamente. Nello storico potrai sempre trovare i suoi ordini.",
                                        icon: 'success',
                                        showCancelButton: false,
                                        confirmButtonColor: '#3085d6',
                                        confirmButtonText: 'Perfetto!'
                                    }).then((result) => {
                                        if (result.isConfirmed) {
                                            location.reload();
                                        }
                                    })
                                } else {
                                    Swal.fire({
                                        title: 'Errore!',
                                        text: "Impossibile cancellare l'utente.",
                                        icon: 'success',
                                        showCancelButton: false,
                                        confirmButtonColor: '#3085d6',
                                        confirmButtonText: 'Va bene!'
                                    }).then((result) => {
                                        if (result.isConfirmed) {
                                            location.reload();
                                        }
                                    })
                                }
                            }
                        });
                    }
                })
            }
        }
    });

}
function MostraOrdine(idOrdine) {
    $.ajax({
        type: 'POST',
        url: "/Home/MostraOrdine?id=" + idOrdine,
        success: function (data) {
            if (data.ok) {
                Swal.fire({
                    title: 'Dettagli Ordine Nr. ' + data.ordineNr + '('+data.codiceArticolo+')',
                    html: '<div class="row" style="margin-top:16px;">                                                                                                  ' +
                        '    <div class="col-12 nopadding">' +
                        data.contenuto +
                        '    </div>                                                                                                                                     ' +
                        '</div>         ' +
                        '<div class="row" style="margin-bottom:16px;margin-top:16px;">                                                                                                  ' +
                        '    <div class="col-12 nopadding ContainerFormCreaUtente">                                                                                                              ' +
                        '        <input type="text" placeholder="Email" id="MailUtente" Class="form-control InputMail" name="mail" />                                      ' +
                        '    </div>                                                                                                                                     ' +
                        '</div>   ',
                })
            }
        }
    })
}
function CreaUtente() {
    Swal.fire({
        title: 'Crea Utente',
        html:
            '<div class="row" style="margin-top:16px;">                                                                                                  ' +
            '    <div class="col-12 nopadding ContainerFormCreaUtente">                                                                                                              ' +
            '        <input type="text" placeholder="Nome Utente" id="UsernameUtente" Class="form-control InputUsername" name="username" />                                      ' +
            '    </div>                                                                                                                                     ' +
            '</div>         ' +
            '<div class="row" style="margin-bottom:16px;margin-top:16px;">                                                                                                  ' +
            '    <div class="col-12 nopadding ContainerFormCreaUtente">                                                                                                              ' +
            '        <input type="text" placeholder="Email" id="MailUtente" Class="form-control InputMail" name="mail" />                                      ' +
            '    </div>                                                                                                                                     ' +
            '</div>   ',
        showCloseButton: true,
        showCancelButton: false,
        focusConfirm: false,
        confirmButtonText:
            'Crea Utente'
    }).then((result) => {
        if (result.isConfirmed) {
            var username = $(".InputUsername").val();
            var mail = $(".InputMail").val();
            console.log(mail);
            console.log(username);
            $.ajax({
                type: 'POST',
                url: "/Account/CreaUtente?username=" + username + "&mail=" + mail,
                success: function (data) {
                        if (data.ok) {
                            Swal.fire(
                                'Evviva!',
                                'Utente creato correttamente.',
                                'success'
                            )
                        } else {
                            Swal.fire(
                                'Errore!',
                                'Impossibile creare l\'utente.',
                                'warning'
                            )
                        }
                }
            });
            
        }
    })
}
function PopupAggiungiArticolo() {
    Swal.fire({
        title: 'Aggiungi Articolo',
        html:
            '<div class="row" style="margin-top:16px;">                                                                                                  ' +
            '    <div class="col-12 nopadding ContainerFormCreaArticolo">                                                                                                              ' +
            '        <input type="text" placeholder="Codice Articolo" id="CodiceArticolo" Class="form-control InputArticolo" name="CodArticolo" />                                      ' +
            '    </div>                                                                                                                                     ' +
            '</div>         ' +
            '<div class="row" style="margin-bottom:16px;margin-top:16px;">                                                                                                  ' +
            '    <div class="col-12 nopadding ContainerFormCreaUtente">                                                                                                              ' +
            '        <input type="text" placeholder="Descrizione" id="DescrizioneArticolo" Class="form-control InputArticolo" name="DescArticolo" />                                      ' +
            '    </div>                                                                                                                                     ' +
            '</div>   ',
        showCloseButton: true,
        showCancelButton: false,
        focusConfirm: false,
        confirmButtonText:
            'Aggiungi articolo'
    }).then((result) => {
        if (result.isConfirmed) {
            var CodiceArticolo = $("#CodiceArticolo").val();
            var DescrizioneArticolo = $("#DescrizioneArticolo").val().replace(' ', '_');
            $.ajax({
                type: 'POST',
                url: "/Home/CreaArticolo?CodiceArticolo=" + CodiceArticolo + "&DescrizioneArticolo=" + DescrizioneArticolo,
                success: function (data) {
                    if (data.ok) {
                        Swal.fire({
                            title: 'Evviva!',
                            text: "Articolo aggiunto correttamente.",
                            icon: 'success',
                            showCancelButton: false,
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'Va bene!'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                location.reload();
                            }
                        })
                    } else {
                        Swal.fire(
                            'Errore!',
                            'Impossibile inserire l\'articolo.',
                            'warning'
                        )
                    }
                }
            });

        }
    })
}
function PopupRichiestaMateriali(idArticolo) {
    $.ajax({
        type: 'POST',
        url: "/Account/IsAdmin/",
        success: function (data) {
            if (data.ok) {
                console.log(data);
                Swal.fire({
                    html:
                        '<div class="testoPopup MainTesto"><h2>Articolo 1</h42></div>' +
                        '<div class="testoPopup DescrizioneTesto"><h4>Descrizione Articolo</h4></div>' +
                        '<div class="row" style="margin-bottom:50px;margin-top:50px;">                                                                                                  ' +
                        '    <div class="col-3 nopadding">                                                                                                              ' +
                        '        <button class="btn btnInput btnInputMinus"><i class="fa-solid fa-minus minus fa-2x"></i></button>                                      ' +
                        '    </div>                                                                                                                                     ' +
                        '    <div class="col-6 nopadding">                                                                                                              ' +
                        '        <input value="0" class="form-control InputQta" />                                                                                      ' +
                        '    </div>                                                                                                                                     ' +
                        '    <div class="col-3 nopadding">                                                                                                              ' +
                        '        <button class="btn btnInput btnInputPlus"><i class="fa-solid fa-plus plus fa-2x"></i></button>                                         ' +
                        '    </div>                                                                                                                                     ' +
                        '</div>                                                                                                                                         ',
                    showCloseButton: true,
                    showCancelButton: false,
                    showDenyButton: true,
                    focusConfirm: false,
                    confirmButtonText:
                        'Inserisci articoli',
                    denyButtonText: "🗑️"
                }).then((result) => {
                    if (result.isConfirmed) {
                        var qta = parseInt($(".InputQta").val());
                        if (qta == 0) {
                            Swal.fire(
                                'Attenzione!',
                                'La quantita\' deve essere almeno di 1 pezzo.',
                                'warning'
                            )
                        } else {
                            $.ajax({
                                type: 'POST',
                                url: "/Home/InserisciArticoloDaOrdinare?id=" + idArticolo + "&qta=" + qta,
                                success: function (data) {
                                    if (data.ok) {
                                        Swal.fire(
                                            'Evviva!',
                                            'Articolo correttamente inserito nel carrello.',
                                            'success'
                                        )
                                    }
                                }
                            });

                        }
                    } else if (result.isDenied) {
                        $.ajax({
                            type: 'POST',
                            url: "/Home/CancellaArticolo?id=" + idArticolo,
                            success: function (data) {
                                if (data.ok) {
                                    Swal.fire({
                                        title: 'Evviva!',
                                        text: "Articolo correttamente cancellato dal magazzino.",
                                        icon: 'success',
                                        showCancelButton: false,
                                        confirmButtonColor: '#3085d6',
                                        confirmButtonText: 'Va bene!'
                                    }).then((result) => {
                                        if (result.isConfirmed) {
                                            location.reload();
                                        }
                                    })
                                }
                            }
                        })
                    }
                })

                $(".btnInputPlus").click(function () {
                    var count = $(".InputQta").val();
                    if (count == '') {
                        count = 0;
                    }
                    $('.InputQta').val(parseInt(count) + 1)
                });
                $(".btnInputMinus").click(function () {
                    var count = $(".InputQta").val();
                    if (count == '') {
                        count = 0;
                    }
                    if (count != '0') {
                        $('.InputQta').val(parseInt(count) - 1);
                    }
                });
            } else {
                Swal.fire({
                    html:
                        '<div class="testoPopup MainTesto"><h2>Articolo 1</h42></div>' +
                        '<div class="testoPopup DescrizioneTesto"><h4>Descrizione Articolo</h4></div>' +
                        '<div class="row" style="margin-bottom:50px;margin-top:50px;">                                                                                                  ' +
                        '    <div class="col-3 nopadding">                                                                                                              ' +
                        '        <button class="btn btnInput btnInputMinus"><i class="fa-solid fa-minus minus fa-2x"></i></button>                                      ' +
                        '    </div>                                                                                                                                     ' +
                        '    <div class="col-6 nopadding">                                                                                                              ' +
                        '        <input value="0" class="form-control InputQta" />                                                                                      ' +
                        '    </div>                                                                                                                                     ' +
                        '    <div class="col-3 nopadding">                                                                                                              ' +
                        '        <button class="btn btnInput btnInputPlus"><i class="fa-solid fa-plus plus fa-2x"></i></button>                                         ' +
                        '    </div>                                                                                                                                     ' +
                        '</div>                                                                                                                                         ',
                    showCloseButton: true,
                    showCancelButton: false,
                    showDenyButton: false,
                    focusConfirm: false,
                    confirmButtonText:
                        'Inserisci articoli'
                }).then((result) => {
                    if (result.isConfirmed) {
                        var qta = parseInt($(".InputQta").val());
                        if (qta == 0) {
                            Swal.fire(
                                'Attenzione!',
                                'La quantita\' deve essere almeno di 1 pezzo.',
                                'warning'
                            )
                        } else {
                            $.ajax({
                                type: 'POST',
                                url: "/Home/InserisciArticoloDaOrdinare?id=" + idArticolo + "&qta=" + qta,
                                success: function (data) {
                                    if (data.ok) {
                                        Swal.fire(
                                            'Evviva!',
                                            'Articolo correttamente inserito nel carrello.',
                                            'success'
                                        )
                                    }
                                }
                            });

                        }
                    } else if (result.isDenied) {
                        $.ajax({
                            type: 'POST',
                            url: "/Home/CancellaArticolo?id=" + idArticolo,
                            success: function (data) {
                                if (data.ok) {
                                    Swal.fire({
                                        title: 'Evviva!',
                                        text: "Articolo correttamente cancellato dal magazzino.",
                                        icon: 'success',
                                        showCancelButton: false,
                                        confirmButtonColor: '#3085d6',
                                        confirmButtonText: 'Va bene!'
                                    }).then((result) => {
                                        if (result.isConfirmed) {
                                            location.reload();
                                        }
                                    })
                                }
                            }
                        })
                    }
                })

                $(".btnInputPlus").click(function () {
                    var count = $(".InputQta").val();
                    if (count == '') {
                        count = 0;
                    }
                    $('.InputQta').val(parseInt(count) + 1)
                });
                $(".btnInputMinus").click(function () {
                    var count = $(".InputQta").val();
                    if (count == '') {
                        count = 0;
                    }
                    if (count != '0') {
                        $('.InputQta').val(parseInt(count) - 1);
                    }
                });
            }
        }
    })
}




$('body').on('show.bs.modal', '.modal', function (e) {
    var button = $(e.relatedTarget); 
    var recipient = button.data('value');
    var type = button.data('type');
    $(this).find('.Delete').hide();
    switch (type) {
        //-------------------------------------------------------------------------------------------SEZIONE ARTICOLI
        case 'add_articolo':
            $(this).find('.modal-title').removeClass('text-danger').html('Crea Articolo');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').hide();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Articolo/Create');
            break;
        case 'edit_articolo':
            $(this).find('.modal-title').removeClass('text-danger').html('Modifica Articolo');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').hide();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Articolo/Edit/'+ recipient);
            break;
        case 'delete_articolo':
            $(this).find('.modal-title').removeClass('text-danger').html('Elimina Articolo');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').hide();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Articolo/Delete/'+ recipient);
            break;
        case 'details_articolo':
            $(this).find('.modal-title').removeClass('text-danger').html('Dettagli Articolo');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').hide();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Articolo/Details/' + recipient);
            break;
        //-------------------------------------------------------------------------------------------SEZIONE MAGAZZINI
        case 'add_magazzino':
            $(this).find('.modal-title').removeClass('text-danger').html('Crea Magazzino');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').hide();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Magazzino/Create');
            break;
        case 'edit_magazzino':
            $(this).find('.modal-title').removeClass('text-danger').html('Modifica Magazzino');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').hide();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Magazzino/Edit/' + recipient);
            break;
        case 'delete_magazzino':
            $(this).find('.modal-title').removeClass('text-danger').html('Elimina Magazzino');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').hide();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Magazzino/Delete/' + recipient);
            break;
        case 'details_magazzino':
            $(this).find('.modal-title').removeClass('text-danger').html('Dettagli Magazzino');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').hide();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Magazzino/Details/' + recipient);
            break;
        //-------------------------------------------------------------------------------------------SEZIONE REPARTI
        case 'add_reparto':
            $(this).find('.modal-title').removeClass('text-danger').html('Crea Reparto');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').hide();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Reparto/Create');
            break;
        case 'edit_reparto':
            $(this).find('.modal-title').removeClass('text-danger').html('Modifica Reparto');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').hide();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Reparto/Edit/' + recipient);
            break;
        case 'delete_reparto':
            $(this).find('.modal-title').removeClass('text-danger').html('Elimina Reparto');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').hide();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Reparto/Delete/' + recipient);
            break;
        case 'details_reparto':
            $(this).find('.modal-title').removeClass('text-danger').html('Dettagli Reparto');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').hide();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Reparto/Details/' + recipient);
            break;
        //-------------------------------------------------------------------------------------------SEZIONE MACCHINE
        case 'add_macchina':
            $(this).find('.modal-title').removeClass('text-danger').html('Crea Macchina');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').hide();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Macchina/Create');
            break;
        case 'edit_macchina':
            $(this).find('.modal-title').removeClass('text-danger').html('Modifica Macchina');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').hide();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Macchina/Edit/' + recipient);
            break;
        case 'delete_macchina':
            $(this).find('.modal-title').removeClass('text-danger').html('Elimina Macchina');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').hide();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Macchina/Delete/' + recipient);
            break;
        case 'details_macchina':
            $(this).find('.modal-title').removeClass('text-danger').html('Dettagli Macchina');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').hide();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Macchina/Details/' + recipient);
            break;
        //-------------------------------------------------------------------------------------------SEZIONE ATTIVITA'
        case 'aziona_attivita':
            $(this).find('.modal-title').removeClass('text-danger').html('Apri Attività');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Attrezzaggio').show();
            $(this).find('.Save').show();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Operatori/ApriAttività/'+ recipient);
            break;
        case 'sospendi_attivita':
            $(this).find('.modal-title').removeClass('text-danger').html('Sospendi Attività');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').show();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Operatori/SospendiAttività/' + recipient);
            break;
        case 'concludi_attivita':
            $(this).find('.modal-title').removeClass('text-danger').html('Concludi Attività');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').show();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Operatori/ConcludiAttività/' + recipient);
            break;
        //-------------------------------------------------------------------------------------------SEZIONE GIACENZE
        case 'add_giacenza':
            $(this).find('.modal-title').removeClass('text-danger').html('Aggiungi Giacenza');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').show();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Giacenza/Create/');
            break;
        case 'edit_giacenza':
            $(this).find('.modal-title').removeClass('text-danger').html('Modifica Giacenza');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').show();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Giacenza/Edit/' + recipient);
            break;
        case 'details_giacenza':
            $(this).find('.modal-title').removeClass('text-danger').html('Dettagli giacenza');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').show();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Giacenza/Details/' + recipient);
            break;
        case 'delete_giacenza':
            $(this).find('.modal-title').removeClass('text-danger').html('Elimina Giacenza');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').show();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Giacenza/Delete/' + recipient);
            break;
        case 'create_odp':
            $(this).find('.modal-title').removeClass('text-danger').html('Crea ODP manuale');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').show();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Giacenza/CreateODP/' + recipient);
            break;
        //-------------------------------------------------------------------------------------------SEZIONE LOTTI
        case 'edit_lotto':
            $(this).find('.modal-title').removeClass('text-danger').html('Modifica Lotto');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').show();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Lotti/Edit/' + recipient);
            break;
        case 'details_lotto':
            $(this).find('.modal-title').removeClass('text-danger').html('Dettagli Lotto');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').hide();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Lotti/Details/' + recipient);
            break;
        case 'delete_lotto':
            $(this).find('.modal-title').removeClass('text-danger').html('Elimina Lotto');
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').show();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Brighetti_Lotti/Delete/' + recipient);
            break;
        //-------------------------------------------------------------------------------------------SEZIONE LOTTI
        case 'show_ordine':
            $(this).find('.modal-title').removeClass('text-danger').html('Dettagli Ordine ' + recipient);
            $(this).data('reload', false);
            $(this).find('.Send').hide();
            $(this).find('.Add').hide();
            $(this).find('.Delete').hide();
            $(this).find('.Save').show();
            $(this).find('.SaveClose').hide();
            $(this).find('.modal-body').html('').load('/Home/MostraOrdine?id=' + recipient);
            break;
    };
});
$('body').on('click', '.ModalSubmit', function (e) {
    e.preventDefault();
    $('body .modal').modal('hide');
    $("body .ModalForm").submit();

    //if (e.currentTarget.classList.contains("Attrezzaggio")) {
    //    submitAttrezzaggio()
    //} else {
    //}

});
$('body').on('submit', '.ModalForm', function (e) {
    console.log(e);
    console.log($(this));
    e.preventDefault();
    var form = $(this);
    var formData = new FormData($(this)[0]);
    $.ajax({
        url: form.attr('action'),
        method: form.attr('method'),
        data: formData,
        processData: false,
        contentType: false,
        async: false,
        beforeSend: function (jqXHR, settings) {
            console.log(settings);
        },
        success: function (result) {
            console.log(result);
            if (result.ok) {
                $.notify({ message: result.message }, { type: 'success' });
                if (typeof result.pathRedirect === 'undefined') {
                    window.location.reload();
                } else {
                    document.location.href = result.pathRedirect;
                }
            }
            else {
                console.log(result);
                $.notify({ message: result.message }, { type: 'danger' });
            }
        },
        error: function (result) {
            console.log(result);
            $.notify({ message: result.message }, { type: 'danger' });
        }
    });
    return false;
});
async function AddNota(id,type) {
    const { value: text } = await Swal.fire({
        input: 'textarea',
        inputLabel: 'Agiungi qui la nota',
        inputPlaceholder: 'Inserisci qui la nota aggiuntiva...',
        inputAttributes: {
            'aria-label': 'Inserisci qui la nota aggiuntiva'
        },
        confirmButtonText: 'Aggiungi',
    })

    if (text) {
        $.ajax({
            url: '/Home/AddNota',
            type: 'POST',
            data: { Id: id, Nota: text, Type:type },
            dataType: 'json',
            success: function (result) {
                if (result.ok) {
                    $.notify({ message: result.message }, { type: 'success' });

                }
                else {
                    console.log(result);
                    $.notify({ message: result.message }, { type: 'danger' });
                }
            },
            error: function (result) {
                console.log(result);
                $.notify({ message: result.message }, { type: 'danger' });
            }
        });
    }
};

function DeleteNota(id) {
    Swal.fire({
        title: "Eliminare nota?",
        text: "ATTENZIONE: questa operazione sarà irreversibile",
        icon: 'warning',
        showCancelButton: false,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sì, cancella!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Home/DeleteNota',
                type: 'POST',
                data: { Id: id },
                dataType: 'json',
                success: function (result) {
                    if (result.ok) {
                        $.notify({ message: result.message }, { type: 'success' });

                    }
                    else {
                        console.log(result);
                        $.notify({ message: result.message }, { type: 'danger' });
                    }
                },
                error: function (result) {
                    console.log(result);
                    $.notify({ message: result.message }, { type: 'danger' });
                }
            });
        }
    });
}

async function AddFile(id, Type) {
    const { value: file } = await Swal.fire({
        title: 'Seleziona File',
        input: 'file',
        inputAttributes: {
            'accept': '*',
            'aria-label': 'Carica File per Documento'
        }
    });

    if (file) {
        var myformData = new FormData();
        myformData.append('id', id);
        myformData.append('file', file);
        myformData.append('Type', Type);
        $.ajax({
            url: '/Home/AddFile',
            type: 'POST',
            processData: false,
            contentType: false,
            cache: false,
            data: myformData,
            enctype: 'multipart/form-data',
            success: function (result) {
                if (result.ok) {
                    $.notify({ message: result.message }, { type: 'success' });
                }
                else {
                    console.log(result);
                    $.notify({ message: result.message }, { type: 'danger' });
                }
            },
            error: function (result) {
                console.log(result);
                $.notify({ message: result.message }, { type: 'danger' });
            }
        });

    }
};

function DeleteFile(id) {
    Swal.fire({
        title: "Eliminare file?",
        text: "ATTENZIONE: questa operazione sarà irreversibile",
        icon: 'warning',
        showCancelButton: false,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sì, cancella!'
    }).then((result) => {
        if (result.isConfirmed) {
            var myformData = new FormData();
            myformData.append('id', id);

            $.ajax({
                url: '/Home/DeleteFile',
                type: 'POST',
                processData: false,
                contentType: false,
                cache: false,
                data: myformData,
                enctype: 'multipart/form-data',
                success: function (result) {
                    if (result.ok) {
                        $.notify({ message: result.message }, { type: 'success' });
                    }
                    else {
                        console.log(result);
                        $.notify({ message: result.message }, { type: 'danger' });
                    }
                },
                error: function (result) {
                    console.log(result);
                    $.notify({ message: result.message }, { type: 'danger' });
                }
            });
        }
    });
}