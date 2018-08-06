using System.Web;
using System.Web.Optimization;

namespace QLSL
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapjs").Include(
                "~/Scripts/bootstrap.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/modalFormjs").Include(
                "~/scripts/modalForm.js",
                "~/scripts/toastr.js",
                "~/scripts/bootbox.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Datetimepicker").Include(
                "~/scripts/moment.js",
                "~/scripts/moment-with-locales.js",
                "~/scripts/bootstrap-datetimepicker.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapSelect").Include(
                "~/Scripts/chosen.jquery.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/font-awesome.css",
                "~/Content/toastr.css",
                "~/Content/bootstrap-datetimepicker.css",
                "~/Content/bootstrap-chosen.css",
                "~/Content/bootstrap.css",
                "~/Content/site.css"));
        }
    }
}
