@ModelType IEnumerable(Of Brighetti.Brighetti_Articolo_ViewModel)

<!-- Modal -->
<style>
    .dataTables_paginate{
        margin-bottom: 5rem!important;

    }
</style>
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
        <h2>Dashboard Articoli</h2>
    </div>
    <div class="col-6" style="text-align: left!important;">
        <button data-type="add_articolo" Class="BtnAggiungiModal  btn  w-auto" style="color:white;" data-bs-toggle="modal" data-bs-target="#exampleModal">
            Crea Articolo
        </button>
    </div>
</div>

<table class="table" id="MainDataTableArticoli" style="margin-bottom: 1rem!important;">
    <thead>
        <tr>
            <th>
                @Html.DisplayNameFor(Function(model) model.CodiceArticolo)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.DescrizioneArticolo)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.Importanza)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.FamigliaId)
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
                    @Html.DisplayFor(Function(modelItem) item.CodiceArticolo)
                </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.DescrizioneArticolo)
                </td>
                <td>
                    @Html.Raw(item.Importanza)
                </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.FamigliaId)
                </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.UltimaModifica.Data.ToString.Split(" ")(0))
                </td>
                <td>
                    <div class="row">
                        <div class="col-3">
                            <button data-type="edit_articolo" data-value="@item.Id" Class="fa-gradient  btn  w-auto" style="color:white;" data-bs-toggle="modal" data-bs-target="#exampleModal">
                                <i class="fa-solid fa-pen-to-square"></i>
                            </button>
                        </div>
                        <div class="col-3">
                            <button onclick="AddNota('@item.Id', 1);" type="button" data-value="@item.Id" Class="fa-gradient  btn  w-auto">
                                <i class="fa-solid fa-note-sticky"></i>
                            </button>
                        </div>
                        <div class="col-3">
                            <button onclick="AddFile('@item.Id', 1);" type="button" data-value="@item.Id" Class="fa-gradient  btn  w-auto">
                                <i class="fa-solid fa-file-arrow-up"></i>
                            </button>
                        </div>
                        <div class="col-3">
                            <button data-type="details_articolo" data-value="@item.Id" Class="fa-gradient  btn  w-auto" style="color:white;" data-bs-toggle="modal" data-bs-target="#exampleModal">
                                <i class="fa-solid fa-magnifying-glass"></i>
                            </button>
                        </div>
                    </div>
                    @*<button data-type="delete_articolo" data-value="@item.Id" Class="fa-gradient  btn  w-auto" style="color:white;" data-bs-toggle="modal" data-bs-target="#exampleModal">
                        <i class="fa-solid fa-trash"></i>
                    </button>*@
                </td>
            </tr>
        Next
    </tbody>

</table>
