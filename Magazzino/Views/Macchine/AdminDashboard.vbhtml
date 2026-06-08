@ModelType List(Of Brighetti.Brighetti_Macchina_ViewModel)
@Code
    ViewData("Title") = "Dashboard Admin"
End Code

<div class="row MainTitleAccount" style="margin: 0!important; padding: 0!important;">
    <div class="col-4" style="margin: 0 !important; padding: 0!important;">
        <h1 class="TitleAccount">Dashboard Admin</h1>
    </div>
    <div class="col-md-8">
        <ul class="nav nav-pills mb-3" id="pills-tab" role="tablist" style="float: right!important;">
            <li class="nav-item" role="presentation">
                <button class="nav-link active" id="pills-dashboard-tab" data-bs-toggle="pill" data-bs-target="#pills-dashboard" type="button" role="tab" aria-controls="pills-dashboard" aria-selected="true" onclick="closeOpenPopovers()">
                    Dashboard
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link position-relative" id="pills-stati-tab" data-bs-toggle="pill" data-bs-target="#pills-stati" type="button" role="tab" aria-controls="pills-stati" aria-selected="false" onclick="closeOpenPopovers()">
                    Stati Macchina
                </button>
            </li>
        </ul>
    </div>
</div>

<div class="tab-content" id="pills-tabContent">
    <div class="tab-pane fade show active" id="pills-dashboard" role="tabpanel" aria-labelledby="pills-dashboard-tab">
        <div class="row">
            <div class="planimetriaCenter" style="position: relative!important;">
                <div class='clickable'></div>
                @For Each pinpoint In Model
                    @<img src="@IIf(pinpoint.MacchinaAccesa, Html.Raw("/Content/assets/PinpointImageAcceso.png"), Html.Raw("/Content/assets/PinpointImage.png"))"
                          id=@pinpoint.IdMacchina
                          data-bs-toggle="popover"
                          data-bs-placement="top"
                          data-bs-html="true"
                          class="classPinPoint"
                          data-bs-content=" "
                          title=@pinpoint.NomeMacchina
                          style="position:absolute;width: 50px; height: 50px; left: @Html.Raw(pinpoint.posX)%; top: @Html.Raw(pinpoint.posY)%;">
                Next
            </div>
            <div class="col-3">
                <button id="visualizzaButton" class="buttonNotSelected col-11" onclick="buttonSelect('visualizza')">  <i class="fa-solid fa-eye"></i> </button>@*Visualizza*@
            </div>
            <div class="col-3">
                <button id="modificaButton" class="buttonNotSelected col-12" onclick="buttonSelect('modifica')">  <i class="fa-solid fa-screwdriver-wrench"></i> </button> @*Modifica*@
            </div>

        </div>
       
    </div>
    <div class="tab-pane fade" id="pills-stati" role="tabpanel" aria-labelledby="pills-stati-tab">
        @Html.Action("MacchineStati")
    </div>
</div>




@Scripts.Render("~/bundles/dashboard")