Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class FamiglieProdotti1
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.FamiglieProdotto",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .NomeFamiglia = c.String(),
                        .DescrizioneFamiglia = c.String()
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
        End Sub
        
        Public Overrides Sub Down()
            DropTable("dbo.FamiglieProdotto")
        End Sub
    End Class
End Namespace
