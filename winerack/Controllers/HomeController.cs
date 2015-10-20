using System.Security.Claims;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace winerack.Controllers
{
  public class HomeController : Controller
  {
    #region Actions

    public IActionResult Index()
    {
      if (HttpContext.User.IsSignedIn())
      {
        return RedirectToAction("Home");
      }

      return View();
    }

    [Authorize]
    public IActionResult Home()
    {
      return View();
    }

    public IActionResult Error()
    {
      return View("~/Views/Shared/Error.cshtml");
    }

    #endregion Actions
  }
}