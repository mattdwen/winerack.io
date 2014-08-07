using System.Linq;
using System.Web;
using System.Web.Mvc;
using winerack.Models;
using winerack.Models.TastingViewModels;
using Microsoft.AspNet.Identity;
using winerack.Logic;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace winerack.Controllers {

	[Authorize]
	public class TastingsController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Private Methods

		private Region Create_Region(Create model) {
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

		private Vineyard Create_Vineyard(Create model) {
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

		private Wine Create_Wine(Create model) {
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

		private Create GetCreateViewModel(Create model = null) {
			if (model == null) {
				model = new Create {
					TastingDate = DateTime.Now
				};
			}

			var user = db.Users.Find(User.Identity.GetUserId());
			model.HasFacebook = (user.Credentials.Where(c => c.CredentialType == CredentialTypes.Facebook).FirstOrDefault() != null);
			model.HasTumblr = (user.Credentials.Where(c => c.CredentialType == CredentialTypes.Tumblr).FirstOrDefault() != null);
			model.HasTwitter = (user.Credentials.Where(c => c.CredentialType == CredentialTypes.Twitter).FirstOrDefault() != null);

			return model;
		}

		private void PopulateCreateViewBag(bool hasFacebook = false) {
			ViewBag.Country = new SelectList(Country.GetCountries(), "ID", "Name");

			ViewBag.VarietalID = db.Varietals.OrderBy(v => v.Name).Select(x => new SelectListItem {
				Text = x.Name,
				Value = x.ID.ToString()
			}).ToList();

			var userId = User.Identity.GetUserId();
			var friendList = new List<SelectListItem>();

			var following = db.Friends.Where(f => f.FollowerID == userId).ToList();
			foreach (var followee in following) {
				friendList.Add(new SelectListItem {
					Text = followee.Followee.Name,
					Value = followee.FolloweeID
				});
			}

			if (hasFacebook) {
				var facebook = new Logic.Social.Facebook(db);
				var facebookFriends = facebook.GetFriends(User.Identity.GetUserId());
				foreach (var friend in facebookFriends) {
					friendList.Add(new SelectListItem {
						Text = friend.name,
						Value = "fb::" + friend.id + "::" + friend.name
					});
				}
			}

			friendList = friendList.OrderBy(f => f.Text).ToList();

			ViewBag.Friends = friendList;
		}

		#endregion Private Methods

		#region Actions

		#region Create

		// GET: /tastings/create
		public ActionResult Create() {
			var model = GetCreateViewModel();
			PopulateCreateViewBag(model.HasFacebook);
			return View(model);
		}

		// POST: /tastings/create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Create model, HttpPostedFileBase photo) {
			if (ModelState.IsValid) {
				// Get the wine
				var wine = Create_Wine(model);

				// Create the tasting
				var tasting = new Tasting {
					UserID = User.Identity.GetUserId(),
					WineID = wine.ID,
					TastedOn = model.TastingDate,
					Notes = model.TastingNotes
				};

				// Save the photo
				if (photo != null && photo.ContentLength > 0) {
					var blobHandler = new Logic.BlobHandler("tastings");
					tasting.ImageID = blobHandler.UploadImage(photo, Images.GetSizes(ImageSizeSets.Standard));
				}

				db.Tastings.Add(tasting);
				db.SaveChanges();

				// Tag Users
				var taggingLogic = new Tagging(db);
				var taggedUsers = taggingLogic.TagUsers(model.Friends, tasting.ID, ActivityVerbs.Tasted, User.Identity.GetUserId());

				// Push to activity log
				var activityLogic = new Logic.Activities(db);
				var actvity = activityLogic.Publish(User.Identity.GetUserId(), ActivityVerbs.Tasted, tasting.ID, tasting.WineID);
				db.SaveChanges();

				wine = db.Wines.Find(wine.ID);

				// Share
				var baseUrl = ConfigurationManager.AppSettings["baseUrl"];
				var tastingUrl = baseUrl + "/tastings/" + tasting.ID.ToString();

				if (model.PostFacebook) {
					var facebook = new Logic.Social.Facebook(db);
					facebook.TasteWine(User.Identity.GetUserId(), tasting.ID);
				}

				if (model.PostTumblr && tasting.ImageID.HasValue) {
					var tumblr = new Logic.Social.Tumblr(db);
					var caption = wine.Description;
					var imageUrl = "https://winerack.blob.core.windows.net/tastings/" + tasting.ImageID.Value.ToString() + "_lg.jpg";
					tumblr.PostPhoto(User.Identity.GetUserId(), imageUrl, caption);
				}

				if (model.PostTwitter) {
					var twitter = new Logic.Social.Twitter(db);
					var tweet = "I'm tasting a " + wine.Vineyard.Name + " " + wine.Description;
					twitter.Tweet(User.Identity.GetUserId(), tweet, tastingUrl);
				}

				return Redirect("/tastings/" + tasting.ID.ToString());
			}

			model = GetCreateViewModel(model);
			PopulateCreateViewBag(model.HasFacebook);
			return View(model);
		}

		#endregion Create

		#region Details

		// GET: /tastings/5
		[AllowAnonymous]
		[Route("tastings/{id:int}")]
		public ActionResult Details(int id) {
			var tasting = db.Tastings.Find(id);
			var model = new Details {
				Tasting = db.Tastings.Find(id),
				TaggedUsers = db.TaggedUsers.Where(t => t.ParentID == id && t.ActivityVerb == ActivityVerbs.Tasted).ToList()
			};
			return View(model);
		}

		#endregion Details

		#endregion Actions
	}
}