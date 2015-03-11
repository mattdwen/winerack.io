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

			if (!context.Users.Any(u => u.UserName == "winerack")) {
				var manager = ApplicationUserManager.Create(context);
				var user = new User {
					CreatedOn = DateTime.Now,
					Email = "admin@winerack.io",
					UserName = "winerack",
					FirstName = "Winerack",
					LastName = "Admin",
					Location = "Hawke's Bay",
					Country = "NZ",
					EmailConfirmed = true
				};

				var result = await manager.CreateAsync(user, "P@ssw0rd");
				await manager.AddToRoleAsync(user.Id, MvcApplication.ADMINISTRATOR_GROUP);
			}

			return true;
		}

        private void Seed_Styles(ApplicationDbContext context) {
            IList<Style> styles = new List<Style>();

            styles.Add(new Style { Name = "Red" });
            styles.Add(new Style { Name = "White" });
            styles.Add(new Style { Name = "Rosé" });
            styles.Add(new Style { Name = "Dessert" });
            styles.Add(new Style { Name = "Sparkling" });

            foreach(var style in styles) {
                context.Styles.AddOrUpdate(s => s.Name, style);
            }

            context.SaveChanges();
        }

		private void Seed_Varietals(ApplicationDbContext context) {
			IList<Varietal> varietals = new List<Varietal>();

			varietals.Add(new Varietal { Name = "Cabernet" });
			varietals.Add(new Varietal { Name = "Cabernet Franc" });
			varietals.Add(new Varietal { Name = "Cabernet Sauvignon" });
			varietals.Add(new Varietal { Name = "Chardonnay" });
			varietals.Add(new Varietal { Name = "Fiano" });
			varietals.Add(new Varietal { Name = "Gewürztraminer" });
			varietals.Add(new Varietal { Name = "Malbec" });
			varietals.Add(new Varietal { Name = "Merlot" });
			varietals.Add(new Varietal { Name = "Montepulciano" });
			varietals.Add(new Varietal { Name = "Muscato" });
			varietals.Add(new Varietal { Name = "Pinot Gris" });
			varietals.Add(new Varietal { Name = "Pinot Noir" });
			varietals.Add(new Varietal { Name = "Riesling" });
			varietals.Add(new Varietal { Name = "Rosé" });
			varietals.Add(new Varietal { Name = "Sangiovese" });
			varietals.Add(new Varietal { Name = "Sauvignon Blanc" });
			varietals.Add(new Varietal { Name = "Sémillon" });
			varietals.Add(new Varietal { Name = "Shiraz" });
			varietals.Add(new Varietal { Name = "Syrah" });
			varietals.Add(new Varietal { Name = "Viognier" });

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

            // Styles
            Seed_Styles(context);

			// Varietals
			Seed_Varietals(context);
		}

		#endregion Implementation
	}
}