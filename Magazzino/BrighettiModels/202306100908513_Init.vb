Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class Init
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Articoli",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .Nome_Articolo = c.String(),
                        .Descrizione_Articolo = c.String(),
                        .MagazzinoId = c.Int(nullable := False),
                        .ArticoloCancellato = c.Boolean(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            CreateTable(
                "dbo.Magazzino",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .NomeMagazzino = c.String(),
                        .DescrizioneMagazzino = c.String()
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            CreateTable(
                "dbo.Ordini",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .IdOrdine = c.Int(nullable := False),
                        .IdArticolo = c.Int(nullable := False),
                        .QtaArticolo = c.Int(nullable := False),
                        .UltimaModifica_OperatoreID = c.String(),
                        .UltimaModifica_Operatore = c.String(),
                        .UltimaModifica_Data = c.DateTime()
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            CreateTable(
                "dbo.OrdiniInCorso",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .IdArticolo = c.Int(nullable := False),
                        .QtaArticolo = c.Int(nullable := False),
                        .UltimaModifica_OperatoreID = c.String(),
                        .UltimaModifica_Operatore = c.String(),
                        .UltimaModifica_Data = c.DateTime()
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
        End Sub
        
        Public Overrides Sub Down()
            DropTable("dbo.OrdiniInCorso")
            DropTable("dbo.Ordini")
            DropTable("dbo.Magazzino")
            DropTable("dbo.Articoli")
        End Sub
    End Class
End Namespace
