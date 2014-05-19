using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace my.winerack.io.Models {
	public class Country {
		public string ID { get; set; }
		public string Name { get; set; }

		public static IEnumerable<Country> GetCountries() {
			return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
				.Select(x => new Country {
					ID = new RegionInfo(x.LCID).Name,
					Name = new RegionInfo(x.LCID).EnglishName
				})
				.GroupBy(c => c.ID)
				.Select(c => c.First())
				.OrderBy(x => x.Name);
		}
	}
}