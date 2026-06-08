Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class Init1
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Brighetti_Previsione",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .CodiceArticolo = c.String(),
                        .DescrizioneArticolo = c.String(),
                        .QuantitaInEntrata = c.Int(nullable := False),
                        .IdAttività = c.Int(nullable := False),
                        .UltimaModifica_OperatoreID = c.String(),
                        .UltimaModifica_Operatore = c.String(),
                        .UltimaModifica_Data = c.DateTime()
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            AddColumn("dbo.Brighetti_Giacenze", "InPrevisioneEntrata", Function(c) c.Decimal(nullable := False, precision := 18, scale := 2))
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Brighetti_Giacenze", "InPrevisioneEntrata")
            DropTable("dbo.Brighetti_Previsione")
        End Sub
    End Class
End Namespace
