using System;
using System.ComponentModel.DataAnnotations;

namespace winerack.ViewModels
{
    public class TastingEditViewModel : WineEditViewModel
    {
        #region Properties

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime TastingDate { get; set; }

        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string TastingNotes { get; set; }

        #endregion Properties
    }
}