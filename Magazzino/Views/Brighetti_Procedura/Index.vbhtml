@ModelType IEnumerable(Of Brighetti.Brighetti_Procedura)
@Code
ViewData("Title") = "Index"
End Code

<h2>Index</h2>

<p>
    @Html.ActionLink("Create New", "Create")
</p>
<table class="table">
    <tr>
        <th>
            @Html.DisplayNameFor(Function(model) model.CodiceArticolo)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.IncrementaleProcedura)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.NomeAttività)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.idReparto)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.idMacchina)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.UltimaModifica)
        </th>
        <th></th>
    </tr>

@For Each item In Model
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) item.CodiceArticolo)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.IncrementaleProcedura)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.NomeAttività)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.idReparto)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.idMacchina)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.UltimaModifica)
        </td>
        <td>
            @Html.ActionLink("Edit", "Edit", New With {.id = item.IdProcedura }) |
            @Html.ActionLink("Details", "Details", New With {.id = item.IdProcedura }) |
            @Html.ActionLink("Delete", "Delete", New With {.id = item.IdProcedura })
        </td>
    </tr>
Next

</table>
