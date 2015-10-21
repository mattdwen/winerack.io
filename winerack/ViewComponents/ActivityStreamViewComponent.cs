using Microsoft.AspNet.Mvc;
using winerack.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace winerack.ViewComponents
{
  public class ActivityStreamViewComponent : ViewComponent
  {
    #region Constructor

    public ActivityStreamViewComponent(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    #endregion Constructor

    #region Declarations

    private readonly ApplicationDbContext _dbContext;

    #endregion Declarations

    #region Public Methods

    public async Task<IViewComponentResult> InvokeAsync()
    {
      var userId = Context.User.GetUserId();

      var activity = await _dbContext.ActivityNotifications
        .Where(n => n.UserID == userId)
        .Select(a => a.Activity)
        .OrderByDescending(a => a.OccuredOn)
        .Take(20)
        .ToListAsync();

      return View(activity);
    }

    #endregion Public Methods
  }
}