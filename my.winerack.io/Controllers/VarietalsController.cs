using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using winerack.Models;

namespace winerack.Controllers {

	public class VarietalsController : Controller {
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Varietals
		public async Task<ActionResult> Index() {
			return View(await db.Varietals.ToListAsync());
		}

		// GET: Varietals/Details/5
		public async Task<ActionResult> Details(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Varietal varietal = await db.Varietals.FindAsync(id);
			if (varietal == null) {
				return HttpNotFound();
			}
			return View(varietal);
		}

		// GET: Varietals/Create
		public ActionResult Create() {
			return View();
		}

		// POST: Varietals/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "ID,Name,Style,CellarMin,CellarMax")] Varietal varietal) {
			if (ModelState.IsValid) {
				db.Varietals.Add(varietal);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			return View(varietal);
		}

		// GET: Varietals/Edit/5
		public async Task<ActionResult> Edit(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Varietal varietal = await db.Varietals.FindAsync(id);
			if (varietal == null) {
				return HttpNotFound();
			}
			return View(varietal);
		}

		// POST: Varietals/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "ID,Name,Style,CellarMin,CellarMax")] Varietal varietal) {
			if (ModelState.IsValid) {
				db.Entry(varietal).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(varietal);
		}

		// GET: Varietals/Delete/5
		public async Task<ActionResult> Delete(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Varietal varietal = await db.Varietals.FindAsync(id);
			if (varietal == null) {
				return HttpNotFound();
			}
			return View(varietal);
		}

		// POST: Varietals/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id) {
			Varietal varietal = await db.Varietals.FindAsync(id);
			db.Varietals.Remove(varietal);
			await db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing) {
			if (disposing) {
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}