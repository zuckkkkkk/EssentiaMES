Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace IdentityMigrations
    Public Partial Class RepartiIdentity
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.AspNetMacchineUsers", "IdReparto", Function(c) c.Int(nullable := False))
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.AspNetMacchineUsers", "IdReparto")
        End Sub
    End Class
End Namespace
