Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class FamiglieProdotti3
        Inherits DbMigration
    
        Public Overrides Sub Up()
            RenameTable(name := "dbo.FamiglieProdotto", newName := "Brighetti_FamiglieProdotto")
        End Sub
        
        Public Overrides Sub Down()
            RenameTable(name := "dbo.Brighetti_FamiglieProdotto", newName := "FamiglieProdotto")
        End Sub
    End Class
End Namespace
