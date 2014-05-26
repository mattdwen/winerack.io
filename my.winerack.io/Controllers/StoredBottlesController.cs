using System.Net;
using System.Web.Mvc;
using winerack.Models;
using System;
using System.Data.Entity;

namespace winerack.Controllers {

	[Authorize]
	public class StoredBottlesController : Controller {

		#region Declarations
		private ApplicationDbContext db = new ApplicationDbContext();
		#endregion

		#region Actions
		#region Update
		// POST: storedbottles/update
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Update(int? BottleID, FormCollection form) {
			if (BottleID == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var bottle = db.Bottles.Find(BottleID);

			if (bottle == null) {
				return HttpNotFound();
			}

			foreach (var stored in bottle.StoredBottles) {
				stored.Location = form["bottle_" + stored.ID];
				db.Entry(stored).State = EntityState.Modified;
			}

			db.SaveChanges();

			return Redirect(Url.RouteUrl(new { controller = "Bottles", action = "Details", id = BottleID }) + "#storage");
		}
		#endregion
		#endregion
	}
}