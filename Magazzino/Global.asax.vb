Imports System.Data.Entity
Imports System.Web.Optimization
Imports System.Web.Security
Imports System.Web.SessionState

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Protected Sub Application_Start()
        ' Lo schema è gestito tramite migration/script: disattiviamo il controllo
        ' di compatibilità del modello per evitare l'errore "model backing context changed"
        ' quando si aggiungono campi/tabelle prima di aver migrato il DB.
        Database.SetInitializer(Of BrighettiModels)(Nothing)

        AreaRegistration.RegisterAllAreas()
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)

        ' Scheduler degli automatismi (Hangfire). Avvio protetto: eventuali errori
        ' non bloccano l'avvio dell'applicazione web.
        SchedulerAutomazioni.Configura()
    End Sub
End Class
