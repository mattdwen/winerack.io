using my.winerack.io.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace my.winerack.io.Controllers {

	[Authorize(Roles = "Administrators")]
	public class VineyardsController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Actions

		#region Index

		// GET: Vineyards
		public ActionResult Index() {
			return View(db.Vineyards.ToList());
		}

		#endregion Index

		#region Details

		// GET: Vineyards/Details/5
		public ActionResult Details(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Vineyard vineyard = db.Vineyards.Find(id);
			if (vineyard == null) {
				return HttpNotFound();
			}
			return View(vineyard);
		}

		#endregion Details

		#region Create

		// GET: Vineyards/Create
		public ActionResult Create() {
			return View();
		}

		// POST: Vineyards/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "ID,Name")] Vineyard vineyard) {
			if (ModelState.IsValid) {
				db.Vineyards.Add(vineyard);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(vineyard);
		}

		#endregion Create

		#region Edit

		// GET: Vineyards/Edit/5
		public ActionResult Edit(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Vineyard vineyard = db.Vineyards.Find(id);
			if (vineyard == null) {
				return HttpNotFound();
			}
			return View(vineyard);
		}

		// POST: Vineyards/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "ID,Name")] Vineyard vineyard) {
			if (ModelState.IsValid) {
				db.Entry(vineyard).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(vineyard);
		}

		#endregion Edit

		#region Delete

		// GET: Vineyards/Delete/5
		public ActionResult Delete(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Vineyard vineyard = db.Vineyards.Find(id);
			if (vineyard == null) {
				return HttpNotFound();
			}
			return View(vineyard);
		}

		// POST: Vineyards/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id) {
			Vineyard vineyard = db.Vineyards.Find(id);
			db.Vineyards.Remove(vineyard);
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