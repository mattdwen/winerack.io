using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace winerack.Models
{
    public class Wine
    {
        #region Constructor

        public Wine()
        {
            Styles = new List<Style>();
            Varietals = new List<Varietal>();
        }

        #endregion Constructor

        #region Properties

        public int ID { get; set; }

        public string Name { get; set; }

        [DisplayFormat(NullDisplayText = "NV")]
        public int? Vintage { get; set; }

        [Required(ErrorMessage = "Region is required")]
        [Display(Name = "Region")]
        [ForeignKey("Region")]
        public int RegionID { get; set; }

        [Required(ErrorMessage = "Vineyard is required")]
        [Display(Name = "Vineyard")]
        [ForeignKey("Vineyard")]
        public int VineyardID { get; set; }

        [NotMapped]
        public string Description
        {
            get
            {
                string description = "";

                if (!string.IsNullOrWhiteSpace(Name)) {
                    description += "'" + Name + "' ";
                }

                if (Vintage.HasValue) {
                    description += Vintage.ToString() + " ";
                }

                description += string.Join(" ", Varietals.Select(v => v.Name).ToList());

                return description;
            }
        }

        #endregion Properties

        #region Relationships

        [JsonIgnore]
        public virtual ICollection<Activity> Activities { get; set; }

        [JsonIgnore]
        [Required]
        public virtual ICollection<Style> Styles { get; set; }

        [JsonIgnore]
        [UIHint("Varietals")]
        [Required]
        public virtual ICollection<Varietal> Varietals { get; set; }

        public virtual Region Region { get; set; }

        public virtual Vineyard Vineyard { get; set; }

        #endregion Relationships

        #region Overrides

        public override string ToString()
        {
            string description = "";

            if (Vineyard != null) {
                description = Vineyard.Name + " ";
            }

            description += Description;

            return description;
        }

        #endregion Overrides
    }
}