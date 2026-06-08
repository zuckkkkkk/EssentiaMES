Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class LottiArticoli
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Brighetti_Lotti",
                Function(c) New With
                    {
                        .IdLotto = c.Int(nullable := False, identity := True),
                        .NomeLotto = c.Int(nullable := False),
                        .DescrizioneLotto = c.Int(nullable := False),
                        .UltimaModifica_OperatoreID = c.String(),
                        .UltimaModifica_Operatore = c.String(),
                        .UltimaModifica_Data = c.DateTime()
                    }) _
                .PrimaryKey(Function(t) t.IdLotto)
            
            CreateTable(
                "dbo.Brighetti_Lotti_Articoli",
                Function(c) New With
                    {
                        .IdLottoArticolo = c.Int(nullable := False, identity := True),
                        .IdLotto = c.Int(nullable := False),
                        .IdArticolo = c.Int(nullable := False),
                        .NomeArticolo = c.String(),
                        .QuantitàArticolo = c.String(),
                        .StatoArticoloLotto = c.Int(nullable := False),
                        .UltimaModifica_OperatoreID = c.String(),
                        .UltimaModifica_Operatore = c.String(),
                        .UltimaModifica_Data = c.DateTime()
                    }) _
                .PrimaryKey(Function(t) t.IdLottoArticolo)
            
        End Sub
        
        Public Overrides Sub Down()
            DropTable("dbo.Brighetti_Lotti_Articoli")
            DropTable("dbo.Brighetti_Lotti")
        End Sub
    End Class
End Namespace
