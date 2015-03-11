using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Microsoft.AspNet.Identity;


namespace winerack.Helpers.Authentication {
	public class PurchaseAuthenticationAttribute : ActionFilterAttribute, IAuthenticationFilter {

		#region Implementation

		public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext) {
			var user = filterContext.HttpContext.User;

			if (user == null || !user.Identity.IsAuthenticated) {
				filterContext.Result = new HttpUnauthorizedResult();
				return;
			}

			var purchaseId = filterContext.HttpContext.Request.RequestContext.RouteData.Values["id"] as string
				?? filterContext.HttpContext.Request["id"] as string;

			if (purchaseId == null) {
				return;
			}

			var db = new Models.ApplicationDbContext();
			var purchase = db.Purchases.Find(int.Parse(purchaseId));

			if (purchase == null) {
				return;
			}

			if (purchase.Bottle.OwnerID != user.Identity.GetUserId()) {
				filterContext.Result = new RedirectResult("/AccessDenied");
			}
		}

		public void OnAuthentication(AuthenticationContext filterContext) {

		}
		#endregion
	}
}