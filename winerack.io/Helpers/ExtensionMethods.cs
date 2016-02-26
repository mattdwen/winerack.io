using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using winerack.Models;

namespace winerack.Helpers {
	public static class ExtensionMethods {

		public static MvcHtmlString CellarDescription(int? min, int? max) {
			string desc = "";

			if (min.HasValue && max.HasValue) {
				desc = min.Value.ToString() + " to " + max.Value.ToString() + " years";
			} else if (max.HasValue) {
				desc = "Up to " + max.Value.ToString() + " years";
			} else if (min.HasValue) {
				desc = "At least " + min.Value.ToString() + " years";
			}

			return MvcHtmlString.Create(desc);
		}

		public static MvcHtmlString TimeAgo(DateTime when) {
			var span = DateTime.Now - when;

			var val = "";

      if (span.TotalMinutes < 1) {
        val = "Just now";
      } else if (span.TotalHours < 1) {
        val = Math.Floor(span.TotalMinutes).ToString() + " minutes ago";
      } else if (span.TotalHours < 2) {
        val = "An hour ago";
      } else if (span.TotalDays < 1) {
        val = Math.Floor(span.TotalHours).ToString() + " hours ago";
      } else if (span.TotalDays < 2) {
        val = "A day ago";
      } else if (span.TotalDays < 7) {
        val = Math.Floor(span.TotalDays).ToString() + " days ago";
      } else if (when.Year == DateTime.Now.Year) {
        val = when.ToString("MMM-d");
      } else {
				val = when.ToString("D");
			}

			return MvcHtmlString.Create(val);
		}

		public static MvcHtmlString BottleQuantity(int quantity) {
			var text = "";

			if (quantity == 12) {
				text = "a case";
			} else if (quantity > 12 && quantity % 12 == 0) {
				text = (quantity / 12).ToString() + " cases";
			} else if (quantity == 6) {
				text = "a half case";
			} else if (quantity > 6 && quantity % 6 == 0) {
				text = (quantity / 6).ToString() + " half cases";
			} else if (quantity == 1) {
				text = "a bottle";
			} else {
				text = quantity.ToString() + " bottles";
			}

			return MvcHtmlString.Create(text);
		}

		public static MvcHtmlString ProfileImageUrl(string size = "sq_sm", string userId = null) {
			if (string.IsNullOrWhiteSpace(userId)) {
				userId = HttpContext.Current.User.Identity.GetUserId();
			}
			var dbContext = new ApplicationDbContext();
			var user = dbContext.Users.Find(userId);
			var url = "/Content/images/profile-picture.png";

		  if (user?.ImageID == null)
		  {
		    return MvcHtmlString.Create(url);
		  }

		  var endPointUrl = ConfigurationManager.AppSettings["Azure:Storage:Endpoint"];
		  url = $"{endPointUrl}/profiles/{user.ImageID.Value}_{size}.jpg";
		  return MvcHtmlString.Create(url);
		}
	}
}