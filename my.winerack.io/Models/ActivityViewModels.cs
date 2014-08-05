using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace winerack.Models.ActivityEventViewModels {
	public class Base {

		#region Constructor

		public Base() {
			TaggedUsers = new List<TaggedUser>();
		}

		#endregion Constructor

		#region Properties

		public User Actor { get; set; }

		public DateTime OccuredOn { get; set; }

		public string Image { get; set; }

		public int ObjectID { get; set; }

		public string ViewUrl { get; set; }

		public IList<TaggedUser> TaggedUsers { get; set; }

		#endregion Properties
	}

	public class Opened : Base {

		#region Properties

		public string Bottle { get; set; }

		public string Winery { get; set; }

		public string Notes { get; set; }

		public int WineID { get; set; }

		public int VineyardID { get; set; }

		#endregion Properties

	}

	public class Purchased : Opened {

		public int Quantity { get; set; }

	}

	public class Tasted : Opened {

	}
}