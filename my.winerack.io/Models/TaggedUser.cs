using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace winerack.Models {

	public class TaggedUser {

		#region Declarations

		public int ID { get; set; }

		public int ParentID { get; set; }

		[ForeignKey("User")]
		public string UserID { get; set; }

		public string AltUserID { get; set; }

		public ActivityVerbs ActivityVerb { get; set; }

		public TaggedUserTypes UserType { get; set; }

		public string Name { get; set; }

		#endregion Declarations

		#region Relationships

		public virtual User User { get; set; }

		#endregion Relationships
	}

	public enum TaggedUserTypes {
		Winerack = 100,
		Facebook = 200
	}

}