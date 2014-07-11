using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace winerack.Models {
	public class ActivityEvent {

		#region Properties

		public int ID { get; set; }

		[Required]
		[ForeignKey("User")]
		public string UserID { get; set; }

		[Required]
		public DateTime OccuredOn { get; set; }

		[Required]
		public ActivityVerbs Verb { get; set; }

		[Required]
		public int Noun { get; set; }

		#endregion Properties

		#region Relationships

		public virtual User User { get; set; }

		#endregion Relationships

	}

	public enum ActivityVerbs {
		Purchased,
		Opened
	}
}