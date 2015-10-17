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


    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    #endregion Properties

    #region Magic Properties

    [NotMapped]
    public string CountryName => new RegionInfo(Country).DisplayName;

    [NotMapped]
    public string Label => $"{Name}, {CountryName}";

    #endregion Magic Properties

    #region Relationshsips

    public virtual ICollection<Vineyard> Vineyards { get; set; }

    public virtual ICollection<Wine> Wines { get; set; }

    #endregion Relationshsips

    #region Public Methods

    public static IEnumerable<Region> GetRegions()
    {
      var context = new ApplicationDbContext();
      return context.Regions
        .OrderBy(x => x.Country)
        .ThenBy(x => x.Name);
    }

    public override string ToString()
    {
      return Name + ", " + CountryName;
    }

    #endregion Public Methods
  }
}