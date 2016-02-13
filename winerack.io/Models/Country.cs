using System.Collections.Generic;
using System.Linq;

namespace winerack.Models {
	public class Country {
		public string ID { get; set; }
		public string Name { get; set; }

		public static IEnumerable<Country> GetCountries() {
      return ISO3166.Country.List
        .OrderBy(x => x.Name)
        .Select(x => new Country {
          ID = x.TwoLetterCode,
          Name = x.Name
        });
		}
	}
}