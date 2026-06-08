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

## 4. Stato delle altre automazioni

Vengono rilasciate a fasi successive (vedi commit dedicati): sincronizzazione
giornaliera con **Mexal**, pianificazione **Hangfire**, creazione automatica dei
lotti e sblocco fase su lotto *Ritornato*. Anche queste resteranno spente finché
non attivate dal pannello.
