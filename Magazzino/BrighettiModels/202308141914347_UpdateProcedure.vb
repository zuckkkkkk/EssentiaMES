Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class UpdateProcedure
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Brighetti_Procedure", "TipoAttivita", Function(c) c.Int(nullable := False))
            AddColumn("dbo.Brighetti_Procedure", "idMacchinaMagazzino", Function(c) c.Int(nullable := False))
            DropColumn("dbo.Brighetti_Procedure", "idMacchina")
        End Sub
        
        Public Overrides Sub Down()
            AddColumn("dbo.Brighetti_Procedure", "idMacchina", Function(c) c.Int(nullable := False))
            DropColumn("dbo.Brighetti_Procedure", "idMacchinaMagazzino")
            DropColumn("dbo.Brighetti_Procedure", "TipoAttivita")
        End Sub
    End Class
End Namespace
