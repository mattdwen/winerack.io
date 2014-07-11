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

		public PartialViewResult User(string userId) {
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
				Notes = storedBottle.Tasting.Notes,
				Bottle = storedBottle.Purchase.Bottle.Wine.Description,
				Winery = storedBottle.Purchase.Bottle.Wine.Vineyard.Name
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
				Quantity = purchase.StoredBottles.Count
			};

			return PartialView(viewmodel);
		}

		#endregion Activities
	}
}