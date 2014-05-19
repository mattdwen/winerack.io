using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace my.winerack.io.Models {
	public class Wine {

		#region Constructor
		public Wine() {
			Region = new Region();
			Vineyard = new Vineyard();
		}
		#endregion

		#region Properties

		public int ID { get; set; }
		public string Name { get; set; }
		public string Varietal { get; set; }
		public int? Vintage { get; set; }

		#endregion Properties

		#region Relationships

		public virtual Region Region { get; set; }
		public virtual Vineyard Vineyard { get; set; }
		public virtual ICollection<Rack> Racks { get; set; }

		#endregion Relationships

	}
}