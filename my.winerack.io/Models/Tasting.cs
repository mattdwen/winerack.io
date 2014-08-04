using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace winerack.Models {

	public class Tasting {

		#region Declarations

		public Tasting() {
			TaggedUsers = new List<User>();
		}

		#endregion Declarations

		#region Declarations

		public int ID { get; set; }

		[ForeignKey("User")]
		public string UserID { get; set; }

		[ForeignKey("Wine")]
		public int WineID { get; set; }

		public Guid? ImageID { get; set; }

		[Display(Name = "Tasted On")]
		[DataType(DataType.Date)]
		public DateTime TastedOn { get; set; }

		[Display(Name = "Tasting Notes")]
		[DataType(DataType.MultilineText)]
		public string Notes { get; set; }

		#endregion Declarations

		#region Relationships

		public virtual IList<User> TaggedUsers { get; set; }

		public virtual User User { get; set; }

		public virtual Wine Wine { get; set; }

		#endregion Relationships
	}

}