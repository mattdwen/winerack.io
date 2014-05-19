using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace my.winerack.io.Models {
	public class WineRackDbContext : DbContext {
		public DbSet<Rack> Racks { get; set; }
		public DbSet<Region> Regions { get; set; }
		public DbSet<Vineyard> Vineyards { get; set; }
		public DbSet<Wine> Wines { get; set; }
	}
}