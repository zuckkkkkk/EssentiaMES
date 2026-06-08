Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class Brighetti_Models_000
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Brighetti_Articoli",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .CodiceArticolo = c.String(),
                        .DescrizioneArticolo = c.String(),
                        .NoteArticolo = c.String(),
                        .UltimaModifica_OperatoreID = c.String(),
                        .UltimaModifica_Operatore = c.String(),
                        .UltimaModifica_Data = c.DateTime()
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            CreateTable(
                "dbo.Brighetti_Attività",
                Function(c) New With
                    {
                        .IdAttività = c.Int(nullable := False, identity := True),
                        .IncrementaleProcedura = c.Short(nullable := False),
                        .CodiceArticolo = c.String(),
                        .NomeAttività = c.String(),
                        .idReparto = c.Int(nullable := False),
                        .idMacchina = c.Int(nullable := False),
                        .QuantitàProdotta = c.Decimal(nullable := False, precision := 18, scale := 2),
                        .QuantitàScartata = c.Decimal(nullable := False, precision := 18, scale := 2),
                        .StatoAttività = c.Int(nullable := False),
                        .UltimaModifica_OperatoreID = c.String(),
                        .UltimaModifica_Operatore = c.String(),
                        .UltimaModifica_Data = c.DateTime()
                    }) _
                .PrimaryKey(Function(t) t.IdAttività)
            
            CreateTable(
                "dbo.Brighetti_Giacenze",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .CodiceArticolo = c.String(),
                        .CodiceMagazzino = c.String(),
                        .QuantitàGiacenza = c.Decimal(nullable := False, precision := 18, scale := 2),
                        .QuantitàSottoscorta = c.Decimal(nullable := False, precision := 18, scale := 2),
                        .UltimaModifica_OperatoreID = c.String(),
                        .UltimaModifica_Operatore = c.String(),
                        .UltimaModifica_Data = c.DateTime()
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            CreateTable(
                "dbo.Brighetti_Macchine",
                Function(c) New With
                    {
                        .IdMacchina = c.Int(nullable := False, identity := True),
                        .NomeMacchina = c.String(),
                        .DescrizioneMacchina = c.String(),
                        .IdReparto = c.Int(nullable := False),
                        .IdUtente = c.String(),
                        .UltimaModifica_OperatoreID = c.String(),
                        .UltimaModifica_Operatore = c.String(),
                        .UltimaModifica_Data = c.DateTime()
                    }) _
                .PrimaryKey(Function(t) t.IdMacchina)
            
            CreateTable(
                "dbo.Brighetti_Magazzini",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .CodiceMagazzino = c.String(),
                        .DescrizioneMagazzino = c.String(),
                        .UltimaModifica_OperatoreID = c.String(),
                        .UltimaModifica_Operatore = c.String(),
                        .UltimaModifica_Data = c.DateTime()
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            CreateTable(
                "dbo.Brighetti_Procedure",
                Function(c) New With
                    {
                        .IdProcedura = c.Int(nullable := False, identity := True),
                        .CodiceArticolo = c.String(),
                        .IncrementaleProcedura = c.Short(nullable := False),
                        .NomeAttività = c.String(),
                        .idReparto = c.Int(nullable := False),
                        .idMacchina = c.Int(nullable := False),
                        .UltimaModifica_OperatoreID = c.String(),
                        .UltimaModifica_Operatore = c.String(),
                        .UltimaModifica_Data = c.DateTime()
                    }) _
                .PrimaryKey(Function(t) t.IdProcedura)
            
            CreateTable(
                "dbo.Brighetti_Reparti",
                Function(c) New With
                    {
                        .IdReparto = c.Int(nullable := False, identity := True),
                        .NomeReparto = c.String(),
                        .DescrizioneReparto = c.String(),
                        .UltimaModifica_OperatoreID = c.String(),
                        .UltimaModifica_Operatore = c.String(),
                        .UltimaModifica_Data = c.DateTime()
                    }) _
                .PrimaryKey(Function(t) t.IdReparto)
            
        End Sub
        
        Public Overrides Sub Down()
            DropTable("dbo.Brighetti_Reparti")
            DropTable("dbo.Brighetti_Procedure")
            DropTable("dbo.Brighetti_Magazzini")
            DropTable("dbo.Brighetti_Macchine")
            DropTable("dbo.Brighetti_Giacenze")
            DropTable("dbo.Brighetti_Attività")
            DropTable("dbo.Brighetti_Articoli")
        End Sub
    End Class
End Namespace
