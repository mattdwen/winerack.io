using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using winerack.Models;

namespace winerack.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class VineyardsController : Controller
    {
        #region Declarations

        private ApplicationDbContext db = new ApplicationDbContext();

        #endregion Declarations

        #region Private Methods

        private Region GetRegion(string name, string country)
        {
            var region = db.Regions.Where(r => r.Name == name && r.Country == country).FirstOrDefault();

            if (region == null) {
                region = new Region {
                    Name = name,
                    Country = country
                };
                db.Regions.Add(region);
            }

            return region;
        }

        #region ViewBag

        private void PopulateViewBag()
        {
            ViewBag.Country = new SelectList(Country.GetCountries(), "ID", "Name");
        }

        #endregion ViewBag

        #endregion Private Methods

        #region Actions

        #region Index

        // GET: Vineyards
        public ActionResult Index()
        {
            return View(db.Vineyards.OrderBy(v => v.Name).ToList());
        }

        #endregion Index

        #region Details

        // GET: Vineyards/Details/5
        [AllowAnonymous]
        [Route("vineyards/{id:int}")]
        public ActionResult Details(int? id)
        {
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
        public ActionResult Create()
        {
            PopulateViewBag();
            return View();
        }

        // POST: Vineyards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.VineyardViewModels.VineyardEditVM model)
        {
            if (ModelState.IsValid) {
                var vineyard = new Vineyard {
                    Name = model.Name,
                    Region = GetRegion(model.Region, model.Country)
                };


                db.Vineyards.Add(vineyard);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        #endregion Create

        #region Edit

        // GET: Vineyards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Vineyard vineyard = db.Vineyards.Find(id);
            if (vineyard == null) {
                return HttpNotFound();
            }

            var model = new Models.VineyardViewModels.VineyardEditVM {
                ID = vineyard.ID,
                Name = vineyard.Name
            };

            if (vineyard.Region != null) {
                model.Region = vineyard.Region.Name;
                model.Country = vineyard.Region.Country;
            }

            PopulateViewBag();
            return View(model);
        }

        // POST: Vineyards/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Models.VineyardViewModels.VineyardEditVM model)
        {
            if (ModelState.IsValid) {
                var vineyard = db.Vineyards.Find(model.ID);
                if (vineyard == null) {
                    return HttpNotFound();
                }

                vineyard.Name = model.Name;
                vineyard.Region = GetRegion(model.Region, model.Country);

                db.Entry(vineyard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateViewBag();
            return View(model);
        }

        #endregion Edit

        #region Delete

        // GET: Vineyards/Delete/5
        public ActionResult Delete(int? id)
        {
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
        public ActionResult DeleteConfirmed(int id)
        {
            Vineyard vineyard = db.Vineyards.Find(id);
            db.Vineyards.Remove(vineyard);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion Delete

        #endregion Actions

        #region Protected Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion Protected Methods
    }
}