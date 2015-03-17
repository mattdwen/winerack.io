using winerack.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System;
using winerack.Helpers.Authentication;
using System.Web;
using winerack.Logic;
using winerack.Models.PurchaseViewModels;

namespace winerack.Controllers {

	[Authorize]
	public class PurchasesController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Private Methods

		private Create GetCreateViewModel(Create model = null) {
			if (model == null) {
				model = new Create {
					PurchasedOn = DateTime.Now
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

		#region Index

		// GET: Purchases
		public ActionResult Index() {
			var userId = User.Identity.GetUserId();
			var purchases = db.Purchases
				.Include(p => p.Bottle)
				.Where(p => p.Bottle.OwnerID == userId);
			return View(purchases.ToList());
		}

		#endregion Index

		#region Details
		// GET: Purchases/Details/5
		[Route("purchases/{id:int}")]
		[AllowAnonymous]
		public ActionResult Details(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Purchase purchase = db.Purchases.Find(id);

			if (purchase == null) {
				return HttpNotFound();
			}

			return View(purchase);
		}
		#endregion

		#region Create
		// GET: Purchases/Create
		public ActionResult Create(int? BottleId = null) {
			if (BottleId == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var model = GetCreateViewModel();
			model.BottleID = BottleId.Value;
			model.Bottle = db.Bottles.Find(BottleId);
			model.Quantity = 1;

			return View(model);
		}

		// POST: Purchases/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Create model, HttpPostedFileBase photo) {
			if (ModelState.IsValid) {
				var purchase = new Purchase {
					BottleID = model.BottleID,
					Notes = model.Notes,
					PurchasedOn = model.PurchasedOn,
					PurchasePrice = model.PurchasePrice,
					Quantity = model.Quantity
				};

				// Add a stored bottle per quantity
				for (int i = 0; i < purchase.Quantity; i++) {
					purchase.StoredBottles.Add(new StoredBottle());
				}

				// Save the photo
				if (photo != null && photo.ContentLength > 0) {
					var blobHandler = new Logic.BlobHandler("purchases");
					purchase.ImageID = blobHandler.UploadImage(photo, Images.GetSizes(ImageSizeSets.Standard));
				}

				// Add the purchase
				db.Purchases.Add(purchase);
				db.SaveChanges();

				// Activity feed
				var activityLogic = new Logic.Activities(db);
				var bottle = db.Bottles.Find(purchase.BottleID);
				activityLogic.Publish(User.Identity.GetUserId(), ActivityVerbs.Purchased, purchase.ID, bottle.WineID);
				activityLogic.SaveChanges();

				// Share
				if (model.PostFacebook) {
					var facebook = new Logic.Social.Facebook(db);
					facebook.PurchaseWine(User.Identity.GetUserId(), purchase.ID);
				}

				if (model.PostTwitter) {
					purchase = db.Purchases.Find(purchase.ID);
					var twitter = new Logic.Social.Twitter(db);
					var quantity = Helpers.ExtensionMethods.BottleQuantity(purchase.Quantity);
					var tweet = "I've purchased " + quantity + " of " + purchase.Bottle.Wine.Description;
					var url = "http://www.winerack.io/purchases/" + purchase.ID.ToString();
					twitter.Tweet(User.Identity.GetUserId(), tweet, url);
				}

				// Redirect
				return RedirectToAction("Index");
			}

			model = GetCreateViewModel(model);

			model.Bottle = db.Bottles.Find(model.BottleID);

			return View(model);
		}
		#endregion

		#region Edit

		// GET: Purchases/Edit/5
		[PurchaseAuthenticationAttribute]
		public ActionResult Edit(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Purchase purchase = db.Purchases.Find(id);
			if (purchase == null) {
				return HttpNotFound();
			}

            purchase.Quantity = 1;

			ViewBag.BottleID = new SelectList(db.Bottles, "ID", "OwnerID", purchase.BottleID);

			return View(purchase);
		}

		// POST: Purchases/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[PurchaseAuthenticationAttribute]
		public ActionResult Edit([Bind(Include = "ID,BottleID,PurchasedOn,PurchasePrice,Notes,Quantity")] Purchase purchase) {
			if (ModelState.IsValid) {
				db.Entry(purchase).State = EntityState.Modified;
				db.SaveChanges();
                return RedirectToAction("Details", new { id = purchase.ID });
			}

            purchase.Bottle = db.Bottles.Find(purchase.BottleID);

			ViewBag.BottleID = new SelectList(db.Bottles, "ID", "OwnerID", purchase.BottleID);

			return View(purchase);
		}

		#endregion Edit

		#region Delete

		// GET: Purchases/Delete/5
		[PurchaseAuthenticationAttribute]
		public ActionResult Delete(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Purchase purchase = db.Purchases.Find(id);
			if (purchase == null) {
				return HttpNotFound();
			}
			return View(purchase);
		}

		// POST: Purchases/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[PurchaseAuthenticationAttribute]
		public ActionResult DeleteConfirmed(int id) {
			Purchase purchase = db.Purchases.Find(id);
			db.Purchases.Remove(purchase);
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