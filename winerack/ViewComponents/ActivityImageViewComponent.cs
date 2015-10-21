using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using winerack.Models;
using winerack.Services;

namespace winerack.ViewComponents
{
  public class ActivityImageViewComponent : ViewComponent
  {
    #region Constructor

    public ActivityImageViewComponent(ApplicationDbContext dbContext, AzureService azure)
    {
      _dbContext = dbContext;
      _azure = azure;
    }

    #endregion Constructor

    #region Declarations

    private readonly ApplicationDbContext _dbContext;
    private readonly AzureService _azure;

    #endregion Declarations

    #region Private Methods

    private async Task<string> GetImageUrl(Activity activity)
    {
      Guid? imageId = null;
      var blobContainer = "";

      switch (activity.Verb)
      {
        case ActivityVerbs.Opened:
          var opening = await _dbContext.Openings.FirstOrDefaultAsync(x => x.StoredBottleID == activity.ObjectID);
          imageId = opening.ImageID;
          blobContainer = "openings";
          break;

        case ActivityVerbs.Purchased:
          var purchase = await _dbContext.Purchases.FirstOrDefaultAsync(x => x.ID == activity.ObjectID);
          imageId = purchase.ImageID;
          blobContainer = "purchases";
          break;

        case ActivityVerbs.Tasted:
          var tasting = await _dbContext.Tastings.FirstOrDefaultAsync(x => x.ID == activity.ObjectID);
          imageId = tasting.ImageID;
          blobContainer = "tastings";
          break;

        default:
          throw new NotImplementedException(activity.Verb.ToString());
      }

      return !imageId.HasValue ? null : _azure.GetUrl(blobContainer, $"{imageId.Value}_lg.jpg");
    }

    #endregion Private Methods

    #region Public Methods

    public async Task<IViewComponentResult> InvokeAsync(Activity activity)
    {
      var imageUrl = await GetImageUrl(activity);
      return View("Default", imageUrl);
    }

    #endregion Public Methods
  }
}