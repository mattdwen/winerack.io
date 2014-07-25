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
	public class OpeningsController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Actions

		#region Details

		// GET: Openings/5
		[AllowAnonymous]
		[Route("openings/{id:int}")]
		public ActionResult Details(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var tasting = db.Openings.Find(id);

			if (tasting == null) {
				return HttpNotFound();
			}

			return View(tasting);
		}

		#endregion Details

		#region Create

		// GET: /Openings/CreateFromBottle?bottleId=5
		[BottleAuthentication(IdParameter="bottleId")]
		public ActionResult CreateFromBottle(int? bottleId) {
			if (bottleId == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var bottle = db.Bottles.Find(bottleId);
			if (bottle == null) {
				return HttpNotFound();
			}

			if (bottle.NumberRemaining == 1) {
				var storedId = bottle.Purchases.SelectMany(p => p.StoredBottles).Where(s => s.Opening == null).FirstOrDefault().ID;
				return RedirectToAction("Create", new { storedBottleId = storedId });
			}

			return View(bottle);
		}

		// GET: Openings/Create/?storedBottleId=5
		[StoredBottleAuthenticationAttribute(IdParameter = "storedBottleId")]
		public ActionResult Create(int? storedBottleId) {
			if (storedBottleId == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var opening = new Opening() {
				OpenedOn = DateTime.Now
			};

			opening.StoredBottle = db.StoredBottles
				.Include("Purchase.Bottle.Wine")
				.Where(b => b.ID == storedBottleId)
				.FirstOrDefault();

			return View(opening);
		}

		// POST: Openings/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "StoredBottleId,TastedOn,Notes")]Opening opening, HttpPostedFileBase photo) {
			if (ModelState.IsValid) {
				// Save the photo
				if (photo != null && photo.ContentLength > 0) {
					var blobHandler = new Logic.BlobHandler("openings");
					opening.ImageID = blobHandler.UploadImage(photo, Images.GetSizes(ImageSizeSets.Standard));
				}

				// Add the tasting
				db.Openings.Add(opening);

				// Publish the event
				ActivityStream.Publish(this.db, User.Identity.GetUserId(), ActivityVerbs.Opened, opening.StoredBottleID);

				// Save
				db.SaveChanges();
				
				return RedirectToAction("Details", new { id = opening.StoredBottleID });
			}

			opening.StoredBottle = db.StoredBottles
					.Include("Purchase.Bottle.Wine")
					.Where(b => b.ID == opening.StoredBottleID)
					.FirstOrDefault(); ;

			return View(opening);
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