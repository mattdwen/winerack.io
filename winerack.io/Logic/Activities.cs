using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using winerack.Models;

namespace winerack.Logic {
	public class Activities {

		#region Constructor

		public Activities(ApplicationDbContext db) {
			this.db = db;
		}

		#endregion Constructor

		#region Declarations

		ApplicationDbContext db;

		#endregion Declarations

		#region Public Methods

		public Activity Publish(string actorID, ActivityVerbs verb, int objectID, int? wineID) {
			// Create the activity
			var activity = new Activity {
				ActorID = actorID,
				OccuredOn = DateTime.Now,
				Verb = verb,
				ObjectID = objectID,
				WineID = wineID
			};

			// Notify the actor
			activity.Notifications.Add(new ActivityNotification { UserID = actorID });

			// Notify subscribers
			var actor = db.Users.Find(actorID);
			foreach (var follower in actor.Followers) {
				activity.Notifications.Add(new ActivityNotification { UserID = follower.FollowerID });
			}

			db.Activities.Add(activity);

			return activity;
		}

		public void SaveChanges() {
			db.SaveChanges();
		}

		#endregion
	}
}