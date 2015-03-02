using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.Linq;

namespace winerack.Models {

    public class Wine
    {

        #region Constructor
        public Wine()
        {
            Varietals = new List<Varietal>();
        }
        #endregion

        #region Properties

        public int ID { get; set; }

        public string Name { get; set; }

        [DisplayFormat(NullDisplayText = "NV")]
        public int? Vintage { get; set; }

        [Required(ErrorMessage="Region is required")]
        [Display(Name = "Region")]
        [ForeignKey("Region")]
        public int RegionID { get; set; }

        [Required(ErrorMessage="Vineyard is required")]
        [Display(Name = "Vineyard")]
        [ForeignKey("Vineyard")]
        public int VineyardID { get; set; }

        [NotMapped]
        public string Description {
            get {
                string description = "";
                if (!string.IsNullOrWhiteSpace(Name)) {
                    description = Name + " ";
                }

                if (Vintage.HasValue) {
                    description += "'" + Vintage.ToString().Substring(2) + " ";
                }

                description += string.Join(" ", Varietals.Select(v => v.Name).ToList());

                return description;
            }
        }

        [NotMapped]
        public VarietalStyles Style
        {
            get
            {
                if (Varietals.Any(v => v.Style == VarietalStyles.Dessert)) {
                    return VarietalStyles.Dessert;
                }

                return VarietalStyles.Other;
            }
        }

        #endregion Properties

        #region Relationships

        [JsonIgnore]
        [UIHint("Varietals")]
        public virtual ICollection<Varietal> Varietals { get; set; }

        public virtual Region Region { get; set; }

        public virtual Vineyard Vineyard { get; set; }

        #endregion Relationships

        #region Overrides

        public override string ToString() {
            string description = "";

            if (Vineyard != null) {
                description = Vineyard.Name + " ";
            }

            if (!string.IsNullOrWhiteSpace(Name)) {
                description = "'" + Name + "' ";
            }

            if (Vintage.HasValue) {
                description += "'" + Vintage.ToString().Substring(2) + " ";
            }

            description += string.Join(" ", Varietals.Select(v => v.Name));

            return description;
        }

        #endregion
    }
}