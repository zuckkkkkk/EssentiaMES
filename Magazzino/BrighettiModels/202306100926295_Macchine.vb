Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class Macchine
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.DatiMacchina",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .IdMacchina = c.Int(nullable := False),
                        .StatoAttività = c.String(),
                        .NomeProgramma = c.String(),
                        .ContaPezzi = c.String(),
                        .Data_OperatoreID = c.String(),
                        .Data_Operatore = c.String(),
                        .Data_Data = c.DateTime()
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            CreateTable(
                "dbo.Macchine",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .NomeMacchina = c.String(),
                        .DescrizioneMacchina = c.String(),
                        .IndirizzoMacchina = c.String()
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
        End Sub
        
        Public Overrides Sub Down()
            DropTable("dbo.Macchine")
            DropTable("dbo.DatiMacchina")
        End Sub
    End Class
End Namespace
