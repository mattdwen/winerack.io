﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Microsoft.AspNet.Identity;


namespace winerack.Helpers.Authentication {
	public class StoredBottleAuthenticationAttribute : ActionFilterAttribute, IAuthenticationFilter {

		#region Declarations
		string _idParameter = "id";
		#endregion

		public string IdParameter {
			get { return _idParameter; }
			set { _idParameter = value; }
		}

		#region Implementation

		public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext) {
			var user = filterContext.HttpContext.User;

			if (user == null || !user.Identity.IsAuthenticated) {
				filterContext.Result = new HttpUnauthorizedResult();
				return;
			}

			var storedBottleId = filterContext.HttpContext.Request.RequestContext.RouteData.Values[_idParameter] as string
				?? filterContext.HttpContext.Request[_idParameter] as string;

			if (storedBottleId == null) {
				return;
			}

			var db = new Models.ApplicationDbContext();
			var storedBottle = db.StoredBottles.Find(int.Parse(storedBottleId));

			if (storedBottle == null) {
				return;
			}

			if (storedBottle.Purchase.Bottle.OwnerID != user.Identity.GetUserId()) {
				filterContext.Result = new RedirectResult("/AccessDenied");
			}
		}

		public void OnAuthentication(AuthenticationContext filterContext) {

		}
		#endregion
	}
}