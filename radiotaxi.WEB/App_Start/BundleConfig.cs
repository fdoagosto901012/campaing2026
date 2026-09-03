using System.Web;
using System.Web.Optimization;

namespace radiotaxi.WEB
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.Add(new StyleBundle("~/Content/font-awesome/css").Include(
          "~/Content/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/login/css").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/bootstrap-responsive.min.css",
                "~/Content/matrix-login.css"));


            bundles.Add(new StyleBundle("~/Content/matrix/css").Include(
                    "~/Content/bootstrap.min.css",
                    "~/Content/bootstrap-responsive.min.css",
                    "~/Content/datepicker.css",
                    "~/Content/wickedpicker.css",
                    "~/Content/fullcalendar.css",
                    "~/Content/uniform.css",
                    "~/Content/select2.css",
                    "~/Content/matrix-style.css",
                    "~/Content/matrix-media.css",
                    "~/Content/bootstrap-wysihtml5.css",
                    "~/Content/jquery.gritter.css",
                    "~/Content/matrix-custom.css"));



            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            

            
            bundles.Add(new ScriptBundle("~/bundles/jquery-custom").Include(
                      "~/Scripts/select2.min.js",
                      "~/Scripts/jquery-ui.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/fullCalendar").Include(
                        "~/Scripts/moment.min.js",
                        "~/Scripts/gcal.min.js",
                        "~/Scripts/fullcalendar.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/last10").Include(
                      "~/Scripts/lastServices.js"));

            bundles.Add(new ScriptBundle("~/bundles/Dynamic").Include(
                      "~/Scripts/DynamicElement.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/fullcalendar.css",
                      "~/Content/wickedpicker.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/fullcalendar.css",
                      "~/Content/wickedpicker.css",
                      "~/Content/site.css"));


            bundles.Add(new StyleBundle("~/Content/dualCalendarCss").Include(
                      "~/Content/daterangepicker.css"));

            
            // Templates sources

            BundleTable.EnableOptimizations = false;
            /*
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.min.js",
                        "~/Scripts/jquery.validate*"));*/

            bundles.Add(new ScriptBundle("~/bundles/matrix-Jquery").Include(
                        "~/Scripts/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/matrix").Include(
                        "~/Scripts/select2.min.js",
                        "~/Scripts/jquery.ui.custom.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/matrix.form_validation.js",
                        "~/Scripts/jquery.wizard.js",
                        "~/Scripts/jquery.uniform.js",
                        "~/Scripts/jquery.dataTables.min.js",
                        "~/Scripts/fullcalendar.min.js",
                        "~/Scripts/matrix.js",
                        "~/Scripts/matrix.calendar.js",
                        "~/Scripts/matrix.tables.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                        "~/Scripts/jquery.min.js",
                        "~/Scripts/matrix.login.js"));

            bundles.Add(new ScriptBundle("~/bundles/DynamicElement").Include(
                        "~/Scripts/DynamicElement.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/customScript").Include(
                        "~/Scripts/scripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/dualCalendar").Include(
                        "~/Scripts/jquery.min.dualCalendarjs.js",
                        "~/Scripts/moment.min.js",
                        "~/Scripts/daterangepicker.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/formBundle").Include(
                        "~/Scripts/masked.js",
                        "~/Scripts/select2.min.js",
                        "~/Scripts/bootstrap-colorpicker.js",
                        "~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/wickedpicker.js",
                        "~/Scripts/jquery.toggle.buttons.js",
                        "~/Scripts/matrix.form_common.js",
                        "~/Scripts/wysihtml5-0.3.0.js",
                        "~/Scripts/jquery.peity.min.js",
                        "~/Scripts/bootstrap-wysihtml5.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/validate").Include(
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtruvise.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/customScript").Include(
                       "~/Scripts/scripts.js"));
        }
    }
}
