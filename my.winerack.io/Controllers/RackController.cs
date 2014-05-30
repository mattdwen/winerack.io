﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using winerack.Models;

namespace winerack.Controllers {

	[Authorize]
	public class RackController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Private Methods

		private IEnumerable<Wine> WinesForSelectList() {
			return db.Wines.Include("Vineyard");
		}

		#endregion Private Methods

		#region Actions

		#region Index

		// GET: Rack
		public ActionResult Index() {
			return View();
		}

		#endregion Index

		#region Add Wine

		// GET: AddWine
		public ActionResult AddWine() {
			var model = new AddWineViewModel {
				Wines = WinesForSelectList(),
				Quantity = 1,
				PurchaseDate = DateTime.Now.ToString("yyyy-MM-dd")
			};

			return View(model);
		}

		// POST: AddWine
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> AddWine([Bind(Include = "WineID,Quantity,Price,PurchaseDate")] AddWineViewModel model) {
			if (ModelState.IsValid) {
				var userId = User.Identity.GetUserId();

				// Look for the bottle
				var bottle = db.Bottles
					.Where(b => b.WineID == model.WineID && b.OwnerID == userId)
					.FirstOrDefault();

				// Do they have it?
				if (bottle == null) {
					// Add a new one
					bottle = new Bottle {
						OwnerID = userId,
						WineID = model.WineID
					};

					db.Bottles.Add(bottle);
				}

				// Add a purchase
				var purchase = new Purchase {
					BottleID = bottle.ID,
					PurchasedOn = Convert.ToDateTime(model.PurchaseDate),
					PurchasePrice = model.Price,
					Quantity = model.Quantity
				};
				db.Purchases.Add(purchase);

				await db.SaveChangesAsync();

				return RedirectToAction("");
			}

			model.Wines = WinesForSelectList();

			return View(model);
		}

		#endregion Add Wine

		#endregion Actions

		#region Partials

		#region Collection

		public PartialViewResult Collection() {
			var userId = User.Identity.GetUserId();

			var bottles = db.Bottles
				.Where(b => b.OwnerID == userId)
				.Select(x => new RackIndexBottleViewModel {
					Id = x.ID,
					Vineyard = x.Wine.Vineyard,
					Wine = x.Wine,
					Purchased = x.Purchases.Sum(p => p.Quantity)
				});

			return PartialView(bottles);
		}

		#endregion Collection

		#endregion Partials
	}
}