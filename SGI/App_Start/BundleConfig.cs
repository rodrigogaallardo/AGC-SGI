using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace SGI
{
    public class BundleConfig
    {
        // Para obtener más información sobre la agrupación de archivos, visite http://go.microsoft.com/fwlink/?LinkId=254726
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                  "~/Scripts/Funciones.js",
                  "~/Scripts/WebForms/WebForms.js",
                  "~/Scripts/WebForms/WebUIValidation.js",
                  "~/Scripts/WebForms/MenuStandards.js",
                  "~/Scripts/WebForms/Focus.js",
                  "~/Scripts/WebForms/GridView.js",
                  "~/Scripts/WebForms/DetailsView.js",
                  "~/Scripts/WebForms/TreeView.js",
                  "~/Scripts/WebForms/WebParts.js",
                  "~/Scripts/Unicorn/bootstrap.js",
                  "~/Scripts/analitycs.js"
                  ));

            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            // Use la versión de desarrollo de Modernizr para desarrollar y aprender. Luego, cuando esté listo
            // para la producción, use la herramienta de creación en http://modernizr.com para elegir solo las pruebas que necesite
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));




            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/Unicorn/jquery.gritter.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/gritter").Include(
                                "~/Scripts/Unicorn/jquery.gritter.js"));

            bundles.Add(new ScriptBundle("~/bundles/Unicorn").Include(
                      "~/Scripts/Unicorn/jquery.ui.custom.js",
                      "~/Scripts/Unicorn/unicorn.js",
                      "~/Scripts/Unicorn/jquery.gritter.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/Unicorn.Tables").Include(
                  "~/Scripts/Unicorn/jquery.dataTables.js",
                  "~/Scripts/Unicorn/unicorn.tables.js",
                  "~/Scripts/Unicorn/select2.js"
                  ));


            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                      "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/fileUpload").Include(
                      "~/Scripts/jquery-fileupload/vendor/jquery.ui.widget.js",
                      "~/Scripts/jquery-fileupload/jquery.iframe-transport.js",
                      "~/Scripts/jquery-fileupload/jquery.fileupload.js"));

            bundles.Add(new StyleBundle("~/bundles/fileUploadCss").Include(
                      "~/Content/themes/jquery-fileupload/jquery.fileupload-ui.css"));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                "~/Scripts/select2.js"));

            bundles.Add(new StyleBundle("~/bundles/select2Css").Include(
                      "~/Content/css/select2.css"));

            bundles.Add(new StyleBundle("~/bundles/iconMoonCss").Include(
                      "~/Content/themes/icon-moon/icon-moon.css"));


            bundles.Add(new ScriptBundle("~/bundles/flot").Include(
                      "~/Scripts/flot/jquery.flot.js",
                      "~/Scripts/flot/jquery.flot.pie.js"
                      ));

            bundles.Add(new StyleBundle("~/bundles/flotCss").Include(
                      "~/Content/jquery-flot/flot.css"));


            bundles.Add(new ScriptBundle("~/bundles/browser").Include(
                      "~/Scripts/browser.js"));

            bundles.Add(new ScriptBundle("~/bundles/autoNumeric").Include(
                      "~/Scripts/autoNumeric/autoNumeric-{version}.js"));

            bundles.Add(new StyleBundle("~/bundles/jqueryCustomCss").Include(
                  "~/Content/themes/base/jquery.ui.custom.css"));

            bundles.Add(new ScriptBundle("~/bundles/timepicker").Include(
                "~/Scripts/jquery.ui.timepicker.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/timepickerCss").Include(
                      "~/Content/themes/timepicker.css"));

            bundles.Add(new ScriptBundle("~/bundles/chartjs").Include(
                    "~/Scripts/Chart.min.js"
                ));
        }
    }
}