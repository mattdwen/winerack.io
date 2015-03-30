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

        [Display(Name = "Is a gift")]
        public bool IsGift { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
		public int Quantity { get; set; }

		[Display(Name = "Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime? PurchasedOn { get; set; }

		[Display(Name = "$ per bottle")]
		[DisplayFormat(DataFormatString = "{0:C}")]
		[DataType(DataType.Currency)]
		public decimal? PurchasePrice { get; set; }

		public string Notes { get; set; }

		public bool HasFacebook { get; set; }

		public bool HasTumblr { get; set; }

		public bool HasTwitter { get; set; }

		public bool PostFacebook { get; set; }

		public bool PostTumblr { get; set; }

		public bool PostTwitter { get; set; }
	}
}