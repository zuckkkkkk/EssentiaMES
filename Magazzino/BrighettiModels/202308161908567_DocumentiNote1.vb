Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class DocumentiNote1
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Brighetti_Documenti", "IdEsterno", Function(c) c.Int(nullable := False))
            AddColumn("dbo.Brighetti_Note", "IdEsterno", Function(c) c.Int(nullable := False))
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Brighetti_Note", "IdEsterno")
            DropColumn("dbo.Brighetti_Documenti", "IdEsterno")
        End Sub
    End Class
End Namespace
