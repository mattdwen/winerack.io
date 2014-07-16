using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace winerack {
	public class RouteConfig {
		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "AccessDenied",
				url: "AccessDenied",
				defaults: new { controller = "Home", action = "AccessDenied" }
			);

			routes.MapRoute(
				name: "WineDetails",
				url: "{controller}/{id}",
				defaults: new { action = "Details" }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
