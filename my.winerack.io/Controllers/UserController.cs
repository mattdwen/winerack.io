using System.Linq;
using System.Net;
using System.Web.Mvc;
using winerack.Models;

namespace winerack.Controllers {

	public class UserController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Actions

		#region Details

		// GET: Users/username
		[Route("user/{username}")]
		public ActionResult Details(string username) {
			if (username == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			User user = db.Users.Where(u => u.UserName == username).FirstOrDefault();

			if (user == null) {
				return HttpNotFound();
			}

			return View(user);
		}

		#endregion Details

		#endregion Actions
	}
}