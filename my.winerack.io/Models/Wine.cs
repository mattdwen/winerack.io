using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace my.winerack.io.Models {
	public class Wine {

		#region Properties

		public int ID { get; set; }

		public string Name { get; set; }

		[Required]
		public string Varietal { get; set; }

		[DisplayFormat(NullDisplayText = "NV")]
		public int? Vintage { get; set; }

		[Display(Name="Region")]
		[ForeignKey("Region")]
		public int RegionID { get; set; }

		[Display(Name="Vineyard")]
		[ForeignKey("Vineyard")]
		public int VineyardID { get; set; }

		#endregion Properties

		#region Relationships

		public virtual Region Region { get; set; }

		public virtual Vineyard Vineyard { get; set; }

		#endregion Relationships

	}
}