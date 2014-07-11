using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace winerack.Models.ActivityEventViewModels {
	public class Base {

		#region Properties

		public DateTime OccuredOn { get; set; }

		public string Username { get; set; }

		#endregion Properties
	}

	public class Opened : Base {

		#region Properties

		public string Bottle { get; set; }

		public string Winery { get; set; }

		public string Notes { get; set; }

		#endregion Properties

	}

	public class Purchased : Opened {

		public int Quantity { get; set; }

	}
}