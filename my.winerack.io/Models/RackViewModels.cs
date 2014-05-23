using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace my.winerack.io.Models {
	public class RackIndexViewModel {
		//public IEnumerable<RackIndexWineViewModel> Wines { get; set; }
	}

	public class RackIndexBottleViewModel {
		public Vineyard Vineyard { get; set; }
		public Wine Wine { get; set; }
		public int Purchased { get; set; }
		public int Drunk { get; set; }
		public int Remaining {
			get { return Purchased - Drunk; }
		}
	}

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