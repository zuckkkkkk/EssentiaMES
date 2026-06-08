@ModelType IndexViewModel
@Code
    ViewBag.Title = "Gestisci"
End Code

<div class="row MainTitleAccount">
    <div class="col-12">
        <h1 class="TitleAccount">Account</h1>
    </div>
</div>
<div class="TextAccount mt-5 ">
    <i class="fa-solid fa-user fa-6x fa-gradient"></i>
</div>
<div class="TextAccount">
    <h3>@User.Identity.Name.ToString.Split("@")(0)</h3>
</div>
<div class="TextAccount">
    <p>In questa sezione puoi effettuare il logout e cambiare password.</p>
</div>
<p class="text-success">@ViewBag.StatusMessage</p>
<div class="text-center containerLogout">
    <a Class="btn btnLogOut" aria-label="" style="display: inline-block;" href="https://drive.google.com/drive/folders/1J8PTHN1RIm1lJFcMY4N0L1ativkn_MpI?usp=sharing">Documentazione Essentia</a>
</div>


<div class="text-center containerCambioPassword">
    <a Class="btn btnCambioPassword" aria-label="" style="display: inline-block;" href="@Url.Action("ChangePassword", "Manage")">Cambia Password</a>
</div>

<div class="text-center containerLogout">
    <a Class="btn btnLogOut" aria-label="" style="display: inline-block;" href="javascript:document.getElementById('logoutForm').submit()">Logout</a>
</div>


@Using Html.BeginForm("LogOff", "Account", FormMethod.Post, New With {.id = "logoutForm", .class = "navbar-right", .style = "margin: 0!important;"})
    @Html.AntiForgeryToken()
End Using