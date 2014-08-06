using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace winerack.Models.PurchaseViewModels {
	public class Create {

		[Required]
		public int BottleID { get; set; }

		public Bottle Bottle { get; set; }

		[Display(Name="Number of bottles")]
		[Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
		public int Quantity { get; set; }

		[Display(Name = "Purchased on")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime? PurchasedOn { get; set; }

		[Display(Name = "$ per bottle")]
		[DisplayFormat(DataFormatString = "{0:C}")]
		[DataType(DataType.Currency)]
		public decimal? PurchasePrice { get; set; }

		public string Notes { get; set; }

		[Display(Name = "Who are you with?")]
		public List<string> Friends { get; set; }

		public bool HasFacebook { get; set; }

		public bool HasTumblr { get; set; }

		public bool HasTwitter { get; set; }

		public bool PostFacebook { get; set; }

		public bool PostTumblr { get; set; }

		public bool PostTwitter { get; set; }
	}
}