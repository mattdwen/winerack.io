using winerack.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System;

namespace winerack.Controllers {

	[Authorize]
	public class PurchasesController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Actions

		#region Index

		// GET: Purchases
		public ActionResult Index() {
			var userId = User.Identity.GetUserId();
			var purchases = db.Purchases
				.Include(p => p.Bottle)
				.Where(p => p.Bottle.OwnerID == userId);
			return View(purchases.ToList());
		}

		#endregion Index

		#region Details
		// GET: Purchases/Details/5
		public ActionResult Details(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Purchase purchase = db.Purchases.Find(id);

			if (purchase == null) {
				return HttpNotFound();
			}

			return View(purchase);
		}
		#endregion

		#region Create
		// GET: Purchases/Create
		public ActionResult Create(int? BottleId = null) {
			if (BottleId == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var purchase = new Purchase {
				BottleID = BottleId.Value,
				Bottle = db.Bottles.Find(BottleId),
				PurchasedOn = DateTime.Now,
				Quantity = 1
			};

			return View(purchase);
		}

		// POST: Purchases/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "BottleID,Quantity,PurchasedOn,PurchasePrice,Notes")] Purchase purchase) {
			if (ModelState.IsValid) {
				// Add the purchase
				db.Purchases.Add(purchase);

				// Add a stored bottle per quantity
				for (int i = 0; i < purchase.Quantity; i++) {
					db.StoredBottles.Add(new StoredBottle {
						BottleID = purchase.BottleID
					});
				}

				// Commit
				db.SaveChanges();

				// Redirect
				return RedirectToAction("Index");
			}

			purchase.Bottle = db.Bottles.Find(purchase.BottleID);

			return View(purchase);
		}
		#endregion

		#region Edit

		// GET: Purchases/Edit/5
		public ActionResult Edit(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Purchase purchase = db.Purchases.Find(id);
			if (purchase == null) {
				return HttpNotFound();
			}
			ViewBag.BottleID = new SelectList(db.Bottles, "ID", "OwnerID", purchase.BottleID);
			return View(purchase);
		}

		// POST: Purchases/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "ID,BottleID,Quantity,PurchasedOn,PurchasePrice,Notes")] Purchase purchase) {
			if (ModelState.IsValid) {
				db.Entry(purchase).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.BottleID = new SelectList(db.Bottles, "ID", "OwnerID", purchase.BottleID);
			return View(purchase);
		}

		#endregion Edit

		#region Delete

		// GET: Purchases/Delete/5
		public ActionResult Delete(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Purchase purchase = db.Purchases.Find(id);
			if (purchase == null) {
				return HttpNotFound();
			}
			return View(purchase);
		}

		// POST: Purchases/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id) {
			Purchase purchase = db.Purchases.Find(id);
			db.Purchases.Remove(purchase);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		#endregion Delete

		#endregion Actions

		#region Protected Methods

		protected override void Dispose(bool disposing) {
			if (disposing) {
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		#endregion Protected Methods
	}
}