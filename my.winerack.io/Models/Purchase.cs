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

		[Range(0,int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
		public int Quantity { get; set; }

		[Display(Name="Date")]
		[DataType("date")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime PurchasedOn { get; set; }

		[Column(TypeName="Money")]
		[Display(Name = "$ per bottle")]
		[DisplayFormat(DataFormatString="{0:C}")]
		[DataType("number")]
		public decimal PurchasePrice { get; set; }

		#endregion Properties

		#region Relationships

		public virtual Bottle Bottle { get; set; }

		#endregion Relationships
	}
}