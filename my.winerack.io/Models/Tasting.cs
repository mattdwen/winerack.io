﻿using System;
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

		[Display(Name = "Opened On")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}", NullDisplayText = "<em>Unknown</em>")]
		public DateTime? TastedOn { get; set; }

		public Guid? ImageID { get; set; }

		[Display(Name = "Tasting Notes")]
		[DataType(DataType.MultilineText)]
		public string Notes { get; set; }

		#endregion

		#region Relationships

		[Display(Name = "Bottle")]
		public virtual StoredBottle StoredBottle { get; set; }

		#endregion
	}
}