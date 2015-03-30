using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using winerack.Validation;

namespace winerack.Models
{
    public class CreateBottleViewModel
    {
        #region Constructor
        public CreateBottleViewModel()
        {
            Styles = new List<int>();
            Varietals = new List<int>();
        }
        #endregion

        #region Properties

        [Required(ErrorMessage = "Vineyard Name is required")]
        public string Vineyard { get; set; }

        public int VineyardID { get; set; }

        [Required(ErrorMessage = "Region is required")]
        public string Region { get; set; }

        public int RegionID { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        [Display(Name = "Style")]
        [AtLeastOneItem(ErrorMessage = "Select at least one style")]
        public List<int> Styles { get; set; }

        [Display(Name = "Varietals")]
        [AtLeastOneItem(ErrorMessage = "Select at least one varietal")]
        public List<int> Varietals { get; set; }

        public int? Vintage { get; set; }

        [Display(Name = "Name")]
        public string WineName { get; set; }

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

        public bool HasFacebook { get; set; }

        public bool HasTumblr { get; set; }

        public bool HasTwitter { get; set; }

        public bool PostFacebook { get; set; }

        public bool PostTumblr { get; set; }

        public bool PostTwitter { get; set; }

        #endregion Properties
    }

    public class CellarBottleViewModel
    {
        public int Min { get; set; }

        public int Max { get; set; }
    }
}

namespace winerack.Models.BottleViewModels
{
    public class ApiBottle
    {
        public int ID { get; set; }

        public string Description { get; set; }

        public string Vineyard { get; set; }

        public string Region { get; set; }

        public int? CellarMin { get; set; }

        public int? CellarMax { get; set; }

        public List<string> Styles { get; set; }

        public List<string> Varietals { get; set; }

        public int? Vintage { get; set; }

        public int Purchased { get; set; }

        public int Opened { get; set; }

        public int Remaining { get; set; }
    }
}