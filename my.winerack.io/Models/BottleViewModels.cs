using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace winerack.Models {
	public class CreateBottleViewModel {
		[Required]
		public string Vineyard { get; set; }

		public int VineyardID { get; set; }

		[Required]
		public string Region { get; set; }

		public int RegionID { get; set; }

		[Required]
		public string Country { get; set; }

		[Required]
		public string Varietal { get; set; }

		public int? Vintage { get; set; }

		[Display(Name = "Name")]
		public string WineName { get; set; }

		[Display(Name = "Date")]
		[Required]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime PurchaseDate { get; set; }

		[Display(Name = "Quantity")]
		[Range(1, int.MaxValue, ErrorMessage="Quantity must be at least 1")]
		public int PurchaseQuantity { get; set; }

		[Display(Name = "$ per bottle")]
		[DisplayFormat(DataFormatString = "{0:C}")]
		[DataType(DataType.Currency)]
		public decimal? PurchaseValue { get; set; }
	}
}