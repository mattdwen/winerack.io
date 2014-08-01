using System;
using System.ComponentModel.DataAnnotations;

namespace winerack.Models {

	public class ExternalLoginConfirmationViewModel {

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}

	public class ExternalLoginListViewModel {

		public string Action { get; set; }

		public string ReturnUrl { get; set; }
	}

	public class ManageUserViewModel {

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Current password")]
		public string OldPassword { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "New password")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm new password")]
		[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	public class LoginViewModel {

		[Required]
		[Display(Name = "Username")]
		public string Username { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}

	public class RegisterViewModel {

		[Required]
		[Display(Name = "Username")]
		[MaxLength(255)]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		[MaxLength(255)]
		public string Email { get; set; }

		[Display(Name = "First Name")]
		[MaxLength(255)]
		public string FirstName { get; set; }

		[Display(Name = "Last Name")]
		[MaxLength(255)]
		public string LastName { get; set; }

		[MaxLength(255)]
		public string Location { get; set; }

		[Required]
		public string Country { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	public class ResetPasswordViewModel {

		[Required]
		[Display(Name = "Username")]
		public string Username { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		public string Code { get; set; }
	}

	public class SettingsViewModel {
		[Display(Name = "First Name")]
		[MaxLength(255)]
		[Required]
		public string FirstName { get; set; }

		[Display(Name = "Last Name")]
		[MaxLength(255)]
		[Required]
		public string LastName { get; set; }

		[MaxLength(255)]
		public string Location { get; set; }

		[Required]
		public string Country { get; set; }

		public Guid? ImageID { get; set; }

		public bool SocialTwitter { get; set; }

		public bool SocialFacebook { get; set; }

		public bool SocialTumblr { get; set; }
	}

	public class ForgotPasswordViewModel {

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}
}