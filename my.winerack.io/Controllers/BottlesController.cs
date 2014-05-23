using my.winerack.io.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace my.winerack.io.Controllers {

	public class BottlesController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Actions

		#region Index

		// GET: Bottles
		public ActionResult Index() {
			var bottles = db.Bottles.Include(b => b.Owner).Include(b => b.Wine);
			return View(bottles.ToList());
		}

		#endregion Index

		#region Details

		// GET: Bottles/Details/5
		public ActionResult Details(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Bottle bottle = db.Bottles.Find(id);
			if (bottle == null) {
				return HttpNotFound();
			}
			return View(bottle);
		}

		#endregion Details

		#region Create

		// GET: Bottles/Create
		public ActionResult Create() {
			ViewBag.OwnerID = new SelectList(db.Users, "Id", "Email");
			ViewBag.WineID = new SelectList(db.Wines, "ID", "Name");
			return View();
		}

		// POST: Bottles/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "ID,WineID,OwnerID,CreatedOn")] Bottle bottle) {
			if (ModelState.IsValid) {
				db.Bottles.Add(bottle);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.OwnerID = new SelectList(db.Users, "Id", "Email", bottle.OwnerID);
			ViewBag.WineID = new SelectList(db.Wines, "ID", "Name", bottle.WineID);
			return View(bottle);
		}

		#endregion Create

		#region Edit

		// GET: Bottles/Edit/5
		public ActionResult Edit(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Bottle bottle = db.Bottles.Find(id);
			if (bottle == null) {
				return HttpNotFound();
			}
			ViewBag.OwnerID = new SelectList(db.Users, "Id", "Email", bottle.OwnerID);
			ViewBag.WineID = new SelectList(db.Wines, "ID", "Name", bottle.WineID);
			return View(bottle);
		}

		// POST: Bottles/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "ID,WineID,OwnerID,CreatedOn")] Bottle bottle) {
			if (ModelState.IsValid) {
				db.Entry(bottle).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.OwnerID = new SelectList(db.Users, "Id", "Email", bottle.OwnerID);
			ViewBag.WineID = new SelectList(db.Wines, "ID", "Name", bottle.WineID);
			return View(bottle);
		}

		#endregion Edit

		#region Delete

		// GET: Bottles/Delete/5
		public ActionResult Delete(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Bottle bottle = db.Bottles.Find(id);
			if (bottle == null) {
				return HttpNotFound();
			}
			return View(bottle);
		}

		// POST: Bottles/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id) {
			Bottle bottle = db.Bottles.Find(id);
			db.Bottles.Remove(bottle);
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