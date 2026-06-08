Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class LottiStato1
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AlterColumn("dbo.Brighetti_Lotti", "NomeLotto", Function(c) c.String())
            AlterColumn("dbo.Brighetti_Lotti", "DescrizioneLotto", Function(c) c.String())
        End Sub
        
        Public Overrides Sub Down()
            AlterColumn("dbo.Brighetti_Lotti", "DescrizioneLotto", Function(c) c.Int(nullable := False))
            AlterColumn("dbo.Brighetti_Lotti", "NomeLotto", Function(c) c.Int(nullable := False))
        End Sub
    End Class
End Namespace
