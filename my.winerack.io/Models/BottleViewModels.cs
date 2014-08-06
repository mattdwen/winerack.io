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
		[Display(Name = "Varietal")]
		public int VarietalID { get; set; }

		public int? Vintage { get; set; }

		[Display(Name = "Name")]
		public string WineName { get; set; }

		[Display(Name = "Purchased On")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime? PurchaseDate { get; set; }

		[Display(Name = "Number of bottles")]
		[Range(1, int.MaxValue, ErrorMessage="Quantity must be at least 1")]
		public int PurchaseQuantity { get; set; }

		[Display(Name = "$ per bottle")]
		[DisplayFormat(DataFormatString = "{0:C}")]
		[DataType(DataType.Currency)]
		public decimal? PurchaseValue { get; set; }

		[Display(Name = "Notes")]
		[DataType(DataType.MultilineText)]
		public string PurchaseNotes { get; set; }

		[Display(Name = "Who are you with?")]
		public List<string> Friends { get; set; }

		[Display(Name="No less than")]
		public int? CellarMin { get; set; }

		[Display(Name="No more than")]
		public int? CellarMax { get; set; }

		public bool HasFacebook { get; set; }

		public bool HasTumblr { get; set; }

		public bool HasTwitter { get; set; }

		public bool PostFacebook { get; set; }

		public bool PostTumblr { get; set; }

		public bool PostTwitter { get; set; }
	}

	public class CellarBottleViewModel {
		public int Min { get; set; }
		public int Max { get; set; }
	}
}

namespace winerack.Models.BottleViewModels {

	public class ApiBottle {

		public int ID { get; set; }

		public string Description { get; set; }

		public string Vineyard { get; set; }

		public string Region { get; set; }

		public int? CellarMin { get; set; }

		public int? CellarMax { get; set; }

		public string Varietal { get; set; }

		public string VarietalStyle { get; set; }

		public int? Vintage { get; set; }

		public int Purchased { get; set; }

		public int Opened { get; set; }

		public int Remaining { get; set; }
	}
}