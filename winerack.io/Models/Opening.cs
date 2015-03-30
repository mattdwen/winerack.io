using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace winerack.Models {

	/// <summary>
	/// Details the opening of a stored bottle
	/// </summary>
	public class Opening {

		#region Properties

		[Key, ForeignKey("StoredBottle")]
		public int StoredBottleID { get; set; }

		[Display(Name = "Opened On")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}", NullDisplayText = "<em>Unknown</em>")]
		public DateTime? OpenedOn { get; set; }

		public Guid? ImageID { get; set; }

		[Display(Name = "Tasting Notes")]
		[DataType(DataType.MultilineText)]
		public string Notes { get; set; }

        [UIHint("Rating")]
        public int? Rating { get; set; }

		#endregion

		#region Relationships

		[Display(Name = "Bottle")]
		public virtual StoredBottle StoredBottle { get; set; }

		#endregion
	}
}