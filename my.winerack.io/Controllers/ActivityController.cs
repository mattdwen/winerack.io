using System.Linq;
using System.Net;
using System.Web.Mvc;
using winerack.Models;

namespace winerack.Controllers {

	public class ActivityController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Partials

		/// <summary>
		/// Render the stream which is visible by a given user
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public PartialViewResult VisibleToUser(string userId) {
			var activity = db.ActivityEvents
				.Where(e => e.UserID == userId)
				.OrderByDescending(e => e.OccuredOn)
				.Take(20)
				.ToList();

			return PartialView("Stream", activity);
		}

		#endregion Partials

		#region Activities

		public PartialViewResult Opened(ActivityEvent activity) {
			var user = db.Users.Where(u => u.Id == activity.UserID).FirstOrDefault();
			var storedBottle = db.StoredBottles.Find(activity.Noun);
			var viewmodel = new Models.ActivityEventViewModels.Opened {
				OccuredOn = activity.OccuredOn,
				Username = user.FirstName + " " + user.LastName,
				Notes = storedBottle.Opening.Notes,
				Bottle = storedBottle.Purchase.Bottle.Wine.Description,
				Winery = storedBottle.Purchase.Bottle.Wine.Vineyard.Name,
				Image = storedBottle.Opening.ImageID.HasValue ? "openings/" + storedBottle.Opening.ImageID.Value : null,
				WineID = storedBottle.Purchase.Bottle.WineID,
				VineyardID = storedBottle.Purchase.Bottle.Wine.VineyardID,
				ViewUrl = "/openings",
				ObjectID = storedBottle.ID
			};
			return PartialView(viewmodel);
		}

		public PartialViewResult Purchased(ActivityEvent activity) {
			var user = db.Users.Where(u => u.Id == activity.UserID).FirstOrDefault();
			var purchase = db.Purchases.Find(activity.Noun);

			var viewmodel = new Models.ActivityEventViewModels.Purchased {
				OccuredOn = activity.OccuredOn,
				Username = user.FirstName + " " + user.LastName,
				Notes = purchase.Notes,
				Bottle = purchase.Bottle.Wine.Description,
				Winery = purchase.Bottle.Wine.Vineyard.Name,
				Quantity = purchase.StoredBottles.Count,
				Image = purchase.ImageID.HasValue ? "purchases/" + purchase.ImageID.Value : null,
				WineID = purchase.Bottle.WineID,
				VineyardID = purchase.Bottle.Wine.VineyardID,
				ViewUrl = "/purchases",
				ObjectID = purchase.ID
			};

			return PartialView(viewmodel);
		}

		#endregion Activities
	}
}