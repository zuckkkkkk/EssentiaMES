Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class DatiMacchina1
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Brighetti_Dati_Macchina", "StatoOperativoMacchina", Function(c) c.String())
            AddColumn("dbo.Brighetti_Dati_Macchina", "EsecuzioneMacchina", Function(c) c.String())
            DropColumn("dbo.Brighetti_Dati_Macchina", "StatoMacchina")
        End Sub
        
        Public Overrides Sub Down()
            AddColumn("dbo.Brighetti_Dati_Macchina", "StatoMacchina", Function(c) c.String())
            DropColumn("dbo.Brighetti_Dati_Macchina", "EsecuzioneMacchina")
            DropColumn("dbo.Brighetti_Dati_Macchina", "StatoOperativoMacchina")
        End Sub
    End Class
End Namespace
