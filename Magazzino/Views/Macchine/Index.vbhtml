@ModelType IEnumerable(Of Brighetti.Macchine)
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
            @Html.DisplayNameFor(Function(model) model.NomeMacchina)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.DescrizioneMacchina)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.IndirizzoMacchina)
        </th>
        <th></th>
    </tr>

@For Each item In Model
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) item.NomeMacchina)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.DescrizioneMacchina)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.IndirizzoMacchina)
        </td>
        <td>
            @Html.ActionLink("Edit", "Edit", New With {.id = item.Id }) |
            @Html.ActionLink("Details", "Details", New With {.id = item.Id }) |
            @Html.ActionLink("Delete", "Delete", New With {.id = item.Id })
        </td>
    </tr>
Next

</table>
