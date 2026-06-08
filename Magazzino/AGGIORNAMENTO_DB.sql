/* ============================================================================
   EssentiaMES - Aggiornamento DB per gli automatismi
   ----------------------------------------------------------------------------
   Aggiunge i campi e le tabelle introdotti dalle nuove funzionalità:
     - Brighetti_Articoli.LottoMinimo            (lotto minimo per articolo)
     - Brighetti_Giacenze.QuantitàScortaMassima  (scorta massima)
     - Brighetti_Lotti.CodiceADLACL              (campo ADL/ACL nel lotto)
     - Brighetti_Impostazioni                    (impostazioni / interruttori automatismi)
     - Brighetti_OrdiniAutomatici                (proposte di riordino automatico)

   Lo script è IDEMPOTENTE: può essere eseguito più volte senza errori.
   Eseguirlo sul database puntato dalla connection string "BrighettiModels".

   NOTA EF6: questo script NON aggiorna la tabella [__MigrationHistory]. Va bene
   perché in Application_Start il controllo di compatibilità del modello è
   disattivato (Database.SetInitializer(Of BrighettiModels)(Nothing)).
   In alternativa, da Visual Studio (Package Manager Console) si può usare:
       Add-Migration Automazioni -ConfigurationTypeName Brighetti.BrighettiMigrations.Configuration
       Update-Database          -ConfigurationTypeName Brighetti.BrighettiMigrations.Configuration
   ============================================================================ */

SET NOCOUNT ON;
GO

/* ---- Brighetti_Articoli.LottoMinimo -------------------------------------- */
IF COL_LENGTH('dbo.Brighetti_Articoli', 'LottoMinimo') IS NULL
BEGIN
    ALTER TABLE dbo.Brighetti_Articoli
        ADD [LottoMinimo] INT NOT NULL CONSTRAINT [DF_Brighetti_Articoli_LottoMinimo] DEFAULT(0);
END
GO

/* ---- Brighetti_Giacenze.QuantitàScortaMassima --------------------------- */
IF COL_LENGTH('dbo.Brighetti_Giacenze', 'QuantitàScortaMassima') IS NULL
BEGIN
    ALTER TABLE dbo.Brighetti_Giacenze
        ADD [QuantitàScortaMassima] DECIMAL(18, 2) NOT NULL
            CONSTRAINT [DF_Brighetti_Giacenze_QuantitaScortaMassima] DEFAULT(0);
END
GO

/* ---- Brighetti_Lotti.CodiceADLACL --------------------------------------- */
IF COL_LENGTH('dbo.Brighetti_Lotti', 'CodiceADLACL') IS NULL
BEGIN
    ALTER TABLE dbo.Brighetti_Lotti
        ADD [CodiceADLACL] NVARCHAR(MAX) NULL;
END
GO

/* ---- Brighetti_Impostazioni --------------------------------------------- */
IF OBJECT_ID('dbo.Brighetti_Impostazioni', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Brighetti_Impostazioni(
        [Id]                          INT IDENTITY(1,1) NOT NULL,
        [Chiave]                      NVARCHAR(MAX) NULL,
        [Valore]                      NVARCHAR(MAX) NULL,
        [Descrizione]                 NVARCHAR(MAX) NULL,
        [Categoria]                   NVARCHAR(MAX) NULL,
        [Tipo]                        TINYINT NOT NULL,
        [UltimaModifica_OperatoreID]  NVARCHAR(MAX) NULL,
        [UltimaModifica_Operatore]    NVARCHAR(MAX) NULL,
        [UltimaModifica_Data]         DATETIME NULL,
        CONSTRAINT [PK_dbo.Brighetti_Impostazioni] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

/* ---- Brighetti_OrdiniAutomatici ----------------------------------------- */
IF OBJECT_ID('dbo.Brighetti_OrdiniAutomatici', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Brighetti_OrdiniAutomatici(
        [Id]                          INT IDENTITY(1,1) NOT NULL,
        [CodiceArticolo]              NVARCHAR(MAX) NULL,
        [CodiceMagazzino]             NVARCHAR(MAX) NULL,
        [QuantitàGiacenza]            DECIMAL(18, 2) NOT NULL,
        [ScortaMinima]                DECIMAL(18, 2) NOT NULL,
        [ScortaMassima]               DECIMAL(18, 2) NOT NULL,
        [LottoMinimo]                 INT NOT NULL,
        [QuantitàProposta]            DECIMAL(18, 2) NOT NULL,
        [Stato]                       TINYINT NOT NULL,
        [Origine]                     NVARCHAR(MAX) NULL,
        [Note]                        NVARCHAR(MAX) NULL,
        [DataGenerazione]             DATETIME NOT NULL,
        [UltimaModifica_OperatoreID]  NVARCHAR(MAX) NULL,
        [UltimaModifica_Operatore]    NVARCHAR(MAX) NULL,
        [UltimaModifica_Data]         DATETIME NULL,
        CONSTRAINT [PK_dbo.Brighetti_OrdiniAutomatici] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

PRINT 'Aggiornamento DB EssentiaMES completato.';
GO
