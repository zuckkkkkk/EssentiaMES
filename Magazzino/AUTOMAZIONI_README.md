# EssentiaMES – Automazioni

Questo documento descrive le funzionalità di automazione aggiunte al gestionale e
come metterle in esercizio. Tutti gli automatismi nascono **spenti** e si attivano
**uno alla volta** dal pannello *Automazioni* (icona robot nel menu Admin).

## 1. Aggiornamento del database (obbligatorio)

Le nuove funzioni aggiungono campi e tabelle. Prima di usarle occorre allineare il DB.

**Opzione A – script SQL (consigliata se non si usa Visual Studio).**
Eseguire `Magazzino/AGGIORNAMENTO_DB.sql` sul database puntato dalla connection
string `BrighettiModels`. Lo script è idempotente (eseguibile più volte).

**Opzione B – migration EF6 (da Visual Studio, Package Manager Console).**
```
Add-Migration Automazioni -ConfigurationTypeName Brighetti.BrighettiMigrations.Configuration
Update-Database          -ConfigurationTypeName Brighetti.BrighettiMigrations.Configuration
```

> Nota: in `Global.asax` è stato disattivato il controllo di compatibilità del
> modello (`Database.SetInitializer(Of BrighettiModels)(Nothing)`), così
> l'applicazione non va in errore se il codice viene pubblicato prima della
> migrazione. Le query sulle nuove tabelle falliranno finché il DB non è aggiornato.

## 2. Cosa è stato aggiunto

### Campi anagrafici
- **Articolo → Lotto Minimo**: quantità minima/multiplo per gli ordini automatici.
- **Giacenza → Scorta Massima**: obiettivo di riordino (oltre alla scorta minima già presente).
- **Lotto → ADL/ACL**: campo di tracciabilità nel lotto.

### Pannello Automazioni (`/Brighetti_Impostazioni`)
Interruttori e parametri salvati in tabella `Brighetti_Impostazioni`. Gli
interruttori hanno effetto immediato; i parametri si salvano col pulsante dedicato.

### Ordini automatici (`/Brighetti_OrdiniAutomatici`)
Genera **proposte di riordino** per gli articoli sotto scorta minima:
- filtra per prefisso codice (di default **G8**, come da test concordato);
- quantità proposta = (scorta massima, oppure *moltiplicatore* × scorta minima) − giacenza − in arrivo;
- la quantità viene arrotondata per eccesso al **lotto minimo** dell'articolo;
- evita doppioni (una sola proposta aperta per articolo/magazzino);
- le proposte sono **Proposto → Confermato/Annullato** (nessun invio automatico).

## 3. Come attivare il riordino automatico su G8 (procedura consigliata)

1. Aggiornare il DB (par. 1).
2. Censire per gli articoli **G8**: *Lotto Minimo* (anagrafica articolo) e
   *Scorta Minima*/*Scorta Massima* (giacenza).
3. Aprire **Automazioni** e, nella sezione *Ordini Automatici*, lasciare
   `prefissi_codice = G8` e impostare il moltiplicatore (es. 2 = doppio della scorta minima).
4. In **Ordini Automatici** premere **“Genera proposte ora”** e verificare i risultati.
5. Quando il comportamento è corretto, attivare l'interruttore
   *“ordini automatici – abilitato”* per la generazione pianificata.

## 4. Pianificazione (Hangfire)

All'avvio dell'applicazione (`Global.asax`) viene configurato Hangfire (usa la
connessione `BrighettiModels`, crea da sé le proprie tabelle `HangFire.*`) e
vengono registrati due job ricorrenti:

- **ordini-automatici**: genera le proposte di riordino ogni N minuti
  (`scheduler.minuti_intervallo_ordini`, default 60);
- **sync-mexal**: sincronizza le giacenze una volta al giorno
  (`scheduler.ora_sync_giornaliera`, default 02:00).

Entrambi rispettano i rispettivi interruttori: se l'automatismo è spento, il job
parte ma non fa nulla. L'avvio di Hangfire è protetto: se fallisce (es. DB non
pronto), l'app web continua a funzionare e gli automatismi restano attivabili a
mano. Il job viene eseguito nel processo IIS: per la massima affidabilità tenere
l'application pool sempre attivo (oppure, in futuro, spostare i job in un servizio
dedicato). La *dashboard* Hangfire è opzionale e richiede OWIN: per abilitarla
aggiungere `app.UseHangfireDashboard("/hangfire")` nel proprio `Startup.vb`
(file non incluso nel repo perché in `.gitignore`).

## 5. Sincronizzazione Mexal (`/Brighetti_Sync`)

Aggiorna le giacenze interne dal gestionale. È predisposta come sorgente
*plug-in* (`IMexalSource`):

- **CSV/file** (implementato): legge un file esportato da Mexal; colonne
  riconosciute dall'intestazione (`Articolo`, `Magazzino`, `Giacenza/Quantità`).
  Percorso e separatore si impostano nel pannello Automazioni.
- **Import manuale**: dalla pagina si può caricare un CSV al volo.
- **API/DB Mexal** (da definire): quando sarà nota la modalità, basterà
  aggiungere una nuova implementazione di `IMexalSource` senza toccare il resto.

## 6. Gestione lotti automatica

- **Creazione automatica**: alla chiusura di una fase di lavorazione il sistema
  crea un lotto (stato *In attesa*) con l'articolo/quantità versati
  (interruttore *lotti – creazione automatica*).
- **Sblocco su "Ritornato"**: quando un lotto passa a *Ritornato*, viene
  sbloccata la fase successiva per gli articoli del lotto (interruttore
  *lotti – ritornato sblocca fase*). Per prudenza lo sblocco avviene solo quando
  è univoco (una sola attività bloccata per quel codice articolo); i casi
  ambigui vengono annotati nel log e lasciati alla gestione manuale.

## 7. Manutenzione dati (pannello Automazioni)

Nella pagina Automazioni è presente una sezione **Manutenzione dati**:

- **Lotto minimo di massa**: assegna lo stesso lotto minimo a tutti gli articoli
  con un certo prefisso codice (es. tutti i `G8` a 50, i `G12` a 100).
- **Cancellazione richieste aperte**: elimina le richieste materiali aperte
  (tabella `OrdiniInCorso`, i "carrelli" non inviati) mantenendo lo storico
  delle richieste già inviate (`Ordini`). Operazione con conferma e tracciata a
  log (Audit).

## 8. Note e voci non incluse

- **Workflow a fasi da tablet**, **export Excel lotti**, **stati lotto
  Inviato/Ritornato**, **avanzamento progressivo lavorazione→magazzino**: già
  presenti e funzionanti (verificati). L'"ordine a catena" tra reparti è proprio
  questo flusso progressivo, non richiede distinta base.
- **Ordini ricorsivi per semilavorati**: coperti dal motore di riordino, che
  considera anche i semilavorati (interruttore dedicato).
- **Sincronizzazione Mexal reale (API/DB)**: sospesa per scelta; resta la
  struttura plug-in (CSV/import manuale) spenta, pronta per il futuro.
- **Tipi ODP parziale/personalizzato**, **coda di lavoro per priorità**: non
  inclusi in questa fase (richiedono ulteriori decisioni di processo).
