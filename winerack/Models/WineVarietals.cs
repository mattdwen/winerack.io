namespace winerack.Models
{
  public class WineVarietals
  {
    #region Properties

    public int WineId { get; set; }

    public int VarietalId { get; set; }

    #endregion Properties

    #region Relvationships

    public Wine Wine { get; set; }

    public Varietal Varietal { get; set; }

    #endregion Relationships
  }
}