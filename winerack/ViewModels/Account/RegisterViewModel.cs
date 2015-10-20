using System.ComponentModel.DataAnnotations;

namespace winerack.ViewModels.Account
{
  public class RegisterViewModel
  {
    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Display(Name = "First name")]
    [MaxLength(255)]
    public string FirstName { get; set; }

    [Display(Name = "Last name")]
    [MaxLength(255)]
    public string LastName { get; set; }

    [MaxLength(255)]
    public string Location { get; set; }

    [Required]
    [MaxLength(255)]
    public string Country { get; set; }
  }
}