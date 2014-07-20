namespace winerack.Migrations {

	using Microsoft.AspNet.Identity.EntityFramework;
	using winerack.Models;
	using System.Data.Entity.Migrations;
	using System.Linq;
	using System.Threading.Tasks;
	using System;
	using System.Collections;
	using System.Collections.Generic;

	internal sealed class Configuration : DbMigrationsConfiguration<winerack.Models.ApplicationDbContext> {

		#region Properties

		public Configuration() {
			AutomaticMigrationsEnabled = false;
		}

		#endregion Properties

		#region Private Methods

		private async Task<bool> Seed_Identity(ApplicationDbContext context) {
			if (!context.Roles.Any(r => r.Name == MvcApplication.ADMINISTRATOR_GROUP)) {
				var store = new RoleStore<IdentityRole>(context);
				var manager = new ApplicationRoleManager(store);
				var role = new IdentityRole { Name = MvcApplication.ADMINISTRATOR_GROUP };

				await manager.CreateAsync(role);
			}

			var adminEmail = "admin@winerack.io";
			if (!context.Users.Any(u => u.Email == adminEmail)) {
				var manager = ApplicationUserManager.Create(context);
				var user = new User {
					CreatedOn = DateTime.Now,
					Email = adminEmail,
					UserName = adminEmail,
					FirstName = "Winerack",
					LastName = "Admin",
					EmailConfirmed = true
				};

				var result = await manager.CreateAsync(user, "P@ssw0rd");
				await manager.AddToRoleAsync(user.Id, MvcApplication.ADMINISTRATOR_GROUP);
			}

			return true;
		}

		private void Seed_Varietals(ApplicationDbContext context) {
			IList<Varietal> varietals = new List<Varietal>();

			varietals.Add(new Varietal { Name = "Cabernet", Style = VarietalStyles.Red });
			varietals.Add(new Varietal { Name = "Cabernet Franc", Style = VarietalStyles.Red });
			varietals.Add(new Varietal { Name = "Cabernet Merlot", Style = VarietalStyles.Red });
			varietals.Add(new Varietal { Name = "Cabernet Merlot Franc Malbec", Style = VarietalStyles.Red });
			varietals.Add(new Varietal { Name = "Cabernet Sauvignon", Style = VarietalStyles.Red });
			varietals.Add(new Varietal { Name = "Chardonnay", Style = VarietalStyles.White });
			varietals.Add(new Varietal { Name = "Fiano", Style = VarietalStyles.White });
			varietals.Add(new Varietal { Name = "Gewürztraminer", Style = VarietalStyles.White });
			varietals.Add(new Varietal { Name = "Malbec", Style = VarietalStyles.Red });
			varietals.Add(new Varietal { Name = "Merlot", Style = VarietalStyles.Red });
			varietals.Add(new Varietal { Name = "Merlot Cabernet Franc", Style = VarietalStyles.Red });
			varietals.Add(new Varietal { Name = "Montepulciano", Style = VarietalStyles.Red });
			varietals.Add(new Varietal { Name = "Muscato", Style = VarietalStyles.Sparkling });
			varietals.Add(new Varietal { Name = "Noble", Style = VarietalStyles.Dessert });
			varietals.Add(new Varietal { Name = "Pinot Gris", Style = VarietalStyles.White });
			varietals.Add(new Varietal { Name = "Pinot Noir", Style = VarietalStyles.Red });
			varietals.Add(new Varietal { Name = "Riesling", Style = VarietalStyles.White });
			varietals.Add(new Varietal { Name = "Rosé", Style = VarietalStyles.Other });
			varietals.Add(new Varietal { Name = "Sangiovese", Style = VarietalStyles.Red });
			varietals.Add(new Varietal { Name = "Sauvignon Blanc", Style = VarietalStyles.White });
			varietals.Add(new Varietal { Name = "Sémillon", Style = VarietalStyles.White });
			varietals.Add(new Varietal { Name = "Shiraz", Style = VarietalStyles.Red });
			varietals.Add(new Varietal { Name = "Syrah", Style = VarietalStyles.Red });
			varietals.Add(new Varietal { Name = "Viognier", Style = VarietalStyles.White });

			foreach (var varietal in varietals) {
				context.Varietals.AddOrUpdate(v => v.Name, varietal);
			}

			context.SaveChanges();
		}

		#endregion Private Methods

		#region Implementation

		protected override void Seed(ApplicationDbContext context) {
			// Identity
			var identityTask = Seed_Identity(context);
			var identityResult = identityTask.Result;

			// Regions
			//context.Regions.AddOrUpdate(
			//	r => new { r.Name, r.Country },
			//	new Region { Name = "Hawke's Bay", Country = "NZ" }
			//);

			//// Vineyards
			//context.Vineyards.AddOrUpdate(
			//	v => new { v.Name },
			//	new Vineyard { Name = "Askerne Estate" }
			//);

			//context.SaveChanges();

			// Varietals
			Seed_Varietals(context);

			// Wines
			//context.Wines.AddOrUpdate(
			//	w => new { w.VarietalID, w.RegionID, w.VineyardID },
			//	new Wine {
			//		VarietalID = 1,
			//		Vintage = 2008,
			//		RegionID = context.Regions.First().ID,
			//		VineyardID = context.Vineyards.First().ID
			//	}
			//);
		}

		#endregion Implementation
	}
}