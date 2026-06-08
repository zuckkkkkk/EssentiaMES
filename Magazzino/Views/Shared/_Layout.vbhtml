<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <link rel="icon" type="image/x-icon" href="~/Content/Small.ico">
    <meta name="viewport"
          content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <title>Gestione Magazzino</title>
    @Styles.Render("~/Content/css")
    @Scripts.Render("~/bundles/modernizr")
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.2.1/css/all.min.css" integrity="sha512-MV7K8+y+gLIBoVD59lQIYicR65iaqukzvf/nwasF0nqhPay5w/9lJmVM2hMDcnK1OnMGCdVK+iQrJ7lzPJQd1w==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/limonte-sweetalert2/11.7.1/sweetalert2.min.js" integrity="sha512-vCI1Ba/Ob39YYPiWruLs4uHSA3QzxgHBcJNfFMRMJr832nT/2FBrwmMGQMwlD6Z/rAIIwZFX8vJJWDj7odXMaw==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/limonte-sweetalert2/11.7.1/sweetalert2.css" integrity="sha512-JzSVRb7c802/njMbV97pjo1wuJAE/6v9CvthGTDxiaZij/TFpPQmQPTcdXyUVucsvLtJBT6YwRb5LhVxX3pQHQ==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.4/css/dataTables.bootstrap5.min.css" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <link href="//cdnjs.cloudflare.com/ajax/libs/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet">
    <link href="//cdnjs.cloudflare.com/ajax/libs/animate.css/3.2.0/animate.min.css" rel="stylesheet">
    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/datepicker")
</head>
<body>
    @If User.Identity.IsAuthenticated Then
        @If User.IsInRole("Operatore") Then
            @<nav Class="navbar navbar-expand-lg navbar-light bg-light" style="height: 80px; width: 98vw; border-radius: 24px 24px 0px 0px; background: #FFFFFF 0% 0% no-repeat padding-box; box-shadow: 1px -4px 36px 18px #0000000d;">
                <div Class="row w-100">
                    <div Class="col" style="display: flex; justify-content: center;">
                        <a href="@Url.Action("AvvioAttivita", "Brighetti_Operatori")"><i Class="fa-solid fa-house fa-2x @IIf(Html.ViewContext.RouteData.GetRequiredString("action") = "AvvioMacchina", "fa-gradient", "fa-gradient-Not")"></i></a>
                    </div>
                    <div Class="col" style="display: flex; justify-content: center; align-items:center">
                        <img src="~/Content/Logo.png" width="50" />
                    </div>
                    <div Class="col" style="display: flex; justify-content: center;">
                        <a href="@Url.Action("Account", "Manage")"><i Class="fa-solid fa-user fa-2x @IIf(Html.ViewContext.RouteData.GetRequiredString("action") = "Account", "fa-gradient", "fa-gradient-Not")"></i></a>
                    </div>
                </div>
            </nav> ElseIf User.IsInRole("Admin") Then
            @<nav Class="navbar navbar-expand-lg navbar-light bg-light" style="height: 80px; width: 98vw; border-radius: 24px 24px 0px 0px; background: #FFFFFF 0% 0% no-repeat padding-box; box-shadow: 1px -4px 36px 18px #0000000d;">
                <div Class="row w-100">
                    <div Class="col" style="display: flex; justify-content: center; align-items: center">
                        <a href="@Url.Action("AdminDashboard", "Macchine")"><i Class="fa-solid fa-house fa-2x @IIf(Html.ViewContext.RouteData.GetRequiredString("action") = "AdminDashboard", "fa-gradient", "fa-gradient-Not")"></i></a>
                    </div>
                    <div Class="col dropup" style="display: flex; justify-content: center;">
                        <btn><i Class="dropbtn fa-solid fa-cart-shopping fa-2x @IIf(Html.ViewContext.RouteData.GetRequiredString("controller") = "Brighetti_Articolo", "fa-gradient", "fa-gradient-Not")"></i></btn>
                        <div class="dropup-content">
                            <a href="@Url.Action("Index", "Brighetti_Articolo")">Dashboard Articoli</a>
                            <a href="@Url.Action("Index", "Brighetti_Magazzino")">Dashboard Magazzino</a>
                            <a href="@Url.Action("Index", "Brighetti_Giacenza")">Dashboard Giacenza</a>
                            <a href="@Url.Action("Index", "Brighetti_Lotti")">Dashboard Lotti</a>
                        </div>
                    </div>
                    <div Class="col" style="display: flex; justify-content: center; align-items:center">
                        <img src="~/Content/Logo.png" width="50" />
                    </div>
                    <div Class="col dropup" style="display: flex; justify-content: center;">
                        <btn><i Class="dropbtn fa-solid fa-industry fa-2x @IIf(Html.ViewContext.RouteData.GetRequiredString("controller") = "Brighetti_Macchina", "fa-gradient", "fa-gradient-Not")"></i></btn>
                        <div class="dropup-content">
                            <a href="@Url.Action("Index", "Brighetti_Reparto")">Dashboard Reparti</a>
                            <a href="@Url.Action("Index", "Brighetti_Macchina")">Dashboard Macchina</a>
                            <a href="@Url.Action("Storico", "Home")">Dashboard Utenti</a>
                        </div>
                    </div>
                    <div Class="col" style="display: flex; justify-content: center; align-items:center">
                        <a href="@Url.Action("Account", "Manage")"><i Class="fa-solid fa-user fa-2x @IIf(Html.ViewContext.RouteData.GetRequiredString("action") = "Account", "fa-gradient", "fa-gradient-Not")"></i></a>
                    </div>
                </div>
            </nav>End If
    End If
    <div Class="container body-content">
        @RenderBody()

    </div>


    @Scripts.Render("~/bundles/notify")
    <script src="https://cdn.datatables.net/1.13.4/js/jquery.dataTables.min.js" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script src="https://cdn.datatables.net/1.13.4/js/dataTables.bootstrap5.min.js" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script src="~/Scripts/dataTables.fixedHeader.min.js"></script>
    @Scripts.Render("~/bundles/app")
    @RenderSection("scripts", required:=False)
</body>
</html>
