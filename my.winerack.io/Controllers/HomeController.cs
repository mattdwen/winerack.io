using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace winerack.Controllers {
	public class HomeController : Controller {
		#region Actions

		public ActionResult Index() {
			return View();
		}

		#endregion Actions
	}
}