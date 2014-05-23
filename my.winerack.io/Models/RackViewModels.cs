using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace my.winerack.io.Models {
	public class AddWineViewModel {
		[Required]
		[Display(Name = "Wine")]
		public int WineID { get; set; }

		[Required]
		[Range(1, int.MaxValue)]
		public int Quantity { get; set; }

		public decimal Price { get; set; }

		[Display(Name="Purchase Date")]
		[DataType("date")]
		public string PurchaseDate { get; set; }

		public IEnumerable<Wine> Wines { get; set; }
	}
}