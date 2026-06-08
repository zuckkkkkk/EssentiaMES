Imports System.Web.Optimization

Public Module BundleConfig
    ' Per altre informazioni sulla creazione di bundle, vedere https://go.microsoft.com/fwlink/?LinkId=301862
    Public Sub RegisterBundles(ByVal bundles As BundleCollection)

        bundles.Add(New ScriptBundle("~/bundles/jquery").Include(
                    "~/Scripts/jquery-{version}.js"))

        bundles.Add(New ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Scripts/jquery.validate*"))

        ' Utilizzare la versione di sviluppo di Modernizr per eseguire attività di sviluppo e formazione. Successivamente, quando si è
        ' pronti per passare alla produzione, usare lo strumento di compilazione disponibile all'indirizzo https://modernizr.com per selezionare solo i test necessari.
        bundles.Add(New ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/modernizr-*"))
        bundles.Add(New ScriptBundle("~/bundles/datepicker").Include(
                    "~/Scripts/datepicker.js"))
        bundles.Add(New ScriptBundle("~/bundles/app").Include(
                  "~/Scripts/app.js"))
        bundles.Add(New ScriptBundle("~/bundles/dashboard").Include(
                  "~/Scripts/dashboard.js"))
        bundles.Add(New StyleBundle("~/Content/css").Include(
                  "~/Content/fixedHeader.dataTables.min.css",
                  "~/Content/css/Datepicker_A.css",
                  "~/Content/site.css"))
        bundles.Add(New ScriptBundle("~/bundles/notify").Include(
                  "~/Scripts/bootstrap.notify.js"))
    End Sub
End Module

