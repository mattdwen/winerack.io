using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace my.winerack.io.Models {
	public class Vineyard {

		#region Properties

		public int ID { get; set; }
		public string Name { get; set; }

		#endregion Properties

		#region Relationships
		public virtual ICollection<Wine> Wines { get; set; }

		#endregion Relationships
	}
}