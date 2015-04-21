using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;

namespace winerack.Models
{
    public class Region
    {
        #region Properties

        public int ID { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        [NotMapped]
        public string CountryName
        {
            get
            {
                return new RegionInfo(Country).DisplayName;
            }
        }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [NotMapped]
        public string Label
        {
            get
            {
                return Name + ", " + CountryName;
            }
        }

        #endregion Properties

        #region Relationshsips

        [JsonIgnore]
        public virtual ICollection<Vineyard> Vineyards { get; set; }

        [JsonIgnore]
        public virtual ICollection<Wine> Wines { get; set; }

        #endregion Relationshsips

        #region Public Methods

        public static IEnumerable<Region> GetRegions()
        {
            var context = new ApplicationDbContext();
            return context.Regions
                .OrderBy(r => r.Country)
                .OrderBy(r => r.Name);
        }

        public override string ToString()
        {
            return Name + ", " + CountryName;
        }

        #endregion Public Methods
    }
}