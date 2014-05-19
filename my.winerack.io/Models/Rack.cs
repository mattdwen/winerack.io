using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace my.winerack.io.Models {
	public class Rack {

		#region Properties

		public int ID { get; set; }
		public int OwnerID { get; set; }

		#endregion Properties

		#region Relationships

		public virtual ICollection<Wine> Wines { get; set; }

		#endregion Relationships

	}
}