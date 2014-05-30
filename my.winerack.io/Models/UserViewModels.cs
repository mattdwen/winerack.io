using System;
using System.ComponentModel.DataAnnotations;

namespace winerack.Models {

	public class UserDetailsViewModel {

		#region Properties

		public string Id { get; set; }

		[Display(Name = "Joined")]
		[DataType(DataType.Date)]
		public DateTime CreatedOn { get; set; }

		public bool Administrator { get; set; }

		public string Email { get; set; }

		public string Name { get; set; }

		public bool Verified { get; set; }

		#endregion Properties
	}

	public class InviteUserViewModel {

		#region Properties

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[Display(Name = "First Name")]
		[MaxLength(255)]
		public string FirstName { get; set; }

		[Required]
		[Display(Name = "Last Name")]
		[MaxLength(255)]
		public string LastName { get; set; }

		#endregion Properties
	}
}