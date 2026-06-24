@ModelType Brighetti.CruscottoViewModel
@Code
    ViewData("Title") = "Centro di Controllo"
End Code

<style>
    .cruscotto { padding: 22px 18px 60px; }
    .cruscotto h2 { font-weight: 700; letter-spacing: -.5px; }
    .cruscotto .sub { color: #7f8c8d; margin-bottom: 22px; }
    .cruscotto .kpi-row { display: grid; grid-template-columns: repeat(auto-fit, minmax(150px, 1fr)); gap: 12px; margin-bottom: 28px; }
    .cruscotto .kpi { border-radius: 14px; padding: 16px 18px; color: #fff; text-decoration: none; display: block; transition: transform .12s ease; }
    .cruscotto .kpi:hover { transform: translateY(-2px); color: #fff; }
    .cruscotto .kpi .n { font-size: 2.1rem; font-weight: 800; line-height: 1; }
    .cruscotto .kpi .l { font-size: .78rem; text-transform: uppercase; letter-spacing: .04em; opacity: .92; margin-top: 6px; }
    .cruscotto .kpi.red { background: #c0392b; }
    .cruscotto .kpi.amber { background: #cf7a0c; }
    .cruscotto .kpi.slate { background: #2c3e50; }
    .cruscotto .kpi.muted { background: #6c7a89; }
    .cruscotto .kpi.zero { background: #95a5a6; opacity: .6; }
    .cruscotto .panel { background: #fff; border: 1px solid #ececec; border-radius: 14px; padding: 18px 20px; margin-bottom: 22px; }
    .cruscotto .panel h5 { font-weight: 700; margin-bottom: 4px; }
    .cruscotto .panel .hint { color: #95a5a6; font-size: .82rem; margin-bottom: 14px; }
    .cruscotto .flow { display: flex; align-items: stretch; gap: 0; overflow-x: auto; padding: 6px 2px 2px; }
    .cruscotto .stage { flex: 1 1 0; min-width: 104px; display: flex; align-items: stretch; }
    .cruscotto .stage .card { flex: 1; background: #f7f9fa; border: 1px solid #e7ecef; border-radius: 12px; padding: 14px 10px 12px; text-align: center; display: flex; flex-direction: column; }
    .cruscotto .stage .card .cnt { font-size: 1.6rem; font-weight: 800; color: #2c3e50; line-height: 1; }
    .cruscotto .stage .card .nm { font-size: .72rem; color: #5a6b7b; margin-top: 8px; line-height: 1.15; min-height: 2.3em; display: flex; align-items: center; justify-content: center; word-break: break-word; }
    .cruscotto .stage .card .load { height: 5px; border-radius: 3px; background: #e4e9ec; margin-top: 10px; overflow: hidden; }
    .cruscotto .stage .card .load > span { display: block; height: 100%; background: linear-gradient(90deg,#3d566e,#2c3e50); border-radius: 3px; }
    .cruscotto .stage.empty .card { opacity: .55; }
    .cruscotto .stage.empty .card .cnt { color: #9aa7b1; }
    .cruscotto .stage .arrow { color: #cdd6dd; align-self: center; padding: 0 6px; flex: 0 0 auto; }
    .cruscotto .stage:last-child .arrow { display: none; }
    .cruscotto .pager { display: flex; align-items: center; justify-content: flex-end; gap: 10px; margin-top: 12px; }
    .cruscotto .pager span { color: #5a6b7b; font-weight: 600; font-size: .85rem; }
    .cruscotto table { width: 100%; }
    .cruscotto .empty-note { color: #27ae60; font-weight: 600; }
    .cruscotto .badge-fermo { background: #cf7a0c; }
    .cruscotto .badge-bloc { background: #6c7a89; }
</style>

<div class="cruscotto">
    <h2>Centro di Controllo</h2>
    <div class="sub">Stato di produzione, scorte e ordini in un'unica pagina.</div>

    @If TempData("Messaggio") IsNot Nothing Then
        @<div class="alert alert-info">@TempData("Messaggio")</div>
    End If

    <div class="kpi-row">
        <a class="kpi @(If(Model.GiacenzeScoperte > 0, "red", "zero"))" href="#scoperte">
            <div class="n">@Model.GiacenzeScoperte</div>
            <div class="l">Sotto scorta senza ordine</div>
        </a>
        <a class="kpi @(If(Model.OdpFermi > 0, "red", "zero"))" href="#ferme">
            <div class="n">@Model.OdpFermi</div>
            <div class="l">ODP fermi</div>
        </a>
        <a class="kpi @(If(Model.ProposteAperte > 0, "amber", "zero"))" href="@Url.Action("Index", "Brighetti_OrdiniAutomatici")">
            <div class="n">@Model.ProposteAperte</div>
            <div class="l">Proposte da valutare</div>
        </a>
        <a class="kpi @(If(Model.AttivitaStandBy > 0, "amber", "zero"))" href="#ferme">
            <div class="n">@Model.AttivitaStandBy</div>
            <div class="l">Attività sospese</div>
        </a>
        <a class="kpi @(If(Model.OrdiniConfermati > 0, "slate", "zero"))" href="@Url.Action("Index", "Brighetti_OrdiniAutomatici")">
            <div class="n">@Model.OrdiniConfermati</div>
            <div class="l">Ordini in arrivo</div>
        </a>
        <a class="kpi @(If(Model.LottiNonRitornati > 0, "slate", "zero"))" href="@Url.Action("Index", "Brighetti_Lotti")">
            <div class="n">@Model.LottiNonRitornati</div>
            <div class="l">Lotti non ritornati</div>
        </a>
        <a class="kpi muted" href="#ferme">
            <div class="n">@Model.AttivitaBloccate</div>
            <div class="l">Fasi in attesa</div>
        </a>
    </div>

    <div class="panel">
        <h5>Flusso di produzione</h5>
        <div class="hint">Attività attive per reparto: dove si trova il materiale lungo la linea.</div>
        @If Model.Flusso Is Nothing OrElse Model.Flusso.Count = 0 Then
            @<div class="empty-note">Nessun reparto configurato.</div>
        Else
            @<div class="flow">
                @For Each stage In Model.Flusso
                    @<div class="stage @(If(stage.Conteggio = 0, "empty", ""))">
                        <div class="card">
                            <div class="cnt">@stage.Conteggio</div>
                            <div class="nm">@stage.NomeReparto</div>
                            <div class="load"><span style="width:@(CInt(stage.Conteggio / Model.FlussoMax * 100))%;"></span></div>
                        </div>
                        <i class="fa-solid fa-chevron-right arrow"></i>
                    </div>
                Next
            </div>
        End If
    </div>

    <div class="panel" id="scoperte">
        <h5>Giacenze sotto scorta senza ordine in corso</h5>
        <div class="hint">Articoli sotto la scorta minima per cui non esiste già una proposta o un ordine.</div>
        @If Model.Scoperte Is Nothing OrElse Model.Scoperte.Count = 0 Then
            @<div class="empty-note">Tutto coperto: nessun articolo scoperto.</div>
        Else
            @<text>
            <form action="@Url.Action("Genera", "Brighetti_OrdiniAutomatici")" method="post" style="margin-bottom:12px;">
                @Html.AntiForgeryToken()
                <button type="submit" class="btn btn-sm btn-primary"><i class="fa-solid fa-rotate"></i> Genera proposte ora</button>
            </form>
            <table class="table table-sm paged" data-page-size="8">
                <thead><tr><th>Articolo</th><th>Magazzino</th><th>Giacenza</th><th>In arrivo</th><th>Scorta min.</th></tr></thead>
                <tbody>
                @For Each g In Model.Scoperte
                    @<tr>
                        <td>@g.CodiceArticolo</td>
                        <td>@g.CodiceMagazzino</td>
                        <td>@g.QuantitàGiacenza</td>
                        <td>@g.InPrevisioneEntrata</td>
                        <td>@g.QuantitàSottoscorta</td>
                    </tr>
                Next
                </tbody>
            </table>
            </text>
        End If
    </div>

    <div class="panel" id="ferme">
        <h5>Attività ferme</h5>
        <div class="hint">Lavorazioni sospese o bloccate in ordini che non possono avanzare.</div>
        @If Model.AttivitaFerme Is Nothing OrElse Model.AttivitaFerme.Count = 0 Then
            @<div class="empty-note">Nessuna attività ferma.</div>
        Else
            @<table class="table table-sm paged" data-page-size="8">
                <thead><tr><th>ODP</th><th>Articolo</th><th>Fase</th><th>Stato</th><th>Dal</th></tr></thead>
                <tbody>
                @For Each a In Model.AttivitaFerme
                    @<tr>
                        <td>@a.OrdineDiProduzione</td>
                        <td>@a.CodiceArticolo</td>
                        <td>@a.NomeAttività</td>
                        <td>
                            @If a.StatoAttività = TipoStatoAttività.StandBy Then
                                @<span class="badge badge-fermo">Sospesa</span>
                            Else
                                @<span class="badge badge-bloc">Bloccata</span>
                            End If
                        </td>
                        <td>@(If(a.UltimaModifica.Data.HasValue, a.UltimaModifica.Data.Value.ToString("dd/MM/yy HH:mm"), ""))</td>
                    </tr>
                Next
                </tbody>
            </table>
        End If
    </div>

    <div class="panel">
        <h5>Lotti da seguire</h5>
        <div class="hint">Lotti ancora in attesa o inviati (non ancora ritornati).</div>
        @If Model.LottiAperti Is Nothing OrElse Model.LottiAperti.Count = 0 Then
            @<div class="empty-note">Nessun lotto in sospeso.</div>
        Else
            @<table class="table table-sm paged" data-page-size="8">
                <thead><tr><th>Lotto</th><th>ADL/ACL</th><th>Stato</th><th>Aggiornato</th><th></th></tr></thead>
                <tbody>
                @For Each l In Model.LottiAperti
                    @<tr>
                        <td>@l.NomeLotto</td>
                        <td>@l.CodiceADLACL</td>
                        <td>
                            @If l.StatoLotto = StatoLotto.In_Attesa Then
                                @<span class="badge bg-warning text-dark">In attesa</span>
                            Else
                                @<span class="badge bg-primary">Inviato</span>
                            End If
                        </td>
                        <td>@(If(l.UltimaModifica.Data.HasValue, l.UltimaModifica.Data.Value.ToString("dd/MM/yy HH:mm"), ""))</td>
                        <td><a class="btn btn-sm btn-outline-secondary" href="@Url.Action("Index", "Brighetti_Lotti")">Apri</a></td>
                    </tr>
                Next
                </tbody>
            </table>
        End If
    </div>
</div>

<script>
    // Paginazione client-side per le tabelle .paged (evita pagine lunghissime).
    (function () {
        function paginate(table) {
            var size = parseInt(table.getAttribute('data-page-size') || '10', 10);
            var rows = Array.prototype.slice.call(table.querySelectorAll('tbody > tr'));
            if (rows.length <= size) return;
            var pages = Math.ceil(rows.length / size), cur = 0;
            var nav = document.createElement('div');
            nav.className = 'pager';
            var prev = document.createElement('button');
            var next = document.createElement('button');
            var info = document.createElement('span');
            prev.type = next.type = 'button';
            prev.className = next.className = 'btn btn-sm btn-outline-secondary';
            prev.innerHTML = '&laquo; Prec.';
            next.innerHTML = 'Succ. &raquo;';
            function render() {
                for (var i = 0; i < rows.length; i++) {
                    rows[i].style.display = (i >= cur * size && i < (cur + 1) * size) ? '' : 'none';
                }
                info.textContent = (cur + 1) + ' / ' + pages + ' (' + rows.length + ' righe)';
                prev.disabled = cur === 0;
                next.disabled = cur === pages - 1;
            }
            prev.onclick = function () { if (cur > 0) { cur--; render(); } };
            next.onclick = function () { if (cur < pages - 1) { cur++; render(); } };
            nav.appendChild(prev);
            nav.appendChild(info);
            nav.appendChild(next);
            table.parentNode.insertBefore(nav, table.nextSibling);
            render();
        }
        var tables = document.querySelectorAll('table.paged');
        for (var i = 0; i < tables.length; i++) { paginate(tables[i]); }
    })();
</script>
