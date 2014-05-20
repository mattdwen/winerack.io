namespace my.winerack.io.Migrations
{
	using my.winerack.io.Models;
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<my.winerack.io.Models.ApplicationDbContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(my.winerack.io.Models.ApplicationDbContext context) {
			context.Regions.AddOrUpdate(
				new Region { Country = "NZ", Name = "Hawke's Bay" }
			);

			context.Vineyards.AddOrUpdate(
				new Vineyard { Name = "Askerne Estate" }
			);
        }
    }
}
