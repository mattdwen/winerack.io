using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace winerack.Models {
	public class Friend {

		#region Declarations

		public int ID { get; set; }

		public DateTime CreatedOn { get; set; }

		[Required]
		[ForeignKey("Followee")]
		public string FolloweeID { get; set; }

		[Required]
		[ForeignKey("Follower")]
		public string FollowerID { get; set; }

		#endregion Declarations

		#region Relationships

		public virtual User Followee { get; set; }

		public virtual User Follower { get; set; }

		#endregion Relationships
	}
}