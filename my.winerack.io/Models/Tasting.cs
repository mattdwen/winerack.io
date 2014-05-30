using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace winerack.Models {
	public class Tasting {

		#region Properties

		[Key, ForeignKey("StoredBottle")]
		public int StoredBottleID { get; set; }

		[Display(Name = "Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(NullDisplayText = "<em>Unknown</em>")]
		public DateTime? TastedOn { get; set; }

		[DataType(DataType.MultilineText)]
		public string Notes { get; set; }

		#endregion

		#region Relationships

		[Display(Name = "Bottle")]
		public virtual StoredBottle StoredBottle { get; set; }

		#endregion
	}
}