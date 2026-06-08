Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class DocumentiNote
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Brighetti_Documenti",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .Nome_File = c.String(),
                        .TypeElem = c.Int(nullable := False),
                        .Percorso_File = c.String(),
                        .DataCreazioneFile = c.DateTime(nullable := False),
                        .Operatore_Id = c.String(),
                        .Operatore_Nome = c.String()
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            CreateTable(
                "dbo.Brighetti_Note",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .TypeElem = c.Int(nullable := False),
                        .Contenuto_Nota = c.String(),
                        .Data_Nota = c.DateTime(nullable := False),
                        .Operatore_Id = c.String(),
                        .Operatore_Nome = c.String()
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
        End Sub
        
        Public Overrides Sub Down()
            DropTable("dbo.Brighetti_Note")
            DropTable("dbo.Brighetti_Documenti")
        End Sub
    End Class
End Namespace
