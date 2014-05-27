namespace winerack.Migrations {

	using Microsoft.AspNet.Identity.EntityFramework;
	using winerack.Models;
	using System.Data.Entity.Migrations;
	using System.Linq;
	using System.Threading.Tasks;

	internal sealed class Configuration : DbMigrationsConfiguration<winerack.Models.ApplicationDbContext> {

		public Configuration() {
			AutomaticMigrationsEnabled = false;
		}

		#region Private Methods

		private async Task<bool> SeedIdentity(ApplicationDbContext context) {
			if (!context.Roles.Any(r => r.Name == Helpers.ADMINISTRATOR_GROUP)) {
				var store = new RoleStore<IdentityRole>(context);
				var manager = new ApplicationRoleManager(store);
				var role = new IdentityRole { Name = Helpers.ADMINISTRATOR_GROUP };

				await manager.CreateAsync(role);
			}

			var adminEmail = "admin@winerack.io";
			if (!context.Users.Any(u => u.Email == adminEmail)) {
				var manager = ApplicationUserManager.Create(context);
				var user = new User {
					Email = adminEmail,
					UserName = adminEmail,
					EmailConfirmed = true
				};

				var result = await manager.CreateAsync(user, "P@ssw0rd");
				await manager.AddToRoleAsync(user.Id, Helpers.ADMINISTRATOR_GROUP);
			}

			return true;
		}

		#endregion Private Methods

		protected override void Seed(winerack.Models.ApplicationDbContext context) {
			// Identity
			var identityTask = SeedIdentity(context);
			var identityResult = identityTask.Result;

			// Regions
			context.Regions.AddOrUpdate(
				r => new { r.Name, r.Country },
				new Region { Name = "Hawke's Bay", Country = "NZ" }
			);

			// Vineyards
			context.Vineyards.AddOrUpdate(
				v => new { v.Name },
				new Vineyard { Name = "Askerne Estate" }
			);

			context.SaveChanges();

			// Wines
			context.Wines.AddOrUpdate(
				w => new { w.Varietal, w.RegionID, w.VineyardID },
				new Wine {
					Varietal = "Merlot",
					Vintage = 2008,
					RegionID = context.Regions.First().ID,
					VineyardID = context.Vineyards.First().ID
				}
			);
		}
	}
}