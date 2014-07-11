using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace winerack.Helpers {
	public static class ExtensionMethods {
		public static MvcHtmlString TimeAgo(DateTime when) {
			var span = DateTime.Now - when;

			var val = "";

			if (span.TotalMinutes < 1) {
				val = "Less than a minute ago";
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
			} else {
				val = when.ToString("D");
			}

			return MvcHtmlString.Create(val);
		}
	}
}