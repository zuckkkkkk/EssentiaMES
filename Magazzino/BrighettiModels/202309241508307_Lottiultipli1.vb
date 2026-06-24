Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class Lottiultipli1
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Brighetti_DettagliGiacenza",
                Function(c) New With
                    {
                        .id = c.Int(nullable := False, identity := True),
                        .idGiacenza = c.Int(nullable := False),
                        .idProcedura = c.Int(nullable := False),
                        .QuantitàProdotta = c.Decimal(nullable := False, precision := 18, scale := 2),
                        .Brighetti_Giacenza_Id = c.Int()
                    }) _
                .PrimaryKey(Function(t) t.id) _
                .ForeignKey("dbo.Brighetti_Giacenze", Function(t) t.Brighetti_Giacenza_Id) _
                .Index(Function(t) t.Brighetti_Giacenza_Id)
            
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.Brighetti_DettagliGiacenza", "Brighetti_Giacenza_Id", "dbo.Brighetti_Giacenze")
            DropIndex("dbo.Brighetti_DettagliGiacenza", New String() { "Brighetti_Giacenza_Id" })
            DropTable("dbo.Brighetti_DettagliGiacenza")
        End Sub
    End Class
End Namespace
