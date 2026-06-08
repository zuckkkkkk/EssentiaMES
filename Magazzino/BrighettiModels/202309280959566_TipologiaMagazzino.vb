Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class TipologiaMagazzino
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Brighetti_Magazzini", "TipologiaMagazzino", Function(c) c.Byte(nullable := False))
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Brighetti_Magazzini", "TipologiaMagazzino")
        End Sub
    End Class
End Namespace
