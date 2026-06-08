Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class _40Macchine
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Brighetti_Macchine", "TipologiaMacchina", Function(c) c.Byte(nullable := False))
            AddColumn("dbo.Brighetti_Macchine", "CollegamentoMacchina", Function(c) c.String())
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Brighetti_Macchine", "CollegamentoMacchina")
            DropColumn("dbo.Brighetti_Macchine", "TipologiaMacchina")
        End Sub
    End Class
End Namespace
