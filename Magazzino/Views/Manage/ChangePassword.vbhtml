@ModelType ChangePasswordViewModel
@Code
    ViewBag.Title = "Cambia password"
End Code
<div class="row MainTitleCambiaPassword">
<div class="col-12">
    <h1 class="TitleCambiaPassword">Cambia Password</h1>
</div>
</div>
@Using Html.BeginForm("ChangePassword", "Manage", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
    @Html.AntiForgeryToken()

    @<text>
        @Html.ValidationSummary("", New With {.class = "text-danger"})
        <div class="form-group">
            <div class="col-12 CenteringInput">
                @Html.PasswordFor(Function(m) m.OldPassword, New With {.class = "form-control InputCambiaPassword", .placeholder = "Vecchia Password..."})
            </div>
        </div>
        <div class="form-group">
            <div class="col-12 CenteringInput">
                @Html.PasswordFor(Function(m) m.NewPassword, New With {.class = "form-control InputCambiaPassword", .placeholder = "Nuova Password..."})
            </div>
        </div>
        <div class="form-group">
            <div class="col-12 CenteringInput">
                @Html.PasswordFor(Function(m) m.ConfirmPassword, New With {.class = "form-control InputCambiaPassword", .placeholder = "Conferma Password..."})
            </div>
        </div>
        <div class="text-center">
            <input type="submit" value="Cambia password" class="btn btnInvioCambioPassword" />
        </div>
    </text>
End Using

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section