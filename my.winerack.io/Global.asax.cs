using System.Configuration;
using System.Data.Entity.Migrations;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace winerack {

	public class MvcApplication : System.Web.HttpApplication {

		protected void Application_Start() {
			if (bool.Parse(ConfigurationManager.AppSettings["MigrateDatabaseToLatestVersion"])) {
				var configuration = new winerack.Migrations.Configuration();
				var migrator = new DbMigrator(configuration);
				migrator.Update();
			}

			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
	}
}