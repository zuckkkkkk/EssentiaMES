Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class CSV
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Brighetti_Dati_Macchina_Csv",
                Function(c) New With
                    {
                        .id = c.Int(nullable := False, identity := True),
                        .idMacchina = c.Int(nullable := False),
                        .DatiValidi = c.Boolean(nullable := False),
                        .DataRilevazione = c.DateTime(nullable := False),
                        .DataLog = c.DateTime(nullable := False),
                        .Programma = c.String(),
                        .ProgrammaDescrizione = c.String(),
                        .DataInizioLavorazione = c.DateTime(nullable := False),
                        .DataFineLavorazione = c.DateTime(nullable := False),
                        .PezziProdotti = c.Int(nullable := False),
                        .TempoInManuale = c.Decimal(nullable := False, precision := 18, scale := 2),
                        .TempoInAttività = c.Decimal(nullable := False, precision := 18, scale := 2),
                        .TempoInAutomatico = c.Decimal(nullable := False, precision := 18, scale := 2),
                        .TempoInLavorazione = c.Decimal(nullable := False, precision := 18, scale := 2)
                    }) _
                .PrimaryKey(Function(t) t.id)
            
        End Sub
        
        Public Overrides Sub Down()
            DropTable("dbo.Brighetti_Dati_Macchina_Csv")
        End Sub
    End Class
End Namespace
