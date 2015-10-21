using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using winerack.Models;

namespace winerack.ViewComponents
{
  public class WineInfoViewComponent : ViewComponent
  {
    #region Constructor

    public WineInfoViewComponent(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    #endregion Constructor

    #region Declarations

    private readonly ApplicationDbContext _dbContext;

    #endregion Declarations

    #region Public Methods

    public async Task<IViewComponentResult> InvokeAsync(int wineId)
    {
      var wine = await _dbContext.Wines
        .Include(x => x.Vineyard)
        .Include(x => x.Region)
        .Include(x => x.Varietals)
        .ThenInclude(v => v.Varietal)
        .FirstOrDefaultAsync(x => x.ID == wineId);
      return View(wine);
    }

    #endregion Public Methods
  }
}