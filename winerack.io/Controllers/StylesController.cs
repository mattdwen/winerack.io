using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using winerack.Models;

namespace winerack.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class StylesController : Controller
    {
        #region Declarations

        private ApplicationDbContext db = new ApplicationDbContext();

        #endregion Declarations

        #region Actions

        #region Index

        // GET: Styles
        public async Task<ActionResult> Index()
        {
            return View(await db.Styles.OrderBy(s => s.Name).ToListAsync());
        }

        #endregion Index

        #region Details

        // GET: Styles/5
        [Route("Styles/{id:int}")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Style style = await db.Styles.FindAsync(id);
            if (style == null) {
                return HttpNotFound();
            }

            return View(style);
        }

        #endregion Details

        #region Create

        // GET: Styles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Styles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name")] Style style)
        {
            if (ModelState.IsValid) {
                db.Styles.Add(style);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(style);
        }

        #endregion Create

        #region Edit

        // GET: Styles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = await db.Styles.FindAsync(id);
            if (style == null) {
                return HttpNotFound();
            }
            return View(style);
        }

        // POST: Styles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name")] Style style)
        {
            if (ModelState.IsValid) {
                db.Entry(style).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(style);
        }

        #endregion Edit

        #region Delete

        // GET: Styles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = await db.Styles.FindAsync(id);
            if (style == null) {
                return HttpNotFound();
            }
            return View(style);
        }

        // POST: Styles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Style style = await db.Styles.FindAsync(id);
            db.Styles.Remove(style);
            await db.SaveChangesAsync();
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