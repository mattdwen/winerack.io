using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using winerack.Models;

namespace winerack.ViewComponents
{
  public class ActivityViewComponent : ViewComponent
  {
    #region Constructor

    public ActivityViewComponent(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    #endregion Constructor

    #region Declarations

    private readonly ApplicationDbContext _dbContext;

    #endregion Declarations

    #region Public Methods

    public async Task<IViewComponentResult> InvokeAsync(Activity activity)
    {
      if (activity.WineID.HasValue)
      {
        activity.Wine = await _dbContext.Wines
          .Include(w => w.Vineyard)
          .FirstOrDefaultAsync(w => w.ID == activity.WineID);
      }

      switch (activity.Verb)
      {
        case ActivityVerbs.Opened:
          return View("Opened", activity);

        case ActivityVerbs.Purchased:
          return View("Purchased", activity);

        case ActivityVerbs.Tasted:
          return View("Tasted", activity);

        default:
          throw new NotImplementedException(activity.Verb.ToString());
      }
    }

    #endregion Public Methods
  }
}