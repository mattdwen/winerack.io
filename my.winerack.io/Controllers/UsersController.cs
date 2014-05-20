using my.winerack.io.Models;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace my.winerack.io.Controllers {

	public class UsersController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Actions

		#region Index

		// GET: Users
		public ActionResult Index() {
			return View(db.Users.ToList());
		}

		#endregion Index

		#region Details

		// GET: Users/Details/5
		public ActionResult Details(string id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			User user = db.Users.Find(id);
			if (user == null) {
				return HttpNotFound();
			}
			return View(user);
		}

		#endregion Details

		#endregion Actions

		#region Public Methods

		protected override void Dispose(bool disposing) {
			if (disposing) {
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		#endregion Public Methods
	}
}