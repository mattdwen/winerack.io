using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using winerack.Helpers.Authentication;
using winerack.Logic;
using winerack.Models;

namespace winerack.Controllers {

	[Authorize]
	public class BottlesController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Private Methods

		#region Create

		private Bottle Create_Bottle(CreateBottleViewModel model) {
			var userId = User.Identity.GetUserId();
			var wine = Create_Wine(model);

			var bottle = db.Bottles
				.Where(b => b.OwnerID == userId && b.WineID == wine.ID)
				.FirstOrDefault();

			if (bottle == null) {
				bottle = new Bottle {
					CreatedOn = DateTime.Now,
					OwnerID = userId,
					WineID = wine.ID,
					CellarMin = model.CellarMin,
					CellarMax = model.CellarMax
				};
				db.Bottles.Add(bottle);
				db.SaveChanges();
			}

			return bottle;
		}

		private Region Create_Region(CreateBottleViewModel model) {
			var region = db.Regions
				.Where(r => r.Country == model.Country && r.Name == model.Region)
				.FirstOrDefault();

			if (region == null) {
				region = new Region {
					Country = model.Country,
					Name = model.Region
				};

				db.Regions.Add(region);
				db.SaveChanges();
			}

			return region;
		}

		private Vineyard Create_Vineyard(CreateBottleViewModel model) {
			var vineyard = db.Vineyards
				.Where(v => v.Name == model.Vineyard)
				.FirstOrDefault();

			if (vineyard == null) {
				vineyard = new Vineyard {
					Name = model.Vineyard
				};

				db.Vineyards.Add(vineyard);
				db.SaveChanges();
			}

			return vineyard;
		}

		private Wine Create_Wine(CreateBottleViewModel model) {
			var region = Create_Region(model);
			var vineyard = Create_Vineyard(model);

			var wine = db.Wines
				.Where(w => w.Name == model.WineName
					&& w.RegionID == region.ID
					&& w.VarietalID == model.VarietalID
					&& w.VineyardID == vineyard.ID
					&& w.Vintage == model.Vintage)
				.FirstOrDefault();

			if (wine == null) {
				wine = new Wine {
					Name = model.WineName,
					RegionID = region.ID,
					VarietalID = model.VarietalID,
					VineyardID = vineyard.ID,
					Vintage = model.Vintage
				};

				db.Wines.Add(wine);
				db.SaveChanges();
			}

			return wine;
		}

		#endregion Create

		#endregion Private Methods

		#region Protected Methods

		protected override void Dispose(bool disposing) {
			if (disposing) {
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		#endregion Protected Methods

		#region Actions

		#region Index

		// GET: Bottles
		public ActionResult Index() {
			var bottles = db.Bottles.Include(b => b.Owner).Include(b => b.Wine);
			return View(bottles.ToList());
		}

		#endregion Index

		#region Details

		// GET: Bottles/Details/5
		[BottleAuthenticationAttribute]
		public ActionResult Details(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Bottle bottle = db.Bottles.Find(id);
			if (bottle == null) {
				return HttpNotFound();
			}
			return View(bottle);
		}

		#endregion Details

		#region Create

		// GET: Bottles/Create
		public ActionResult Create() {
			var user = db.Users.Find(User.Identity.GetUserId());

			var model = new CreateBottleViewModel {
				PurchaseDate = DateTime.Now,
				PurchaseQuantity = 1,
				CellarMin = 1,
				CellarMax = 3,
				HasFacebook = (user.Credentials.Where(c => c.CredentialType == CredentialTypes.Facebook).FirstOrDefault() != null),
				HasTumblr = (user.Credentials.Where(c => c.CredentialType == CredentialTypes.Tumblr).FirstOrDefault() != null),
				HasTwitter = (user.Credentials.Where(c => c.CredentialType == CredentialTypes.Twitter).FirstOrDefault() != null)
			};

			ViewBag.Country = new SelectList(Country.GetCountries(), "ID", "Name");
			ViewBag.VarietalID = db.Varietals.OrderBy(v => v.Name).Select(x => new SelectListItem {
				Text = x.Name,
				Value = x.ID.ToString()
			}).ToList();

			return View(model);
		}

		// POST: Bottles/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CreateBottleViewModel model, HttpPostedFileBase photo) {
			if (ModelState.IsValid) {
				// Get the bottle
				var bottle = Create_Bottle(model);

				// Create the purchase
				var purchase = new Purchase {
					BottleID = bottle.ID,
					PurchasedOn = model.PurchaseDate,
					PurchasePrice = model.PurchaseValue,
					Quantity = model.PurchaseQuantity,
					Notes = model.PurchaseNotes
				};

				// Save the photo
				if (photo != null && photo.ContentLength > 0) {
					var blobHandler = new Logic.BlobHandler("purchases");
					purchase.ImageID = blobHandler.UploadImage(photo, Images.GetSizes(ImageSizeSets.Standard));
				}

				// Add a stored bottle per quantity
				for (int i = 0; i < purchase.Quantity; i++) {
					purchase.StoredBottles.Add(new StoredBottle());
				}

				db.Purchases.Add(purchase);
				db.SaveChanges();

				// Push to activity log
				Logic.ActivityStream.Publish(db, User.Identity.GetUserId(), ActivityVerbs.Purchased, purchase.ID);
				db.SaveChanges();

				if (model.PostTwitter) {
					purchase = db.Purchases
						.Where(p => p.ID == purchase.ID)
						.Include("Bottle.Wine.Varietal")
						.FirstOrDefault();
					var twitter = new Logic.Social.Twitter(db);
					var quantity = Helpers.ExtensionMethods.BottleQuantity(purchase.Quantity);
					var tweet = "I've purchased " + quantity + " of " + purchase.Bottle.Wine.Description;
					var url = "http://winerack.io/purchases/" + purchase.ID.ToString();
					twitter.Tweet(User.Identity.GetUserId(), tweet, url);
				}

				return RedirectToAction("Details", new { id = bottle.ID });
			}

			ViewBag.Country = new SelectList(Country.GetCountries(), "ID", "Name");
			ViewBag.VarietalID = db.Varietals.OrderBy(v => v.Name).Select(x => new SelectListItem {
				Text = x.Name,
				Value = x.ID.ToString()
			}).ToList();

			var user = db.Users.Find(User.Identity.GetUserId());
			model.HasFacebook = (user.Credentials.Where(c => c.CredentialType == CredentialTypes.Facebook).FirstOrDefault() != null);
			model.HasTumblr = (user.Credentials.Where(c => c.CredentialType == CredentialTypes.Tumblr).FirstOrDefault() != null);
			model.HasTwitter = (user.Credentials.Where(c => c.CredentialType == CredentialTypes.Twitter).FirstOrDefault() != null);

			return View(model);
		}

		#endregion Create

		#region Edit

		// GET: Bottles/Edit/5
		[BottleAuthenticationAttribute]
		public ActionResult Edit(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Bottle bottle = db.Bottles.Find(id);
			if (bottle == null) {
				return HttpNotFound();
			}
			ViewBag.OwnerID = new SelectList(db.Users, "Id", "Email", bottle.OwnerID);
			ViewBag.WineID = new SelectList(db.Wines, "ID", "Name", bottle.WineID);
			return View(bottle);
		}

		// POST: Bottles/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[BottleAuthenticationAttribute]
		public ActionResult Edit([Bind(Include = "ID,WineID,OwnerID,CreatedOn")] Bottle bottle) {
			if (ModelState.IsValid) {
				db.Entry(bottle).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.OwnerID = new SelectList(db.Users, "Id", "Email", bottle.OwnerID);
			ViewBag.WineID = new SelectList(db.Wines, "ID", "Name", bottle.WineID);
			return View(bottle);
		}

		#endregion Edit

		#region Delete

		// GET: Bottles/Delete/5
		[BottleAuthenticationAttribute]
		public ActionResult Delete(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Bottle bottle = db.Bottles.Find(id);
			if (bottle == null) {
				return HttpNotFound();
			}
			return View(bottle);
		}

		// POST: Bottles/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[BottleAuthenticationAttribute]
		public ActionResult DeleteConfirmed(int id) {
			Bottle bottle = db.Bottles.Find(id);
			db.Bottles.Remove(bottle);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		#endregion Delete

		#region Drink

		// GET: Bottles/Drink/5
		[BottleAuthentication]
		public ActionResult Drink(int? id) {
			if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var bottle = db.Bottles.Find(id);

			if (bottle == null) {
				return HttpNotFound();
			}

			return View(bottle);
		}

		#endregion Drink

		#endregion Actions

		#region Partial Views

		public PartialViewResult Rack() {
			var userId = User.Identity.GetUserId();

			var bottles = db.Bottles
				.Where(b => b.OwnerID == userId);

			var purchases = db.Purchases
				.Include(p => p.Bottle)
				.Where(p => p.Bottle.OwnerID == userId);

			return PartialView(bottles.ToList());
		}

		#endregion Partial Views
	}
}