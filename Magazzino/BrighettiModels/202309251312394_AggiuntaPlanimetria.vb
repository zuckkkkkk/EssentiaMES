Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class AggiuntaPlanimetria
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Brighetti_Planimetria",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .IdEsternoMacchina = c.Int(nullable := False),
                        .posX = c.Int(nullable := False),
                        .posY = c.Int(nullable := False),
                        .UltimaModifica_OperatoreID = c.String(),
                        .UltimaModifica_Operatore = c.String(),
                        .UltimaModifica_Data = c.DateTime()
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
        End Sub
        
        Public Overrides Sub Down()
            DropTable("dbo.Brighetti_Planimetria")
        End Sub
    End Class
End Namespace
