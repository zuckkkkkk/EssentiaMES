Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class DatiMacchina
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Brighetti_Dati_Macchina",
                Function(c) New With
                    {
                        .id = c.Int(nullable := False, identity := True),
                        .idMacchina = c.Int(nullable := False),
                        .DatiValidi = c.Boolean(nullable := False),
                        .DataRilevazione = c.DateTime(nullable := False),
                        .Programma = c.String(),
                        .ProgrammaDescrizione = c.String(),
                        .StatoMacchina = c.String(),
                        .PezziProdotti = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.id)
            
        End Sub
        
        Public Overrides Sub Down()
            DropTable("dbo.Brighetti_Dati_Macchina")
        End Sub
    End Class
End Namespace
