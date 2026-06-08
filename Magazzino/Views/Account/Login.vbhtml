@ModelType LoginViewModel
@Code
    ViewBag.Title = "Accedi"
End Code
@Styles.Render("~/Content/css")
<style>
    body {
        overflow: hidden!important; /* Hide scrollbars */
    }
</style>
<div class="spiegazioni" style="position: absolute; left: 16px; top: 16px; z-index:10000; width: 400px;">
    <p>Build: 09/01/2023 - V 0.3.0</p>
</div>
<div class="row LoginContainer">
    <div class="col-md-8">
        <section class="card cardLogin" style="box-shadow: none!important">
            <div class="text-center">
                <img src="~/Content/Small.png" width="200" style="margin:32px"/>
            </div>
            @Using Html.BeginForm("Login", "Account", New With {.ReturnUrl = ViewBag.ReturnUrl}, FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
                @Html.AntiForgeryToken()
                @<text>
                    <div class="inputContainer">
                        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
                        <div class="row my-3">
                            <div class="col-12 inputLoginContainer">
                                @Html.TextBoxFor(Function(m) m.username, New With {.class = "form-control inputLogin", .placeholder = "Username"})
                            </div>
                        </div>
                        <div class="row my-3">
                            <div class="col-12 inputLoginContainer">
                                @Html.PasswordFor(Function(m) m.Password, New With {.class = "form-control inputLogin", .placeholder = "Password"})
                            </div>
                        </div>
                        <div class="row mt-5 mb-4">
                            <div class="col-12 text-center">
                                <input type="submit" value="Accedi" class="btn btnRichiesta" />
                            </div>
                        </div>
                    </div>
                </text>
            End Using
        </section>
    </div>
</div>
<p style="position: absolute; bottom: 16px; right: 16px; margin: 0!important;">Build: @ViewBag.BuildDate - V.@Costanti.WebAppVersion</p>
@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
