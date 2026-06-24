@ModelType IEnumerable(Of Brighetti.ImpostazioniCategoriaViewModel)
@Code
    ViewData("Title") = "Automazioni"
End Code

<div class="container" style="margin-top:24px;">
    <h2>Automazioni e impostazioni</h2>
    <p class="text-muted">
        Attiva gli automatismi <strong>uno alla volta</strong> con gli interruttori (effetto immediato).
        I parametri di configurazione si salvano con il pulsante in fondo.
    </p>

    @If TempData("Messaggio") IsNot Nothing Then
        @<div class="alert alert-success">@TempData("Messaggio")</div>
    End If

    @Using (Html.BeginForm("Salva", "Brighetti_Impostazioni"))
        @Html.AntiForgeryToken()
        @For Each gruppo In Model
            @<div class="card" style="margin-bottom:18px;">
                <div class="card-header"><strong>@gruppo.Categoria</strong></div>
                <div class="card-body">
                    @For Each imp In gruppo.Impostazioni
                        @<div class="row" style="padding:8px 0; border-bottom:1px solid #f0f0f0;">
                            <div class="col-md-7">
                                <div>@imp.Descrizione</div>
                                <small class="text-muted">@imp.Chiave</small>
                            </div>
                            <div class="col-md-5">
                                @If imp.Tipo = TipoImpostazione.Booleano Then
                                    @<div class="form-check form-switch">
                                        <input class="form-check-input toggle-automazione" type="checkbox"
                                               role="switch" id="imp_@imp.Id" data-id="@imp.Id"
                                               @(If(imp.Valore IsNot Nothing AndAlso imp.Valore.ToLower() = "true", "checked", ""))>
                                        <label class="form-check-label" for="imp_@imp.Id">Attivo</label>
                                    </div>
                                ElseIf imp.Tipo = TipoImpostazione.Numero Then
                                    @<input type="number" step="any" class="form-control" name="imp_@imp.Id" value="@imp.Valore" />
                                Else
                                    @<input type="text" class="form-control" name="imp_@imp.Id" value="@imp.Valore" />
                                End If
                            </div>
                        </div>
                    Next
                </div>
            </div>
        Next

        @<div style="margin:18px 0;">
            <button type="submit" class="btn btn-primary">Salva parametri</button>
        </div>
    End Using

    <div class="card" style="margin-bottom:18px;">
        <div class="card-header"><strong>Manutenzione dati</strong></div>
        <div class="card-body">
            <h6>Lotto minimo di massa</h6>
            <p class="text-muted">Imposta lo stesso lotto minimo a tutti gli articoli con un certo prefisso codice (es. tutti i G8 a 50).</p>
            @Using (Html.BeginForm("AggiornaLottoMinimoMassivo", "Brighetti_Impostazioni"))
                @Html.AntiForgeryToken()
                @<div class="row">
                    <div class="col-md-4">
                        <input type="text" name="prefisso" class="form-control" placeholder="Prefisso codice (es. G8)" />
                    </div>
                    <div class="col-md-4">
                        <input type="number" name="valore" class="form-control" placeholder="Lotto minimo" value="0" />
                    </div>
                    <div class="col-md-4">
                        <button type="submit" class="btn btn-outline-primary">Applica a tutti</button>
                    </div>
                </div>
            End Using

            <hr />

            <h6>Cancellazione richieste aperte</h6>
            <p class="text-muted">Elimina tutte le richieste materiali ancora aperte/in sospeso (carrelli non inviati). Le richieste già inviate (storico) vengono mantenute.</p>
            @Using (Html.BeginForm("CancellaRichiesteAperte", "Brighetti_Impostazioni", FormMethod.Post, New With {.onsubmit = "return confirm('Eliminare TUTTE le richieste aperte? Lo storico delle richieste inviate resta. Operazione non reversibile.');"}))
                @Html.AntiForgeryToken()
                @<button type="submit" class="btn btn-outline-danger">Elimina richieste aperte</button>
            End Using
        </div>
    </div>

    <script>
        document.querySelectorAll('.toggle-automazione').forEach(function (el) {
            el.addEventListener('change', function () {
                var id = this.getAttribute('data-id');
                var checkbox = this;
                var token = document.querySelector('input[name="__RequestVerificationToken"]');
                var body = 'id=' + encodeURIComponent(id);
                if (token) { body += '&__RequestVerificationToken=' + encodeURIComponent(token.value); }
                fetch('@Url.Action("Toggle", "Brighetti_Impostazioni")', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                    body: body
                }).then(function (r) { return r.json(); })
                  .then(function (data) {
                      if (!data.ok) {
                          checkbox.checked = !checkbox.checked;
                          if (window.Swal) { Swal.fire('Errore', data.messaggio || 'Operazione non riuscita', 'error'); }
                      }
                  }).catch(function () {
                      checkbox.checked = !checkbox.checked;
                  });
            });
        });
    </script>
</div>
