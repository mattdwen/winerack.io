using System.Web.Mvc;
using winerack.Helpers.Authentication;
using winerack.Models;
using System.Linq;
using Microsoft.AspNet.Identity;

namespace winerack.Controllers {

	[Authorize]
	public class TastingsController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Actions

		#region Create

		// GET: Tastings/Create
		[StoredBottleAuthenticationAttribute(IdParameter = "storedBottleId")]
		public ActionResult Create(int? storedBottleId) {
			var tasting = new Tasting();

			if (storedBottleId != null) {
				tasting.StoredBottle = db.StoredBottles
					.Include("Purchase.Bottle.Wine")
					.Where(b => b.ID == storedBottleId)
					.FirstOrDefault();
			}

			return View(tasting);
		}

		// POST: Tastings/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include="StoredBottleId,TastedOn,Notes")]Tasting tasting) {
			if (ModelState.IsValid) {
				db.Tastings.Add(tasting);
				db.SaveChanges();
				return RedirectToAction("Details", new { id = tasting.StoredBottleID });
			}

			tasting.StoredBottle = db.StoredBottles
					.Include("Purchase.Bottle.Wine")
					.Where(b => b.ID == tasting.StoredBottleID)
					.FirstOrDefault();;

			return View(tasting);
		}

		#endregion Create

		#endregion Actions
	}
}