using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace winerack.Models
{
  public class Credentials
  {
    #region Properties

    public int ID { get; set; }

    [ForeignKey("User")]
    public string UserID { get; set; }

    [Required]
    public CredentialTypes CredentialType { get; set; }

    public string Key { get; set; }

    public string Secret { get; set; }

    public string Data1 { get; set; }

    #endregion Properties

    #region Relationships

    public virtual ApplicationUser User { get; set; }

    #endregion Relationships
  }

  public enum CredentialTypes
  {
    Twitter = 100,
    Facebook = 200,
    Tumblr = 300
  }
}