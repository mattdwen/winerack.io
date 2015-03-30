using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace winerack.Models.OpeningViewModels {
	public class Create {

		public StoredBottle StoredBottle { get; set; }

		public int StoredBottleID { get; set; }

		[Display(Name = "Opened On")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}", NullDisplayText = "<em>Unknown</em>")]
		public DateTime? OpenedOn { get; set; }

        public int Rating { get; set; }

		[Display(Name = "Tasting Notes")]
		[DataType(DataType.MultilineText)]
		public string Notes { get; set; }

		public bool HasFacebook { get; set; }

		public bool HasTumblr { get; set; }

		public bool HasTwitter { get; set; }

		public bool PostFacebook { get; set; }

		public bool PostTumblr { get; set; }

		public bool PostTwitter { get; set; }
	}
}