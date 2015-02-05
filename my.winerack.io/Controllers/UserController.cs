using Microsoft.AspNet.Identity;
using System;
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

		#region Partials

		#region Profile

		public PartialViewResult MiniProfile() {
			var userId = User.Identity.GetUserId();
			var model = new Models.MiniProfileViewModel();
			var user = db.Users.Find(userId);

			if (user == null) {
				return null;
			}

			model.Username = user.UserName;
			model.Name = user.Name;
			model.Location = user.LocationDescription;

			model.BottlesTotal = db.StoredBottles
				.Where(sb => sb.Purchase.Bottle.OwnerID == userId)
				.Count();

			model.BottlesUnique = db.Bottles
				.Where(b => b.OwnerID == userId)
				.Count();

			model.BottlesDrunk = db.Openings
				.Where(t => t.StoredBottle.Purchase.Bottle.OwnerID == userId)
				.Count();

			return PartialView(model);
		}

		public PartialViewResult StatBar(string userId) {
			var model = new Models.MiniProfileViewModel();
			var user = db.Users.Find(userId);

			model.BottlesTotal = db.StoredBottles
				.Where(sb => sb.Purchase.Bottle.OwnerID == userId)
				.Count();

			model.BottlesUnique = db.Bottles
				.Where(b => b.OwnerID == userId)
				.Count();

			model.BottlesDrunk = db.Openings
				.Where(t => t.StoredBottle.Purchase.Bottle.OwnerID == userId)
				.Count();

			model.Tasted = db.Tastings
				.Where(t => t.UserID == userId)
				.Count();

			return PartialView(model);
		}

		#endregion Profile

		#endregion Partials
	}
}