using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using winerack.Models;

namespace winerack.Logic {
	public static class ActivityStream {

		#region Public Methods

		public static void Publish(ApplicationDbContext db, string userId, ActivityVerbs verb, int noun) {
			var activity = new ActivityEvent {
				UserID = userId,
				OccuredOn = DateTime.Now,
				Verb = verb,
				Noun = noun
			};

			db.ActivityEvents.Add(activity);
		}

		#endregion
	}
}