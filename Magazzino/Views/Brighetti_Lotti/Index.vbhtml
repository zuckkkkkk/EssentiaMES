@ModelType IEnumerable(Of Brighetti.Brighetti_Lotti_ViewModel)
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
                <Button type="button" class="btn btn-secondary SaveClose ModalSubmit">Salva e Chiudi</Button>
            </div>
        </div>
    </div>
</div>

<div class="row">
    <div class="col-6">
        <h2>Dashboard Lotti</h2>
    </div>
</div>
<table class="table" id="MainDataTableLotti">
    <thead>
        <tr>
            <th>
                @Html.DisplayNameFor(Function(model) model.StatoLotto)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.NomeLotto)
            </th>
            <th>
                @Html.DisplayNameFor(Function(model) model.DescrizioneLotto)
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
        @Html.Raw(item.StatoLotto)
    </td>
    <td>
        @Html.DisplayFor(Function(modelItem) item.NomeLotto)
    </td>
    <td>
        @Html.DisplayFor(Function(modelItem) item.DescrizioneLotto)
    </td>
    <td>
        @Html.DisplayFor(Function(modelItem) item.UltimaModifica.Data)
    </td>
    <td style="text-align: right;">
        <button data-type="edit_lotto" data-value="@item.IdLotto" Class="fa-gradient  btn  w-auto" style="color:white;" data-bs-toggle="modal" data-bs-target="#exampleModal">
            <i class="fa-solid fa-pen-to-square"></i>
        </button>
        <button data-type="details_lotto" data-value="@item.IdLotto" Class="fa-gradient  btn  w-auto" style="color:white;" data-bs-toggle="modal" data-bs-target="#exampleModal">
            <i class="fa-solid fa-magnifying-glass"></i>
        </button>
        <button onclick = "AddFile('@item.IdLotto', 5);" type="button" data-value="@item.IdLotto" Class="fa-gradient  btn  w-auto">
            <i Class="fa-solid fa-file-arrow-up"></i>
        </button>
        <Button onclick = "AddNota('@item.IdLotto', 5);" type="button" data-value="@item.IdLotto" Class="fa-gradient  btn  w-auto">
            <i Class="fa-solid fa-note-sticky"></i>
        </button>
        <a href="@Url.Action("ListaArticoli", "Brighetti_Lotti", New With {.id = item.IdLotto})" Class="fa-gradient  btn  w-auto"><i class="fa-solid fa-file-arrow-down"></i></a>
    </td>
</tr>
        Next
    </tbody>

</table>
