Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace IdentityMigrations
    Public Partial Class Macchine
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.AspNetMacchineUsers",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .IdUtente = c.String(),
                        .IdMacchina = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
        End Sub
        
        Public Overrides Sub Down()
            DropTable("dbo.AspNetMacchineUsers")
        End Sub
    End Class
End Namespace
