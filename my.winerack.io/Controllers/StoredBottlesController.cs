using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using winerack.Models;

namespace winerack.Controllers {

	[Authorize]
	public class StoredBottlesController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

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

			var storedBottles = bottle.Purchases.SelectMany(p => p.StoredBottles);
			foreach (var stored in storedBottles) {
				stored.Location = form["bottle_" + stored.ID];
				db.Entry(stored).State = EntityState.Modified;
			}

			db.SaveChanges();

			return Redirect(Url.RouteUrl(new { controller = "Bottles", action = "Details", id = BottleID }) + "#bottles");
		}

		#endregion Update

		#region Delete

		// GET: StoredBottles/Delete/5
		public ActionResult Delete(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			StoredBottle bottle = db.StoredBottles.Find(id);

			if (bottle == null) {
				return HttpNotFound();
			}

			return View(bottle);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id) {
			var stored = db.StoredBottles.Find(id);
			var bottleId = stored.Purchase.BottleID;
			db.StoredBottles.Remove(stored);
			db.SaveChanges();
			return Redirect(Url.RouteUrl(new { controller = "Bottles", action = "Details", id = bottleId }) + "#bottles");
		}

		#endregion Delete

		#endregion Actions
	}
}