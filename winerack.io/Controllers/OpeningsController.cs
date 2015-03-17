using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using winerack.Helpers.Authentication;
using winerack.Logic;
using winerack.Models;
using winerack.Models.OpeningViewModels;

namespace winerack.Controllers {

	[Authorize]
	public class OpeningsController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Private Methods

		private Create GetCreateViewModel(Create model = null) {
			if (model == null) {
				model = new Create {
					OpenedOn = DateTime.Now
				};
			}

			var user = db.Users.Find(User.Identity.GetUserId());
			model.HasFacebook = (user.Credentials.Where(c => c.CredentialType == CredentialTypes.Facebook).FirstOrDefault() != null);
			model.HasTumblr = (user.Credentials.Where(c => c.CredentialType == CredentialTypes.Tumblr).FirstOrDefault() != null);
			model.HasTwitter = (user.Credentials.Where(c => c.CredentialType == CredentialTypes.Twitter).FirstOrDefault() != null);

			return model;
		}

		#endregion Private Methods

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

			var model = GetCreateViewModel();

			model.StoredBottle = db.StoredBottles
				.Include("Purchase.Bottle.Wine")
				.Where(b => b.ID == storedBottleId)
				.FirstOrDefault();
			model.StoredBottleID = model.StoredBottle.ID;

			return View(model);
		}

		// POST: Openings/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Create model, HttpPostedFileBase photo) {
			if (ModelState.IsValid) {
				var opening = new Opening {
					OpenedOn = model.OpenedOn,
					Notes = model.Notes,
					StoredBottleID = model.StoredBottleID
				};

				// Save the photo
				if (photo != null && photo.ContentLength > 0) {
					var blobHandler = new Logic.BlobHandler("openings");
					opening.ImageID = blobHandler.UploadImage(photo, Images.GetSizes(ImageSizeSets.Standard));
				}

				// Add the tasting
				db.Openings.Add(opening);

				// Publish the event
				var activityLogic = new Logic.Activities(db);
				var storedBottle = db.StoredBottles.Find(opening.StoredBottleID);
				activityLogic.Publish(User.Identity.GetUserId(), ActivityVerbs.Opened, opening.StoredBottleID, storedBottle.Purchase.Bottle.WineID);
				activityLogic.SaveChanges();

				// Save
				db.SaveChanges();

				var wine = db.StoredBottles.Find(model.StoredBottleID).Purchase.Bottle.Wine;

				// Share
				if (model.PostFacebook) {
					var facebook = new Logic.Social.Facebook(db);
					facebook.OpenWine(User.Identity.GetUserId(), opening.StoredBottleID);
				}

				if (model.PostTumblr && opening.ImageID.HasValue) {
					var tumblr = new Logic.Social.Tumblr(db);
					var caption = wine.Description;
					var imageUrl = "https://winerack.blob.core.windows.net/openings/" + opening.ImageID.Value.ToString() + "_lg.jpg";
					tumblr.PostPhoto(User.Identity.GetUserId(), imageUrl, caption);
				}

				if (model.PostTwitter) {
					var twitter = new Logic.Social.Twitter(db);
					var tweet = "I've opened a " + wine.Description;
					var url = "http://www.winerack.io/openings/" + opening.StoredBottleID.ToString();
					twitter.Tweet(User.Identity.GetUserId(), tweet, url);
				}
				
				return RedirectToAction("Details", new { id = opening.StoredBottleID });
			}

			model = GetCreateViewModel(model);

			model.StoredBottle = db.StoredBottles
				.Include("Purchase.Bottle.Wine")
				.Where(b => b.ID == model.StoredBottleID)
				.FirstOrDefault();

			return View(model);
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