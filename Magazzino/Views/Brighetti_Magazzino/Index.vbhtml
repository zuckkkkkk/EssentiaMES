@ModelType IEnumerable(Of Brighetti.Brighetti_Magazzino)
@Code
    ViewData("Title") = "Gestione Magazzini"
End Code


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
    <div class="col-6">
        <h2>Dashboard Magazzini</h2>
    </div>
    <div class="col-6" style="text-align: left!important;">
        <button data-type="add_magazzino" Class="BtnAggiungiMagazzinoModal  btn  w-auto" style="color:white;" data-bs-toggle="modal" data-bs-target="#exampleModal">
            Crea Magazzino
        </button>
    </div>
</div>

<table class="table" id="MainDataTableMagazzini">
    <thead>
        <tr>
            <th>
                @Html.DisplayNameFor(Function(model) model.CodiceMagazzino)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.DescrizioneMagazzino)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.UltimaModifica)
            </th>
            <th>Azioni</th>
        </tr>
    </thead>

    <tbody>
        @For Each item In Model
            @<tr>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.CodiceMagazzino)
                </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.DescrizioneMagazzino)
                </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.UltimaModifica.Data)
                </td>
                <td>
                    <button data-type="edit_magazzino" data-value="@item.Id" Class="fa-gradient  btn  w-auto" style="color:white;" data-bs-toggle="modal" data-bs-target="#exampleModal">
                        <i class="fa-solid fa-pen-to-square"></i>
                    </button>
                    <button data-type="details_magazzino" data-value="@item.Id" Class="fa-gradient  btn  w-auto" style="color:white;" data-bs-toggle="modal" data-bs-target="#exampleModal">
                        <i class="fa-solid fa-magnifying-glass"></i>
                    </button>
                    <button data-type="delete_magazzino" data-value="@item.Id" Class="fa-gradient  btn  w-auto" style="color:white;" data-bs-toggle="modal" data-bs-target="#exampleModal">
                        <i class="fa-solid fa-trash"></i>
                    </button>
                </td>
            </tr>
        Next
    </tbody>

</table>
