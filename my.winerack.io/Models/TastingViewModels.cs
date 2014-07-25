using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace winerack.Models.TastingViewModels {

	public class Create {

		#region Properties
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

		[Display(Name = "Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime TastingDate { get; set; }

		[Display(Name = "Notes")]
		[DataType(DataType.MultilineText)]
		public string TastingNotes { get; set; }

		#endregion

	}

}