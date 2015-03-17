using Mindscape.Raygun4Net;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace winerack {

	public class MvcApplication : System.Web.HttpApplication
    {

        #region Constants

        public const string ADMINISTRATOR_GROUP = "Administrators";

        #endregion Constants

        #region Declarations

        RaygunClient client;

        #endregion Declarations

        protected void Application_Start() {
            var config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
            var raygunKey = config.AppSettings.Settings["raygun:apiKey"];
            if (raygunKey != null) {
                client = new RaygunClient(raygunKey.Value);
            }

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

        protected void Application_Error()
        {
            if (client != null) {
                var exception = Server.GetLastError();
                client.SendInBackground(exception);
            }
        }
	}
}