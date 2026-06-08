Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class TipoAttività
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Brighetti_Attività", "TipoAttivitaProcedura", Function(c) c.Int(nullable := False))
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Brighetti_Attività", "TipoAttivitaProcedura")
        End Sub
    End Class
End Namespace
