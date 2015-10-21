using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace winerack.Models
{
  public class Varietal
  {
    #region Properties

    public int ID { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    #endregion Properties

    #region Relationships

    public virtual ICollection<WineVarietals> Wines { get; set; }

    #endregion Relationships

    #region Public Methods

    public static IEnumerable<Varietal> GetVarietals()
    {
      var context = new ApplicationDbContext();
      return context.Varietals
        .OrderBy(r => r.Name);
    }

    #endregion Public Methods
  }
}