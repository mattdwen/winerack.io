using System.Web.Mvc;

namespace winerack.Controllers {

	public class HomeController : Controller {

		#region Actions

		public ActionResult Index() {
			return View();
		}

		public ActionResult AccessDenied() {
			Response.StatusCode = 403;
			return View();
		}

		[Route("privacy")]
		public ActionResult Privacy() {
			return View();
		}

		#endregion Actions
	}
}