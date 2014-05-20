using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace my.winerack.io.Models {
	public class ApplicationDbContext : IdentityDbContext<User> {
		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false) {
		}

		public static ApplicationDbContext Create() {
			return new ApplicationDbContext();
		}

		public DbSet<Region> Regions { get; set; }
		public DbSet<Vineyard> Vineyards { get; set; }
		public DbSet<Wine> Wines { get; set; }
	}
}