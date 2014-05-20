namespace my.winerack.io.Migrations
{
	using my.winerack.io.Models;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<my.winerack.io.Models.ApplicationDbContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(my.winerack.io.Models.ApplicationDbContext context) {
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

			return;
			context.SaveChanges();

			// Wines
			context.Wines.AddOrUpdate(
				w => new { w.Name, w.RegionID, w.Varietal, VinyardID = w.VineyardID },
				new Wine {
					Varietal = "Merlot",
					Vintage = 2008,
					RegionID = context.Regions.Where(r => r.Name == "Hawke's Bay").First().ID,
					VineyardID = context.Vineyards.Where(v => v.Name == "Askerne Estate").First().ID
				}
			);
        }
    }
}
