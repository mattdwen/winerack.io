using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace winerack.Models
{
  public class Purchase
  {
    #region Properties

    public int ID { get; set; }

    [Required]
    [ForeignKey("Bottle")]
    public int BottleID { get; set; }

    public Guid? ImageID { get; set; }

    [Display(Name = "Is a gift")]
    public bool IsGift { get; set; }

    [NotMapped]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

    [Display(Name = "Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? PurchasedOn { get; set; }

    [Column(TypeName = "Money")]
    [Display(Name = "$ per bottle")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [DataType(DataType.Currency)]
    public decimal? PurchasePrice { get; set; }

    public string Notes { get; set; }

    #endregion Properties

    #region Relationships

    public virtual Bottle Bottle { get; set; }

    public virtual ICollection<StoredBottle> StoredBottles { get; set; } = new List<StoredBottle>();

    #endregion Relationships
  }
}