using winerack.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using winerack.Models.WineViewModels;
using System;
using System.Collections.Generic;

namespace winerack.Controllers {

	[Authorize(Roles = "Administrators")]
	public class WinesController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

        #region Private Methods

        private WineCreateAndEditModel GetWineCreateEditViewModel(Wine wine)
        {
            var viewModel = new WineCreateAndEditModel {
                Name = wine.Name,
                RegionID = wine.RegionID,
                Styles = wine.Styles.Select(s => s.ID).ToList(),
                Varietals = wine.Varietals.Select(v => v.ID).ToList(),
                VineyardID = wine.VineyardID,
                Vintage = wine.Vintage
            };

            return viewModel;
        }

        private Wine UpdateWine(Wine wine, WineCreateAndEditModel viewModel)
        {
            wine.Name = viewModel.Name;
            wine.VineyardID = viewModel.VineyardID;
            wine.RegionID = viewModel.RegionID;
            wine.Vintage = viewModel.Vintage;

            // Remove old styles
            var removeStyles = new List<Style>();
            foreach (var style in wine.Styles) {
                if (!viewModel.Styles.Contains(style.ID)) {
                    removeStyles.Add(style);
                }
            }
            foreach (var style in removeStyles) {
                wine.Styles.Remove(style);
            }

            // Add new styles
            foreach (var styleId in viewModel.Styles) {
                var style = db.Styles.Find(styleId);
                if (style != null) {
                    wine.Styles.Add(style);
                }
            }

            // Remove old varietals
            var removeVarietals = new List<Varietal>();
            foreach (var varietal in wine.Varietals) {
                if (!viewModel.Varietals.Contains(varietal.ID)) {
                    removeVarietals.Add(varietal);
                }
            }
            foreach (var varietal in removeVarietals) {
                wine.Varietals.Remove(varietal);
            }

            // Add new varietals
            foreach (var varietalId in viewModel.Varietals) {
                var varietal = db.Varietals.Find(varietalId);
                if (varietal != null) {
                    wine.Varietals.Add(varietal);
                }
            }

            return wine;
        }

        #endregion

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
		[Route("wines/{id:int}")]
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
            var viewModel = new WineCreateAndEditModel();

			return View(viewModel);
		}

		// POST: Wines/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(WineCreateAndEditModel viewModel) {
			if (ModelState.IsValid) {
                var wine = UpdateWine(new Wine(), viewModel);
				db.Wines.Add(wine);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

            return View(viewModel);
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

            var viewModel = GetWineCreateEditViewModel(wine);

            return View(viewModel);
		}

		// POST: Wines/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int? id, WineCreateAndEditModel viewModel) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Wine wine = db.Wines.Find(id);
            if (wine == null) {
                return HttpNotFound();
            }

			if (ModelState.IsValid) {
                wine = UpdateWine(wine, viewModel);
                db.Entry(wine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
			}

            return View(viewModel);
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