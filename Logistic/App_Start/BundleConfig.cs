using System.Web;
using System.Web.Optimization;

namespace Logistic
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
			System.Web.Optimization.BundleTable.EnableOptimizations = false;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.3.1.min.js",
                        "~/Scripts/jquery-3.3.1.js",
                        "~/Scripts/jquery.slimscroll.min.js",
                        "~/Scripts/jquery.blockUI.js",
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                       "~/Scripts/fastclick.js",
                       "~/Scripts/adminlte.min.js",
                       "~/Scripts/demo.js",
                       "~/Scripts/main.js",
                       "~/Scripts/datetimepicker/bootstrap-timepicker.min.js",
                       "~/Scripts/datetimepicker/bootstrap-datepicker.min.js",
                       "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/ionicons.min.css",
                      "~/Content/plugins/font-awesome-4.7.0/css/font-awesome.min.css",
                      "~/Content/plugins/timepicker/bootstrap-timepicker.min.css",
                      "~/Content/dist/css/AdminLTE.min.css",
                      "~/Content/dist/css/skins/_all-skins.min.css",
                      "~/Content/bootstrap-datepicker.min.css",
                      "~/Content/ionicons.min.css",
                      "~/Content/site.css"));
        }
    }
}
