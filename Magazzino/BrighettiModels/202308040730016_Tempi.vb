Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class Tempi
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Brighetti_Tempi",
                Function(c) New With
                    {
                        .IdTempo = c.Int(nullable := False, identity := True),
                        .IdAttività = c.Int(nullable := False),
                        .TipoAttività = c.Int(nullable := False),
                        .DataInizio = c.DateTime(nullable := False),
                        .DataFine = c.DateTime(nullable := False),
                        .Note = c.String()
                    }) _
                .PrimaryKey(Function(t) t.IdTempo)
            
        End Sub
        
        Public Overrides Sub Down()
            DropTable("dbo.Brighetti_Tempi")
        End Sub
    End Class
End Namespace
