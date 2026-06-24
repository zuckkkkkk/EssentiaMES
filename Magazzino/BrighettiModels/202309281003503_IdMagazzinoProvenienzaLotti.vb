Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class IdMagazzinoProvenienzaLotti
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Brighetti_Lotti", "IdMagazzinoProvenienza", Function(c) c.Int(nullable := False))
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Brighetti_Lotti", "IdMagazzinoProvenienza")
        End Sub
    End Class
End Namespace
