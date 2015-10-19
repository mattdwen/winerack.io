using System.ComponentModel.DataAnnotations;

namespace winerack.ViewModels.Account
{
  public class SignViewModel
  {
    [Required]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; } = true;
  }
}