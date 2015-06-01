using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using winerack.Validation;

namespace winerack.ViewModels
{
    public class WineEditViewModel
    {
        #region Constructor

        public WineEditViewModel()
        {
            Styles = new List<int>();
            Varietals = new List<int>();
        }

        #endregion Constructor

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

        [Display(Name = "Name")]
        public string WineName { get; set; }

        public int? Vintage { get; set; }

        public bool HasFacebook { get; set; }

        public bool HasTumblr { get; set; }

        public bool HasTwitter { get; set; }

        public bool PostFacebook { get; set; }

        public bool PostTumblr { get; set; }

        public bool PostTwitter { get; set; }

        #endregion Properties
    }
}