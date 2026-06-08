Imports System
Imports System.Data.Entity
Imports System.Data.Entity.Migrations
Imports System.Linq
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework

Namespace IdentityMigrations

    Friend NotInheritable Class Configuration
        Inherits DbMigrationsConfiguration(Of ApplicationDbContext)

        Public Sub New()
            AutomaticMigrationsEnabled = False
            MigrationsDirectory = "IdentityMigrations"
        End Sub

        Protected Overrides Sub Seed(context As ApplicationDbContext)
            If Not context.Roles.Any(Function(r) r.Name = "Admin") Then
                Dim store = New RoleStore(Of IdentityRole)(context)
                Dim manager = New RoleManager(Of IdentityRole)(store)
                Dim role = New IdentityRole With {.Name = "Admin"}
                manager.Create(role)
            End If
            If Not context.Roles.Any(Function(r) r.Name = "Operatore") Then
                Dim store = New RoleStore(Of IdentityRole)(context)
                Dim manager = New RoleManager(Of IdentityRole)(store)
                Dim role = New IdentityRole With {.Name = "Operatore"}
                manager.Create(role)
            End If
            If Not context.Roles.Any(Function(r) r.Name = "Reparto") Then
                Dim store = New RoleStore(Of IdentityRole)(context)
                Dim manager = New RoleManager(Of IdentityRole)(store)
                Dim role = New IdentityRole With {.Name = "Reparto"}
                manager.Create(role)
            End If

            If Not context.Users.Any(Function(u) u.UserName = "Admin") Then
                Dim store = New UserStore(Of ApplicationUser)(context)
                Dim manager = New UserManager(Of ApplicationUser)(store)
                Dim user = New ApplicationUser With {.UserName = "Admin"}
                manager.Create(user, "Brighetti.1")
                manager.AddToRole(user.Id, "Admin")
            End If
            If Not context.Users.Any(Function(u) u.UserName = "Operatore") Then
                Dim store = New UserStore(Of ApplicationUser)(context)
                Dim manager = New UserManager(Of ApplicationUser)(store)
                Dim user = New ApplicationUser With {.UserName = "Operatore"}
                manager.Create(user, "Brighetti.1")
                manager.AddToRole(user.Id, "Operatore")
            End If
            If Not context.Users.Any(Function(u) u.UserName = "Fresa") Then
                Dim store = New UserStore(Of ApplicationUser)(context)
                Dim manager = New UserManager(Of ApplicationUser)(store)
                Dim user = New ApplicationUser With {.UserName = "Fresa"}
                manager.Create(user, "Brighetti.1")
                manager.AddToRole(user.Id, "Reparto")
            End If
            If Not context.Users.Any(Function(u) u.UserName = "Utente_Tornitura") Then
                Dim store = New UserStore(Of ApplicationUser)(context)
                Dim manager = New UserManager(Of ApplicationUser)(store)
                Dim user = New ApplicationUser With {.UserName = "Utente_Tornitura"}
                manager.Create(user, "Brighetti.1")
                manager.AddToRole(user.Id, "Operatore")

            End If
            If Not context.Users.Any(Function(u) u.UserName = "Utente_Fresatura") Then
                Dim store = New UserStore(Of ApplicationUser)(context)
                Dim manager = New UserManager(Of ApplicationUser)(store)
                Dim user = New ApplicationUser With {.UserName = "Utente_Fresatura"}
                manager.Create(user, "Brighetti.1")
                manager.AddToRole(user.Id, "Operatore")
            End If

            If Not context.Users.Any(Function(u) u.UserName = "Utente_Rettifica") Then
                Dim store = New UserStore(Of ApplicationUser)(context)
                Dim manager = New UserManager(Of ApplicationUser)(store)
                Dim user = New ApplicationUser With {.UserName = "Utente_Rettifica"}
                manager.Create(user, "Brighetti.1")
                manager.AddToRole(user.Id, "Operatore")
            End If
        End Sub

    End Class

End Namespace
