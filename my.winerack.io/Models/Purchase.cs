using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace my.winerack.io.Models {

	public class Purchase {

		#region Properties

		public int ID { get; set; }

		[Required]
		[ForeignKey("Bottle")]
		public int BottleID { get; set; }

		public int Quantity { get; set; }

		public DateTime PurchasedOn { get; set; }

		[Column(TypeName="Money")]
		public decimal PurchasePrice { get; set; }

		#endregion Properties

		#region Relationships

		public virtual Bottle Bottle { get; set; }

		#endregion Relationships
	}
}