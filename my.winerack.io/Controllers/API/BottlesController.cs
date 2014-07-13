using Microsoft.AspNet.Identity;
using System;
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