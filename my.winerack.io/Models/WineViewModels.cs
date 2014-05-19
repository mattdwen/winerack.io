using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace my.winerack.io.Models {
	public class WineViewModel {
		public string Vineyard { get; set; }
		public string Region { get; set; }
		public string Country { get; set; }

		public string Name { get; set; }
		public string Varietal { get; set; }
		public int? Vintage { get; set; }
	}
}