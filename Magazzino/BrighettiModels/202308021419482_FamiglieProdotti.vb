Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class FamiglieProdotti
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Articoli", "FamigliaId", Function(c) c.Int(nullable := False))
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Articoli", "FamigliaId")
        End Sub
    End Class
End Namespace
