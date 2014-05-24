using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace my.winerack.io.Models {

	public class Bottle {

		#region Properties

		public int ID { get; set; }

		[Required]
		[Display(Name = "Wine")]
		[ForeignKey("Wine")]
		public int WineID { get; set; }

		[Required]
		[Display(Name = "Owner")]
		[ForeignKey("Owner")]
		public string OwnerID { get; set; }

		[Required]
		[Display(Name = "Added")]
		public DateTime CreatedOn { get; set; }

		#endregion Properties

		#region Relationships

		public virtual Wine Wine { get; set; }

		public virtual User Owner { get; set; }

		[Display(Name = "Purchased")]
		public virtual ICollection<Purchase> Purchases { get; set; }

		#endregion Relationships
	}
}