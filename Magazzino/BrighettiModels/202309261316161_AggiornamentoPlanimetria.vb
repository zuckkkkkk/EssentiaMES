Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class AggiornamentoPlanimetria
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AlterColumn("dbo.Brighetti_Planimetria", "posX", Function(c) c.String())
            AlterColumn("dbo.Brighetti_Planimetria", "posY", Function(c) c.String())
        End Sub
        
        Public Overrides Sub Down()
            AlterColumn("dbo.Brighetti_Planimetria", "posY", Function(c) c.Int(nullable := False))
            AlterColumn("dbo.Brighetti_Planimetria", "posX", Function(c) c.Int(nullable := False))
        End Sub
    End Class
End Namespace
