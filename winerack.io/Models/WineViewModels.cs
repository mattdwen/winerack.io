using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using winerack.Validation;

namespace winerack.Models.WineViewModels
{
    public class WineCreateAndEditModel
    {
        #region Constructor

        public WineCreateAndEditModel()
        {
            Styles = new List<int>();
            Varietals = new List<int>();
        }

        #endregion Constructor

        #region Properties

        [Display(Name = "Vineyard")]
        [Required(ErrorMessage = "Vineyard is required")]
        public int VineyardID { get; set; }

        [Display(Name = "Region")]
        [Required(ErrorMessage = "Region is required")]
        public int RegionID { get; set; }

        [AtLeastOneItem(ErrorMessage = "Select at least one style")]
        public List<int> Styles { get; set; }

        [AtLeastOneItem(ErrorMessage = "Select at least one varietal")]
        public List<int> Varietals { get; set; }

        public int? Vintage { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        #endregion Properties
    }
}