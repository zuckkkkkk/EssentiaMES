Imports System.Globalization
Imports System.IO
Imports System.Net.Mail
Imports System.Reflection
Imports System.Security.Claims
Imports System.Threading.Tasks
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security
Imports Owin

<Authorize>
Public Class AccountController
    Inherits Controller
    Private _signInManager As ApplicationSignInManager
    Private _userManager As ApplicationUserManager

    Private appctx As New ApplicationDbContext

    Public Sub New()
    End Sub

    Public Sub New(appUserMan As ApplicationUserManager, signInMan As ApplicationSignInManager)
        UserManager = appUserMan
        SignInManager = signInMan
    End Sub

    Public Property SignInManager() As ApplicationSignInManager
        Get
            Return If(_signInManager, HttpContext.GetOwinContext().[Get](Of ApplicationSignInManager)())
        End Get
        Private Set
            _signInManager = Value
        End Set
    End Property

    Public Property UserManager() As ApplicationUserManager
        Get
            Return If(_userManager, HttpContext.GetOwinContext().GetUserManager(Of ApplicationUserManager)())
        End Get
        Private Set
            _userManager = Value
        End Set
    End Property

    '
    ' GET: /Account/Login
    <AllowAnonymous>
    Public Function Login(returnUrl As String) As ActionResult
        ViewBag.BuildDate = IO.File.GetCreationTime(Assembly.GetExecutingAssembly().Location).ToString.Split(" ")(0)
        If User.Identity.IsAuthenticated Then
            If User.IsInRole("Operatore") Then
                Return RedirectToAction("Dashboard", "Macchine")
            Else
                Return RedirectToAction("AdminDashboard", "Macchine")
            End If
        End If
        ViewData!ReturnUrl = returnUrl
        Return View()
    End Function
    <HttpPost>
    Public Async Function CreaUtente(ByVal username As String, ByVal mail As String) As Task(Of JsonResult)
        If ModelState.IsValid Then
            Dim user = New ApplicationUser() With {
                .UserName = username,
                .Email = IIf(Not IsNothing(mail), mail, "info@mattiazucchini.com")
            }
            Dim psw = "Brighetti.1"
            Dim result = Await UserManager.CreateAsync(user, psw)
            appctx.SaveChanges()
            Try
                Dim AddToRole = UserManager.AddToRole(user.Id, "Operatore")
                appctx.SaveChanges()
            Catch ex As Exception

            End Try

            If result.Succeeded Then
                Try
                    'Dim mySmtp As New SmtpClient
                    'Dim myMail As New MailMessage()
                    'mySmtp.UseDefaultCredentials = False
                    'mySmtp.Credentials = New System.Net.NetworkCredential("hello@chefly.it", "Chefly2022!")
                    'mySmtp.Host = "chefly.it"
                    'myMail = New MailMessage()
                    'myMail.From = New MailAddress("hello@chefly.it")
                    'mySmtp.EnableSsl = False
                    'Dim StrContent = ""
                    'myMail.To.Add(mail)
                    'myMail.Subject = "Test"
                    'Using reader = New StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Views/Shared/NuovoAccount.html")
                    '    Dim readFile As String = reader.ReadToEnd()
                    '    StrContent = readFile
                    '    StrContent = StrContent.Replace("[username]", username)
                    '    StrContent = StrContent.Replace("[Password]", psw)
                    'End Using
                    'myMail.Body = StrContent.ToString
                    'myMail.IsBodyHtml = True
                    'mySmtp.Send(myMail)
                Catch ex As Exception

                End Try
                Return Json(New With {.ok = True})
            End If
            AddErrors(result)
        End If
    End Function
    Function IsAdmin() As JsonResult
        Dim OpName = User.Identity.Name
        Dim idRoleAdmin = appctx.Roles.Where(Function(x) x.Name = "Admin").First
        If (User.IsInRole("Admin")) Then
            Return Json(New With {.ok = True})
        Else
            Return Json(New With {.ok = False})
        End If
    End Function
    <HttpPost>
    <ValidateAntiForgeryToken>
    Public Async Function CreateAccount(model As RegisterViewModel) As Task(Of JsonResult)
        If ModelState.IsValid Then
            Dim user = New ApplicationUser() With {
                .UserName = model.Email,
                .Email = model.Email
            }
            Dim result = Await UserManager.CreateAsync(user, model.Password)
            If result.Succeeded Then
                Dim currentUser = UserManager.FindByName(user.UserName)
                Dim roleResult = UserManager.AddToRole(user.Id, "Operatore")
                Await SignInManager.SignInAsync(user, isPersistent:=False, rememberBrowser:=False)
                Return Json(New With {.ok = True})
            End If
            AddErrors(result)
        End If

    End Function
    <Authorize(Roles:="Admin")>
    Function CancellaUtente(ByVal id As String) As JsonResult
        Try
            Dim UserToBeRemoved = appctx.Users.Where(Function(x) x.Id = id).First
            appctx.Users.Remove(UserToBeRemoved)
            appctx.SaveChanges()
            Return Json(New With {.ok = True})
        Catch ex As Exception
            Return Json(New With {.ok = False})
        End Try
        Return Json(New With {.ok = True})
    End Function
    <Authorize(Roles:="Admin")>
    Function Utenti() As ActionResult
        Dim ListaUtenti As New List(Of UtentiViewModel)
        Dim Role = appctx.Roles.Where(Function(X) X.Name = "Operatore").First.Users
        For Each u In appctx.Users.ToList
            If Role.Where(Function(x) x.UserId = u.Id).Count > 0 Then
                ListaUtenti.Add(New UtentiViewModel With {
                .Id = u.Id,
                .Email = u.Email,
                .Username = u.UserName
            })
            End If
        Next
        Return View(ListaUtenti)
    End Function
    <Authorize(Roles:="Admin")>
    Function MostraUtente(ByVal id As String) As JsonResult
        Dim utente = appctx.Users.Find(id)
        Return Json(New With {.ok = True, .utente = utente})
    End Function
    '
    ' POST: /Account/Login
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function Login(model As LoginViewModel, returnUrl As String) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View(model)
        End If

        ' Questa opzione non calcola il numero di tentativi di accesso non riusciti per il blocco dell'account
        ' Per abilitare il conteggio degli errori di password per attivare il blocco, impostare shouldLockout := True
        Dim result = Await SignInManager.PasswordSignInAsync(model.username, model.Password, model.RememberMe, shouldLockout:=False)
        Select Case result
            Case SignInStatus.Success
                If User.IsInRole("Operatore") Then
                    Return RedirectToAction("AvvioAttivita", "Brighetti_Operatore")
                End If
                Return RedirectToLocal(returnUrl)
            Case SignInStatus.LockedOut
                Return View("Lockout")
            Case SignInStatus.RequiresVerification
                Return RedirectToAction("SendCode", New With {
                    returnUrl,
                    model.RememberMe
                })
            Case Else
                ModelState.AddModelError("", "Credenziali inserite non valide.")
                Return View(model)
        End Select
    End Function

    '
    ' GET: /Account/VerifyCode
    <AllowAnonymous>
    Public Async Function VerifyCode(provider As String, returnUrl As String, rememberMe As Boolean) As Task(Of ActionResult)
        ' Impostare come condizione che l'utente abbia già eseguito l'accesso con nome utente/password o account di accesso esterno
        If Not Await SignInManager.HasBeenVerifiedAsync() Then
            Return View("Error")
        End If
        Return View(New VerifyCodeViewModel() With {
            .Provider = provider,
            .ReturnUrl = returnUrl,
            .RememberMe = rememberMe
        })
    End Function

    '
    ' POST: /Account/VerifyCode
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function VerifyCode(model As VerifyCodeViewModel) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View(model)
        End If

        ' La parte di codice seguente protegge i codici di autenticazione a due fattori dagli attacchi di forza bruta. 
        ' Se un utente immette codici non corretti in un intervallo di tempo specificato, l'account dell'utente 
        ' viene bloccato per un intervallo di tempo specificato. 
        ' Si possono configurare le impostazioni per il blocco dell'account in IdentityConfig
        Dim result = Await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:=model.RememberMe, rememberBrowser:=model.RememberBrowser)
        Select Case result
            Case SignInStatus.Success
                Return RedirectToLocal(model.ReturnUrl)
            Case SignInStatus.LockedOut
                Return View("Lockout")
            Case Else
                ModelState.AddModelError("", "Codice non valido.")
                Return View(model)
        End Select
    End Function

    '
    ' GET: /Account/Register
    <AllowAnonymous>
    Public Function Register() As ActionResult
        Return View()
    End Function

    '
    ' POST: /Account/Register
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function Register(model As RegisterViewModel) As Task(Of ActionResult)
        If ModelState.IsValid Then
            Dim user = New ApplicationUser() With {
                .UserName = model.Email,
                .Email = model.Email
            }
            Dim result = Await UserManager.CreateAsync(user, model.Password)
            If result.Succeeded Then
                Await SignInManager.SignInAsync(user, isPersistent:=False, rememberBrowser:=False)

                ' Per altre informazioni su come abilitare la conferma dell'account e la reimpostazione della password, vedere https://go.microsoft.com/fwlink/?LinkID=320771
                ' Inviare un messaggio di posta elettronica con questo collegamento
                ' Dim code = Await UserManager.GenerateEmailConfirmationTokenAsync(user.Id)
                ' Dim callbackUrl = Url.Action("ConfirmEmail", "Account", New With { .userId = user.Id, code }, protocol := Request.Url.Scheme)
                ' Await UserManager.SendEmailAsync(user.Id, "Conferma account", "Per confermare l'account, fare clic <a href=""" & callbackUrl & """>qui</a>")

                Return RedirectToAction("Index", "Home")
            End If
            AddErrors(result)
        End If

        ' Se si è arrivati a questo punto, significa che si è verificato un errore, rivisualizzare il form
        Return View(model)
    End Function

    '
    ' GET: /Account/ConfirmEmail
    <AllowAnonymous>
    Public Async Function ConfirmEmail(userId As String, code As String) As Task(Of ActionResult)
        If userId Is Nothing OrElse code Is Nothing Then
            Return View("Error")
        End If
        Dim result = Await UserManager.ConfirmEmailAsync(userId, code)
        Return View(If(result.Succeeded, "ConfirmEmail", "Error"))
    End Function

    '
    ' GET: /Account/ForgotPassword
    <AllowAnonymous>
    Public Function ForgotPassword() As ActionResult
        Return View()
    End Function

    '
    ' POST: /Account/ForgotPassword
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function ForgotPassword(model As ForgotPasswordViewModel) As Task(Of ActionResult)
        If ModelState.IsValid Then
            Dim user = Await UserManager.FindByNameAsync(model.Email)
            If user Is Nothing OrElse Not (Await UserManager.IsEmailConfirmedAsync(user.Id)) Then
                ' Non rivelare che l'utente non esiste o non è confermato
                Return View("ForgotPasswordConfirmation")
            End If
            ' Per altre informazioni su come abilitare la conferma dell'account e la reimpostazione della password, vedere https://go.microsoft.com/fwlink/?LinkID=320771
            ' Inviare un messaggio di posta elettronica con questo collegamento
            ' Dim code = Await UserManager.GeneratePasswordResetTokenAsync(user.Id)
            ' Dim callbackUrl = Url.Action("ResetPassword", "Account", New With { .userId = user.Id, code }, protocol := Request.Url.Scheme)
            ' Await UserManager.SendEmailAsync(user.Id, "Reimposta password", "Per reimpostare la password, fare clic <a href=""" & callbackUrl & """>qui</a>")
            ' Return RedirectToAction("ForgotPasswordConfirmation", "Account")
        End If

        ' Se si è arrivati a questo punto, significa che si è verificato un errore, rivisualizzare il form
        Return View(model)
    End Function

    '
    ' GET: /Account/ForgotPasswordConfirmation
    <AllowAnonymous>
    Public Function ForgotPasswordConfirmation() As ActionResult
        Return View()
    End Function

    '
    ' GET: /Account/ResetPassword
    <AllowAnonymous>
    Public Function ResetPassword(code As String) As ActionResult
        Return If(code Is Nothing, View("Error"), View())
    End Function

    '
    ' POST: /Account/ResetPassword
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function ResetPassword(model As ResetPasswordViewModel) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View(model)
        End If
        Dim user = Await UserManager.FindByNameAsync(model.Email)
        If user Is Nothing Then
            ' Non rivelare che l'utente non esiste
            Return RedirectToAction("ResetPasswordConfirmation", "Account")
        End If
        Dim result = Await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password)
        If result.Succeeded Then
            Return RedirectToAction("ResetPasswordConfirmation", "Account")
        End If
        AddErrors(result)
        Return View()
    End Function

    '
    ' GET: /Account/ResetPasswordConfirmation
    <AllowAnonymous>
    Public Function ResetPasswordConfirmation() As ActionResult
        Return View()
    End Function

    '
    ' POST: /Account/ExternalLogin
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Function ExternalLogin(provider As String, returnUrl As String) As ActionResult
        ' Richiedere un reindirizzamento al provider di accesso esterno
        Return New ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", New With {
            returnUrl
        }))
    End Function

    '
    ' GET: /Account/SendCode
    <AllowAnonymous>
    Public Async Function SendCode(returnUrl As String, rememberMe As Boolean) As Task(Of ActionResult)
        Dim userId = Await SignInManager.GetVerifiedUserIdAsync()
        If userId Is Nothing Then
            Return View("Error")
        End If
        Dim userFactors = Await UserManager.GetValidTwoFactorProvidersAsync(userId)
        Dim factorOptions = userFactors.[Select](Function(purpose) New SelectListItem() With {
            .Text = purpose,
            .Value = purpose
        }).ToList()
        Return View(New SendCodeViewModel() With {
            .Providers = factorOptions,
            .ReturnUrl = returnUrl,
            .RememberMe = rememberMe
        })
    End Function

    '
    ' POST: /Account/SendCode
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function SendCode(model As SendCodeViewModel) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View()
        End If

        ' Generare il token e inviarlo
        If Not Await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider) Then
            Return View("Error")
        End If
        Return RedirectToAction("VerifyCode", New With {
            .Provider = model.SelectedProvider,
            model.ReturnUrl,
            model.RememberMe
        })
    End Function

    '
    ' GET: /Account/ExternalLoginCallback
    <AllowAnonymous>
    Public Async Function ExternalLoginCallback(returnUrl As String) As Task(Of ActionResult)
        Dim loginInfo = Await AuthenticationManager.GetExternalLoginInfoAsync()
        If loginInfo Is Nothing Then
            Return RedirectToAction("Login")
        End If

        ' Se l'utente ha già un account, consentire l'accesso dell'utente a questo provider di accesso esterno
        Dim result = Await SignInManager.ExternalSignInAsync(loginInfo, isPersistent:=False)
        Select Case result
            Case SignInStatus.Success
                Return RedirectToLocal(returnUrl)
            Case SignInStatus.LockedOut
                Return View("Lockout")
            Case SignInStatus.RequiresVerification
                Return RedirectToAction("SendCode", New With {
                    returnUrl,
                    .RememberMe = False
                })
            Case Else
                ' Se l'utente non ha un account, chiedere all'utente di crearne uno
                ViewData!ReturnUrl = returnUrl
                ViewData!LoginProvider = loginInfo.Login.LoginProvider
                Return View("ExternalLoginConfirmation", New ExternalLoginConfirmationViewModel() With {
                    .Email = loginInfo.Email
                })
        End Select
    End Function

    '
    ' POST: /Account/ExternalLoginConfirmation
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function ExternalLoginConfirmation(model As ExternalLoginConfirmationViewModel, returnUrl As String) As Task(Of ActionResult)
        If User.Identity.IsAuthenticated Then
            Return RedirectToAction("Index", "Manage")
        End If

        If ModelState.IsValid Then
            ' Recuperare le informazioni sull'utente dal provider di accesso esterno
            Dim info = Await AuthenticationManager.GetExternalLoginInfoAsync()
            If info Is Nothing Then
                Return View("ExternalLoginFailure")
            End If
            Dim userInfo = New ApplicationUser() With {
                .UserName = model.Email,
                .Email = model.Email
            }
            Dim result = Await UserManager.CreateAsync(userInfo)
            If result.Succeeded Then
                result = Await UserManager.AddLoginAsync(userInfo.Id, info.Login)
                If result.Succeeded Then
                    Await SignInManager.SignInAsync(userInfo, isPersistent:=False, rememberBrowser:=False)
                    Return RedirectToLocal(returnUrl)
                End If
            End If
            AddErrors(result)
        End If

        ViewData!ReturnUrl = returnUrl
        Return View(model)
    End Function

    '
    ' POST: /Account/LogOff
    Public Function LogOff() As ActionResult
        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie)
        Return RedirectToAction("Index", "Home")
    End Function

    '
    ' GET: /Account/ExternalLoginFailure
    <AllowAnonymous>
    Public Function ExternalLoginFailure() As ActionResult
        Return View()
    End Function

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If _userManager IsNot Nothing Then
                _userManager.Dispose()
                _userManager = Nothing
            End If
            If _signInManager IsNot Nothing Then
                _signInManager.Dispose()
                _signInManager = Nothing
            End If
        End If

        MyBase.Dispose(disposing)
    End Sub

#Region "Helper"
    ' Usato per la protezione XSRF durante l'aggiunta di account di accesso esterni
    Private Const XsrfKey As String = "XsrfId"

    Private ReadOnly Property AuthenticationManager() As IAuthenticationManager
        Get
            Return HttpContext.GetOwinContext().Authentication
        End Get
    End Property

    Private Sub AddErrors(result As IdentityResult)
        For Each [error] In result.Errors
            ModelState.AddModelError("", [error])
        Next
    End Sub

    Private Function RedirectToLocal(returnUrl As String) As ActionResult
        If Url.IsLocalUrl(returnUrl) Then
            Return Redirect(returnUrl)
        End If
        Return RedirectToAction("Index", "Home")
    End Function

    Friend Class ChallengeResult
        Inherits HttpUnauthorizedResult
        Public Sub New(provider As String, redirectUri As String)
            Me.New(provider, redirectUri, Nothing)
        End Sub

        Public Sub New(provider As String, redirect As String, user As String)
            LoginProvider = provider
            RedirectUri = redirect
            UserId = user
        End Sub

        Public Property LoginProvider As String
        Public Property RedirectUri As String
        Public Property UserId As String

        Public Overrides Sub ExecuteResult(context As ControllerContext)
            Dim properties = New AuthenticationProperties() With {
                .RedirectUri = RedirectUri
            }
            If UserId IsNot Nothing Then
                properties.Dictionary(XsrfKey) = UserId
            End If
            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider)
        End Sub
    End Class
#End Region
    Private Function RandomString(ByRef Length As String) As String
        Dim str As String = Nothing
        Dim rnd As New Random
        For i As Integer = 0 To Length
            Dim chrInt As Integer = 0
            Do
                chrInt = rnd.Next(30, 122)
                If (chrInt >= 48 And chrInt <= 57) Or (chrInt >= 65 And chrInt <= 90) Or (chrInt >= 97 And chrInt <= 122) Then
                    Exit Do
                End If
            Loop
            str &= Chr(chrInt)
        Next
        str = "ebs_0_" + str + "@"
        Return str
    End Function
End Class
