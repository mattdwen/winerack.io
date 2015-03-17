using System.Web.Optimization;

namespace winerack {

	public class BundleConfig {

		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles) {

			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
				"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
				"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
				"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/angular").Include(
				"~/Scripts/dist/angular.js"));

            bundles.Add(new ScriptBundle("~/bundles/winerack").Include(
                "~/Scripts/winerack.js"
                ));

			bundles.Add(new StyleBundle("~/Content/css").Include(
				"~/Scripts/vendor/jQRangeSlider/css/classic.css",
                "~/Content/vendor/select2/select2.css",
				"~/Content/css/main.css",
				"~/Content/css/icomoon-wines.css"));

			bundles.Add(new ScriptBundle("~/bundles/bottlecreate").Include(
				"~/Scripts/bloodhound/vineyards.js",
				"~/Scripts/bloodhound/regions.js",
				"~/Scripts/bloodhound/varietals.js",
				"~/Scripts/jquery-ui-1.10.4.min.js",
				"~/Scripts/bottles/create.js"));

			bundles.Add(new ScriptBundle("~/bundles/tastingscreate").Include(
				"~/Scripts/bloodhound/vineyards.js",
				"~/Scripts/bloodhound/regions.js",
				"~/Scripts/bloodhound/varietals.js",
				"~/Scripts/jquery-ui-1.10.4.min.js",
				"~/Scripts/tastings/create.js"));

            bundles.Add(new ScriptBundle("~/bundles/wine-editor").Include(
                "~/Scripts/wine/editor.js"
                ));

			// Set EnableOptimizations to false for debugging. For more information,
			// visit http://go.microsoft.com/fwlink/?LinkId=301862
			BundleTable.EnableOptimizations = false;
		}
	}
}