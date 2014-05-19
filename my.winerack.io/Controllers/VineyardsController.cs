using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using my.winerack.io.Models;

namespace my.winerack.io.Controllers
{
    public class VineyardsController : Controller
    {
        private WineRackDbContext db = new WineRackDbContext();

        // GET: Vineyards
        public ActionResult Index()
        {
            return View(db.Vineyards.ToList());
        }

        // GET: Vineyards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vineyard vineyard = db.Vineyards.Find(id);
            if (vineyard == null)
            {
                return HttpNotFound();
            }
            return View(vineyard);
        }

        // GET: Vineyards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vineyards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] Vineyard vineyard)
        {
            if (ModelState.IsValid)
            {
                db.Vineyards.Add(vineyard);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vineyard);
        }

        // GET: Vineyards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vineyard vineyard = db.Vineyards.Find(id);
            if (vineyard == null)
            {
                return HttpNotFound();
            }
            return View(vineyard);
        }

        // POST: Vineyards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] Vineyard vineyard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vineyard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vineyard);
        }

        // GET: Vineyards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vineyard vineyard = db.Vineyards.Find(id);
            if (vineyard == null)
            {
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
