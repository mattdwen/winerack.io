using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using winerack.Models;
using winerack.Services;
using winerack.ViewModels.Profile;

namespace winerack.Controllers
{
  [Authorize]
  public class ProfileController : Controller
  {
    #region Constructor

    public ProfileController(ApplicationDbContext dbContext, AzureService azure)
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

    private UserProfileViewModel GetProfileViewModel(ApplicationUser user)
    {
      var model = new UserProfileViewModel
      {
        Username = user.UserName,
        BannerImageUrl = "/images/banner-default.jpg",
        ProfileImageUrl = user.ImageID.HasValue ? _azure.GetUrl("profiles", $"{user.ImageID.Value}.jpg") : "/images/profile-default.png"
      };

      return model;
    }

    #endregion Private Methods

    #region Actions

    public async Task<IActionResult> Index()
    {
      var userId = HttpContext.User.GetUserId();
      var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

      if (user == null)
      {
        return View("Error");
      }

      var model = GetProfileViewModel(user);
      model.Editable = true;

      return View(model);
    }

    #endregion Actions
  }
}