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

		[NotMapped]
		[Display(Name = "Average Price")]
		[DisplayFormat(NullDisplayText = "N/A")]
		public decimal? AveragePrice {
			get {
				var purchased = this.Purchases.Where(p => p.PurchasePrice > 0).Sum(p => p.Quantity);
				var total = this.Purchases.Where(p => p.PurchasePrice > 0).Sum(p => p.PurchasePrice);
				if (purchased > 0 && total > 0) {
					return total / purchased;
				}

				return null;
			}
		}

		#endregion Properties

		#region Relationships

		public virtual Wine Wine { get; set; }

		public virtual User Owner { get; set; }

		[Display(Name = "Purchased")]
		public virtual ICollection<Purchase> Purchases { get; set; }

		#endregion Relationships
	}
}