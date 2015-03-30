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
			var activity = db.ActivityNotifications
				.Where(n => n.UserID == userId)
				.Select(a => a.Activity)
				.OrderByDescending(a => a.OccuredOn)
				.Take(20)
				.ToList();

			return PartialView("Stream", activity);
		}

		/// <summary>
		/// Render the stream of activity by a given user
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public PartialViewResult ByUser(string userId) {
			var activity = db.Activities
				.Where(a => a.ActorID == userId)
				.OrderByDescending(a => a.OccuredOn)
				.Take(20)
				.ToList();

			return PartialView("Stream", activity);
		}

		#endregion Partials

		#region Activities

		public PartialViewResult Opened(Activity activity) {
			var storedBottle = db.StoredBottles.Find(activity.ObjectID);
			var viewmodel = new Models.ActivityEventViewModels.Opened {
				OccuredOn = activity.OccuredOn,
				Actor = activity.Actor,
				Notes = storedBottle.Opening.Notes,
				Image = storedBottle.Opening.ImageID.HasValue ? "openings/" + storedBottle.Opening.ImageID.Value : null,
				Wine = storedBottle.Purchase.Bottle.Wine,
				ViewUrl = "/openings",
				ObjectID = storedBottle.ID
			};
			return PartialView(viewmodel);
		}

		public PartialViewResult Purchased(Activity activity) {
			var purchase = db.Purchases.Find(activity.ObjectID);

			var viewmodel = new Models.ActivityEventViewModels.Purchased {
				Actor = activity.Actor,
				OccuredOn = activity.OccuredOn,
				Notes = purchase.Notes,
				Quantity = purchase.StoredBottles.Count,
				Image = purchase.ImageID.HasValue ? "purchases/" + purchase.ImageID.Value : null,
				Wine = purchase.Bottle.Wine,
				ViewUrl = "/purchases",
				ObjectID = purchase.ID
			};

			return PartialView(viewmodel);
		}

		public PartialViewResult Tasted(Activity activity) {
			var tasting = db.Tastings.Find(activity.ObjectID);

			var viewmodel = new Models.ActivityEventViewModels.Tasted {
				Actor = activity.Actor,
				OccuredOn = activity.OccuredOn,
				Notes = tasting.Notes,
				Image = tasting.ImageID.HasValue ? "tastings/" + tasting.ImageID.Value : null,
				Wine = tasting.Wine,
				ViewUrl = "/tastings",
				ObjectID = tasting.ID
			};

			return PartialView(viewmodel);
		}

		#endregion Activities
	}
}