@ModelType IEnumerable(Of Brighetti.MacchineStatiViewModel)
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
                <Button type="button" id="Send_Btn" class="btn btn-primary Send ModalSubmit">Invia</Button>
                <Button type="button" class="btn btn-danger Delete ModalSubmit">Elimina</Button>
                <Button type="button" class="btn btn-primary Save ModalSubmit BtnAggiungiModal">Salva Modifiche</Button>
                <Button type="button" class="btn btn-secondary SaveClose ModalSubmit">Salva e Chiudi</Button>
            </div>
        </div>
    </div>
</div>
<table class="table" id="MainDataTableMacchineStati">
    <thead>
        <tr>
            <th>
                @Html.DisplayNameFor(Function(model) model.NomeMacchina)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.StatoMacchina)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.UltimiDatiRilevatia)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.UltimoProgrammaMacchina)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.GraficoAndamento)
            </th>
            <th>Azioni</th>
        </tr>
    </thead>

    <tbody>
        @For Each item In Model
            @<tr>
    <td>
        @Html.DisplayFor(Function(modelItem) item.NomeMacchina)
    </td>
    <td>
        @Html.DisplayFor(Function(modelItem) item.StatoMacchina)
    </td>
    <td>
        @Html.DisplayFor(Function(modelItem) item.UltimiDatiRilevatia)
    </td>
    <td>
        @Html.DisplayFor(Function(modelItem) item.UltimoProgrammaMacchina)
    </td>
    <td>
        @Html.Raw(item.GraficoAndamento)
    </td>
    <td style="text-align: right;">
        <a  href="@Url.Action("Analisi", "Macchine", New With {.id = item.IdMacchina})"Class="fa-gradient  btn  w-auto" style="color:white;" >
            <i class="fa-solid fa-magnifying-glass"></i>
        </a>
    </td>
</tr>
        Next
    </tbody>

</table>
