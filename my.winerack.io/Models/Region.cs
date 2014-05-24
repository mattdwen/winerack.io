using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Web;

namespace my.winerack.io.Models {
	public class Region {

		#region Properties

		public int ID { get; set; }

		[Required]
		public string Country { get; set; }

		[NotMapped]
		public string CountryName {
			get {
				return new RegionInfo(Country).DisplayName;
			}
		}

		[Required]
		public string Name { get; set; }

		[NotMapped]
		public string Label {
			get {
				return Name + ", " + CountryName;
			}
		}

		#endregion

		#region Relationshsips

		[JsonIgnore]
		public virtual ICollection<Wine> Wines { get; set; }

		#endregion Relationships

		#region Public Methods

		public static IEnumerable<Region> GetRegions() {
			var context = new ApplicationDbContext();
			return context.Regions
				.OrderBy(r => r.Country)
				.OrderBy(r => r.Name);
		}

		#endregion Public Methods

	}
}