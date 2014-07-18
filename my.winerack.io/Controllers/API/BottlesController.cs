using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using winerack.Models;

namespace winerack.Controllers.API {

	[Authorize]
	public class BottlesController : ApiController {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Endpoints

		// GET: api/bottles
		public IList<Models.BottleViewModels.ApiBottle> GetBottles() {
			var result = new List<Models.BottleViewModels.ApiBottle>();

			var userId = User.Identity.GetUserId();
			var bottles = db.Bottles
				.Include("Wine")
				.Where(b => b.OwnerID == userId)
				.ToList();

			foreach (var bottle in bottles) {
				if (bottle.NumberRemaining < 1) {
					continue;
				}

				result.Add(new Models.BottleViewModels.ApiBottle {
					ID = bottle.ID,
					Description = bottle.Wine.Description,
					Vineyard = bottle.Wine.Vineyard.Name,
					Region = bottle.Wine.Region.Label,
					Vintage = bottle.Wine.Vintage,
					CellarMin = bottle.CellarMin,
					CellarMax = bottle.CellarMax,
					Varietal = bottle.Wine.Varietal.Name,
					VarietalStyle = bottle.Wine.Varietal.Style.ToString(),
					Purchased = bottle.NumberOfBottles,
					Opened = bottle.NumberDrunk,
					Remaining = bottle.NumberRemaining
				});
			}

			return result;
		}

		// POST: /api/Bottles/5/Cellar
		[HttpPost]
		[ActionName("cellar")]
		public IHttpActionResult PostCellar(int id, CellarBottleViewModel model) {
			var bottle = db.Bottles.Find(id);

			if (bottle == null) {
				return NotFound();
			}

			if (bottle.OwnerID != User.Identity.GetUserId()) {
				throw new HttpResponseException(HttpStatusCode.Unauthorized);
			}

			bottle.CellarMin = model.Min;
			bottle.CellarMax = model.Max;
			db.SaveChanges();

			return Ok();
		}

		#endregion Endpoints
	}
}