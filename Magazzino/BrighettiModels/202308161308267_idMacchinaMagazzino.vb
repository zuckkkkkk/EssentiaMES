Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class idMacchinaMagazzino
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Brighetti_Attività", "idMacchinaMagazzino", Function(c) c.Int(nullable := False))
            DropColumn("dbo.Brighetti_Attività", "idMacchina")
        End Sub
        
        Public Overrides Sub Down()
            AddColumn("dbo.Brighetti_Attività", "idMacchina", Function(c) c.Int(nullable := False))
            DropColumn("dbo.Brighetti_Attività", "idMacchinaMagazzino")
        End Sub
    End Class
End Namespace
