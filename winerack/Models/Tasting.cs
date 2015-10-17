using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace winerack.Models
{
  public class Tasting
  {
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

    public virtual ApplicationUser User { get; set; }

    public virtual Wine Wine { get; set; }

    #endregion Relationships
  }
}