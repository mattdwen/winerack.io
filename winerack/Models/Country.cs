using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace winerack.Models
{
  public class Country
  {
    #region Properties

    public string ID { get; set; }

    public string Name { get; set; }

    #endregion Properties

    #region Public Methods

    public static IEnumerable<Country> GetCountries()
    {
      return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
        .Select(x => new Country
        {
          ID = new RegionInfo(x.LCID).Name,
          Name = new RegionInfo(x.LCID).EnglishName
        })
        .GroupBy(c => c.ID)
        .Select(c => c.First())
        .OrderBy(x => x.Name);
    }

    #endregion Public Methods
  }
}