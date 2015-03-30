using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace winerack.Models.ActivityEventViewModels {
	public class Base {

		#region Properties

		public User Actor { get; set; }

		public DateTime OccuredOn { get; set; }

		public string Image { get; set; }

		public int ObjectID { get; set; }

		public string ViewUrl { get; set; }

		#endregion Properties
	}

	public class Opened : Base {

		#region Properties

		public string Notes { get; set; }

        [UIHint("Rating")]
        public int? Rating { get; set; }

        public Wine Wine { get; set; }

		#endregion Properties

	}

	public class Purchased : Opened {

        public bool IsGift { get; set; }

		public int Quantity { get; set; }

	}

	public class Tasted : Opened {

	}
}