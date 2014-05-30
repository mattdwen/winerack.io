using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace winerack.Models {

	public class ApplicationDbContext : IdentityDbContext<User> {

		#region Constructor

		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false) {
		}

		public static ApplicationDbContext Create() {
			return new ApplicationDbContext();
		}

		#endregion Constructor

		#region Properties

		public DbSet<Bottle> Bottles { get; set; }

		public DbSet<Purchase> Purchases { get; set; }

		public DbSet<Region> Regions { get; set; }

		public DbSet<StoredBottle> StoredBottles { get; set; }

		public DbSet<Tasting> Tastings { get; set; }

		public DbSet<Vineyard> Vineyards { get; set; }

		public DbSet<Wine> Wines { get; set; }

		#endregion Properties

		#region Overrides
		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);
			modelBuilder.Conventions.Add(new DateTime2Convention());
		}
		#endregion

		#region Conventions

		public class DateTime2Convention : Convention {

			public DateTime2Convention() {
				this.Properties<DateTime>()
					.Configure(c => c.HasColumnType("datetime2"));
			}
		}

		#endregion Conventions
	}
}