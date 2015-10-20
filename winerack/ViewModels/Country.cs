using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace winerack.ViewModels
{
  public class Country
  {
    #region Properties

    public string Id { get; set; }

    public string Name { get; set; }

    #endregion Properties

    #region Public Methods

    public static IEnumerable<Country> GetCountries()
    {
      return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
        .Select(x => new Country
        {
          Id = new RegionInfo(x.LCID).Name,
          Name = new RegionInfo(x.LCID).EnglishName
        })
        .GroupBy(c => c.Id)
        .Select(c => c.First())
        .OrderBy(x => x.Name);
    }

    #endregion Public Methods
  }
}