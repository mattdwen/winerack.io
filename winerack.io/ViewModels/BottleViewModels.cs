using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace winerack.ViewModels
{
    public class BottleEditViewModel : WineEditViewModel
    {
        #region Properties

        [Display(Name = "Is a gift")]
        public bool IsGift { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? PurchaseDate { get; set; }

        [Display(Name = "Quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int PurchaseQuantity { get; set; }

        [Display(Name = "$ per bottle")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [DataType(DataType.Currency)]
        public decimal? PurchaseValue { get; set; }

        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string PurchaseNotes { get; set; }

        [Display(Name = "At least")]
        public int? CellarMin { get; set; }

        [Display(Name = "No more than")]
        public int? CellarMax { get; set; }

        #endregion
    }
}