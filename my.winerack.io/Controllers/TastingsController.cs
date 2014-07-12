using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using winerack.Helpers.Authentication;
using winerack.Logic;
using winerack.Models;

namespace winerack.Controllers {

	[Authorize]
	public class TastingsController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Actions

		#region Details

		// GET: Tastings/Details/5
		[TastingAuthentication]
		public ActionResult Details(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var tasting = db.Tastings.Find(id);

			if (tasting == null) {
				return HttpNotFound();
			}

			return View(tasting);
		}

		#endregion Details

		#region Create

		// GET: /Tastings/CreateFromBottle?bottleId=5
		[BottleAuthentication(IdParameter="bottleId")]
		public ActionResult CreateFromBottle(int? bottleId) {
			if (bottleId == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var bottle = db.Bottles.Find(bottleId);
			if (bottle == null) {
				return HttpNotFound();
			}

			return View(bottle);
		}

		// GET: Tastings/Create/?storedBottleId=5
		[StoredBottleAuthenticationAttribute(IdParameter = "storedBottleId")]
		public ActionResult Create(int? storedBottleId) {
			if (storedBottleId == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var tasting = new Tasting() {
				TastedOn = DateTime.Now
			};

			tasting.StoredBottle = db.StoredBottles
				.Include("Purchase.Bottle.Wine")
				.Where(b => b.ID == storedBottleId)
				.FirstOrDefault();

			return View(tasting);
		}

		// POST: Tastings/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "StoredBottleId,TastedOn,Notes")]Tasting tasting, HttpPostedFileBase photo) {
			if (ModelState.IsValid) {
				// Save the photo
				if (photo != null && photo.ContentLength > 0) {
					var blobHandler = new Logic.BlobHandler("tastings");
					tasting.ImageID = blobHandler.Upload(photo);
				}

				// Add the tasting
				db.Tastings.Add(tasting);

				// Publish the event
				ActivityStream.Publish(this.db, User.Identity.GetUserId(), ActivityVerbs.Opened, tasting.StoredBottleID);

				// Save
				db.SaveChanges();
				
				return RedirectToAction("Details", new { id = tasting.StoredBottleID });
			}

			tasting.StoredBottle = db.StoredBottles
					.Include("Purchase.Bottle.Wine")
					.Where(b => b.ID == tasting.StoredBottleID)
					.FirstOrDefault(); ;

			return View(tasting);
		}

		#endregion Create

		#region Start

		public ActionResult Start() {
			var userId = User.Identity.GetUserId();
			var bottles = db.Bottles
				.Where(b => b.OwnerID == userId)
				.ToList();

			return View(bottles);
		}

		#endregion Start

		#endregion Actions

	}
}