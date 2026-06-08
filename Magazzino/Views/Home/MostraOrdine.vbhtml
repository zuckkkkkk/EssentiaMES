@ModelType IEnumerable(Of Brighetti.AttivitàviewModel)
<table class="table" id="MainDataTableDettaglioOrdine">
    <thead>
        <tr>
            <th>
                @Html.DisplayNameFor(Function(model) model.NomeAttività)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.Quantitàprodotta)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.QuantitàScartata)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.TempoLavorazione)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.TempoAttrezzaggio)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.CountNote)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.CountAllegati)
            </th>
        </tr>
    </thead>

    <tbody>
        @For Each item In Model
            @<tr>
    <td>
        @Html.DisplayFor(Function(modelItem) item.NomeAttività)
    </td>
    <td>
        @Html.DisplayFor(Function(modelItem) item.Quantitàprodotta) pz.
    </td>
    <td>
        @Html.DisplayFor(Function(modelItem) item.QuantitàScartata) pz.
    </td>
    <td>
        @Html.DisplayFor(Function(modelItem) item.TempoLavorazione) min.
    </td>
    <td>
        @Html.DisplayFor(Function(modelItem) item.TempoAttrezzaggio) min.
    </td>
    <td>
        @Html.DisplayFor(Function(modelItem) item.CountNote)
    </td>
    <td>
        @Html.DisplayFor(Function(modelItem) item.CountAllegati)
    </td>
    
</tr>
        Next
    </tbody>
</table>

<script>
    $("#MainDataTableDettaglioOrdine").DataTable({
        dom: '<"row align-items-center"<"col col-auto"><"col"><"col col-auto"><"row align-items-center"<"col"><"col col-auto">>',
        language: {
            "decimal": ",",
            "emptyTable": "Nessun Dato Disponibile",
            "info": "Visualizzazione da _START_ a _END_ di _TOTAL_ dettagli",
            "infoEmpty": "Visualizzazione da 0 a 0 di 0 dettagli",
            "infoFiltered": "(Filtrati su _MAX_ dettagli Totali)",
            "infoPostFix": "",
            "thousands": ".",
            "lengthMenu": "Mostra _MENU_",
            "loadingRecords": "Caricamento...",
            "processing": '<i class="fa fa-circle-notch fa-spin fa-3x fa-fw" style="color: purple;"></i><span class="sr-only">Caricamento...</span> ',
            "search": "Cerca Magazzini <i class='fas fa-search'></i>",
            "zeroRecords": "Nessun dettaglio",
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
</script>