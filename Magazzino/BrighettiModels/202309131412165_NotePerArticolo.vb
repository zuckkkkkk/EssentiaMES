Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace BrighettiMigrations
    Public Partial Class NotePerArticolo
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Brighetti_Lotti_Articoli", "NoteArticolo", Function(c) c.String())
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Brighetti_Lotti_Articoli", "NoteArticolo")
        End Sub
    End Class
End Namespace
