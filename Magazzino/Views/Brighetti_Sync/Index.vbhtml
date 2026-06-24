@Code
    ViewData("Title") = "Sincronizzazione Mexal"
End Code

<div class="container" style="margin-top:24px;">
    <h2>Sincronizzazione giacenze Mexal</h2>

    <p>
        Stato automatismo:
        @If ViewBag.Abilitato Then
            @<span class="badge bg-success">ATTIVO</span>
        Else
            @<span class="badge bg-secondary">SPENTO</span>
        End If
        &nbsp; Sync giornaliera alle <code>@ViewBag.Ora</code>
        &nbsp; <a href="@Url.Action("Index", "Brighetti_Impostazioni")">Configura nel pannello Automazioni</a>
    </p>
    <p>File CSV configurato: <code>@(If(String.IsNullOrEmpty(ViewBag.Percorso), "(non impostato)", ViewBag.Percorso))</code></p>

    @If TempData("Messaggio") IsNot Nothing Then
        @<div class="alert alert-info">@TempData("Messaggio")</div>
    End If

    <div class="card" style="margin-bottom:18px;">
        <div class="card-header"><strong>Esecuzione dal file configurato</strong></div>
        <div class="card-body">
            <p class="text-muted">Legge il file CSV indicato nel pannello Automazioni e aggiorna le giacenze.</p>
            @Using (Html.BeginForm("EseguiOra", "Brighetti_Sync"))
                @Html.AntiForgeryToken()
                @<button type="submit" class="btn btn-primary">
                    <i class="fa-solid fa-rotate"></i> Sincronizza ora
                </button>
            End Using
        </div>
    </div>

    <div class="card">
        <div class="card-header"><strong>Import manuale di un file CSV</strong></div>
        <div class="card-body">
            <p class="text-muted">
                Carica un file esportato da Mexal. Colonne riconosciute (dall'intestazione):
                <code>Articolo</code>, <code>Magazzino</code>, <code>Giacenza/Quantità</code>.
            </p>
            @Using (Html.BeginForm("Importa", "Brighetti_Sync", FormMethod.Post, New With {.enctype = "multipart/form-data"}))
                @Html.AntiForgeryToken()
                @<div class="row">
                    <div class="col-md-8">
                        <input type="file" name="file" accept=".csv,.txt" class="form-control" />
                    </div>
                    <div class="col-md-4">
                        <button type="submit" class="btn btn-outline-primary">Importa e sincronizza</button>
                    </div>
                </div>
            End Using
        </div>
    </div>
</div>
