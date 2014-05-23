﻿using my.winerack.io.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace my.winerack.io.Controllers {

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
		public ActionResult Edit([Bind(Include = "ID,BottleID,Quantity,PurchasedOn,PurchasePrice")] Purchase purchase) {
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