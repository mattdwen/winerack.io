using System.Web.Optimization;

namespace winerack
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/dist/angular.js"));

            bundles.Add(new ScriptBundle("~/bundles/winerack").Include(
                "~/Scripts/winerack.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Scripts/vendor/jQRangeSlider/css/classic.css",
                "~/Content/vendor/select2/select2.css",
                "~/Content/css/main.css",
                "~/Content/css/icomoon-wines.css"));

            bundles.Add(new ScriptBundle("~/bundles/bottles/create").Include(                
                "~/Scripts/bottles/create.js"));

            bundles.Add(new ScriptBundle("~/bundles/tastings/create").Include(
                "~/Scripts/tastings/create.js"));

            bundles.Add(new ScriptBundle("~/bundles/vineyard/edit").Include(
                "~/Scripts/bloodhound/regions.js",
                "~/Scripts/vineyard/edit.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/wine/editor").Include(
                "~/Scripts/bloodhound/vineyards.js",
                "~/Scripts/bloodhound/regions.js",
                "~/Scripts/bloodhound/varietals.js",
                "~/Scripts/wine/editor.js"
                ));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}