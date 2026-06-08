Public Class DataTableAjaxPostModel
    Public Property draw As Integer
    Public Property start As Integer
    Public Property length As Integer
    Public Property columns As List(Of Column)
    Public Property search As Search
    Public Property order As List(Of Order)
End Class

Public Class Column
    Public Property data As String
    Public Property name As String
    Public Property searchable As Boolean
    Public Property orderable As Boolean
    Public Property search As Search
End Class

Public Class Search
    Public Property value As String
    Public Property regex As String
End Class

Public Class Order
    Public Property column As Integer
    Public Property dir As String
End Class