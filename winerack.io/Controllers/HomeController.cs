using System.Web.Mvc;

namespace winerack.Controllers
{
    public class HomeController : Controller
    {
        #region Actions

        public ActionResult Index()
        {
            if (Request.IsAuthenticated) {
                return View("IndexLoggedIn");
            }
            return View();
        }

        public ActionResult AccessDenied()
        {
            Response.StatusCode = 403;
            return View();
        }

        #endregion Actions
    }
}