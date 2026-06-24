Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class hiddenFlag
        Inherits DbMigration
    
        Public Overrides Sub Up()
            DropColumn("dbo.Brighetti_Articoli", "Hidden_AutoCreaODP")
            DropColumn("dbo.Brighetti_DettagliGiacenza", "ODP")
            DropColumn("dbo.Brighetti_DettagliGiacenza", "idAttività")
            DropColumn("dbo.Brighetti_Lotti_Articoli", "IncrementaleAttività")
            DropColumn("dbo.Brighetti_Lotti_Articoli", "OrdineDiProduzione")
        End Sub
        
        Public Overrides Sub Down()
            AddColumn("dbo.Brighetti_Lotti_Articoli", "OrdineDiProduzione", Function(c) c.String())
            AddColumn("dbo.Brighetti_Lotti_Articoli", "IncrementaleAttività", Function(c) c.Int(nullable := False))
            AddColumn("dbo.Brighetti_DettagliGiacenza", "idAttività", Function(c) c.Int(nullable := False))
            AddColumn("dbo.Brighetti_DettagliGiacenza", "ODP", Function(c) c.String())
            AddColumn("dbo.Brighetti_Articoli", "Hidden_AutoCreaODP", Function(c) c.Boolean(nullable := False))
        End Sub
    End Class
End Namespace
