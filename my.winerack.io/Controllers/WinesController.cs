using winerack.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace winerack.Controllers {

	[Authorize(Roles = "Administrators")]
	public class WinesController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Actions

		#region Index

		// GET: Wines
		public ActionResult Index() {
			return View(db.Wines.ToList());
		}

		#endregion Index

		#region Details

		// GET: Wines/Details/5
		[AllowAnonymous]
		public ActionResult Details(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Wine wine = db.Wines.Find(id);
			if (wine == null) {
				return HttpNotFound();
			}
			return View(wine);
		}

		#endregion Details

		#region Create

		// GET: Wines/Create
		public ActionResult Create() {
			return View();
		}

		// POST: Wines/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "ID,Name,Varietal,Vintage,RegionID,VineyardID")] Wine wine) {
			if (ModelState.IsValid) {
				db.Wines.Add(wine);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(wine);
		}

		#endregion Create

		#region Edit

		// GET: Wines/Edit/5
		public ActionResult Edit(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Wine wine = db.Wines.Find(id);
			if (wine == null) {
				return HttpNotFound();
			}
			return View(wine);
		}

		// POST: Wines/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "ID,Name,Varietal,Vintage,RegionID,VineyardID")] Wine wine) {
			if (ModelState.IsValid) {
				db.Entry(wine).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(wine);
		}

		#endregion Edit

		#region Delete

		// GET: Wines/Delete/5
		public ActionResult Delete(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Wine wine = db.Wines.Find(id);
			if (wine == null) {
				return HttpNotFound();
			}
			return View(wine);
		}

		// POST: Wines/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id) {
			Wine wine = db.Wines.Find(id);
			db.Wines.Remove(wine);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		#endregion Delete

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