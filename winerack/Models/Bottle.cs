using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace winerack.Models
{
  public class Bottle
  {
    #region Properties

    public int ID { get; set; }

    [Required]
    [Display(Name = "Wine")]
    [ForeignKey("Wine")]
    public int WineID { get; set; }

    [Required]
    [Display(Name = "Owner")]
    [ForeignKey("Owner")]
    public string OwnerID { get; set; }

    [Required]
    [Display(Name = "Added")]
    public DateTime CreatedOn { get; set; }

    public int? CellarMin { get; set; }

    public int? CellarMax { get; set; }

    #endregion Properties

    #region Magic Properties

    [NotMapped]
    [Display(Name = "Purchased")]
    public int NumberOfBottles
    {
      get
      {
        return Purchases?.Sum(p => p.StoredBottles.Count) ?? 0;
      }
    }

    [NotMapped]
    [Display(Name = "Opened")]
    public int NumberDrunk
    {
      get
      {
        return Purchases?.SelectMany(p => p.StoredBottles).Count(s => s.Opening != null) ?? 0;
      }
    }

    [NotMapped]
    [Display(Name = "Remaining")]
    public int NumberRemaining
    {
      get
      {
        if (Purchases == null) return 0;
        return NumberOfBottles - NumberDrunk;
      }
    }

    #endregion Magic Properties

    #region Relationships

    public virtual Wine Wine { get; set; }

    public virtual ApplicationUser Owner { get; set; }

    [Display(Name = "Purchased")]
    public virtual ICollection<Purchase> Purchases { get; set; }

    #endregion Relationships
  }
}