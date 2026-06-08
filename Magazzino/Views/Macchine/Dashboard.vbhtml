@ModelType IEnumerable(Of Brighetti.Macchine)
@Code
    ViewData("Title") = "Dashboard"
End Code

<div class="row MainTitleAccount">
    <div class="col-12">
        <h1 class="TitleAccount">Dashboard</h1>
    </div>
</div>
@If Model.Count = 0 Then
    @<div class="MissingItemsCarrello mt-5 ">
        <i class="fa-solid fa-wrench fa-6x fa-gradient"></i>
    </div>
    @<div class="MissingItemsCarrello">
        <h3>Dashboard vuota!</h3>
    </div>
    @<div class="MissingItemsCarrello">
        <p>Pare che tu non abbia nessuna macchina associata! Richiedi al tuo responsabile l'associazione per poter segnare i dati aggiuntivi.</p>
    </div>
End If
    <div class="row">
        @For Each item In Model
                @<div Class="col-sm-4" style="display: flex; justify-content: center; align-items: center;">
                    <div class="card cardArticolo">
                        <div class="row TestoArticolo">
                            <div class="col-9">
                                <h4> @item.NomeMacchina</h4>
                                <p> @item.DescrizioneMacchina</p>
                                <a href="@Url.Action("Istanza", "Macchine", New With {.id = item.Id})" class="stretched-link"></a>
                            </div>
                        </div>
                    </div>
                </div>
        Next
    </div>
