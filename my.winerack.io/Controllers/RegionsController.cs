using winerack.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace winerack.Controllers {

	[Authorize(Roles = "Administrators")]
	public class RegionsController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Actions

		#region Index

		// GET: Regions
		public ActionResult Index() {
			return View(db.Regions.ToList());
		}

		#endregion Index

		#region Details

		// GET: Regions/Details/5
		[AllowAnonymous]
		[Route("regions/{id:int}")]
		public ActionResult Details(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Region region = db.Regions.Find(id);
			if (region == null) {
				return HttpNotFound();
			}
			return View(region);
		}

		#endregion Details

		#region Create

		// GET: Regions/Create
		public ActionResult Create() {
			return View();
		}

		// POST: Regions/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "ID,Country,Name")] Region region) {
			if (ModelState.IsValid) {
				db.Regions.Add(region);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(region);
		}

		#endregion Create

		#region Edit

		// GET: Regions/Edit/5
		public ActionResult Edit(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Region region = db.Regions.Find(id);
			if (region == null) {
				return HttpNotFound();
			}
			return View(region);
		}

		// POST: Regions/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "ID,Country,Name")] Region region) {
			if (ModelState.IsValid) {
				db.Entry(region).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(region);
		}

		#endregion Edit

		#region Delete

		// GET: Regions/Delete/5
		public ActionResult Delete(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Region region = db.Regions.Find(id);
			if (region == null) {
				return HttpNotFound();
			}
			return View(region);
		}

		// POST: Regions/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id) {
			Region region = db.Regions.Find(id);
			db.Regions.Remove(region);
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