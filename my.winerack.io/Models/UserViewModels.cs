using System;
using System.ComponentModel.DataAnnotations;

namespace winerack.Models {

	public class InviteUserViewModel {

		#region Properties

		[Required]
		public string Username { get; set; }

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

	public class MiniProfileViewModel {

		#region Properties

		public string Name { get; set; }

		public string Username { get; set; }

		public string Location { get; set; }

		public int BottlesTotal { get; set; }

		public int BottlesUnique { get; set; }

		public int BottlesDrunk { get; set; }

		public int Tasted { get; set; }

		#endregion Properties
	}
}