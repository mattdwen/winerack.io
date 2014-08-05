using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using winerack.Models;

namespace winerack.Logic {
	public class Tagging {

		#region Constructor

		public Tagging(ApplicationDbContext db) {
			this.db = db;
		}

		#endregion Constructor

		#region Declarations

		ApplicationDbContext db;

		#endregion Declarations

		#region Public Methods

		/// <summary>
		/// Parse a CSV of user ids and return a list of TaggedUser objects to store with a parent
		/// 
		/// The userIDs are either local userIDs or prefixed with "fb:" for Facebook IDs.
		/// </summary>
		/// <param name="userIDs"></param>
		public IList<TaggedUser> TagUsers(IList<string> userIDs, int parentID, ActivityVerbs verb, string userId) {
			var taggedUsers = new List<TaggedUser>();

			if (userIDs == null) {
				return taggedUsers;
			}

			var facebook = new Social.Facebook(db);

			foreach (var id in userIDs) {
				var taggedUser = new TaggedUser {
					ParentID = parentID,
					ActivityVerb = verb
				};

				if (id.Substring(0, 4) == "fb::") {
					taggedUser.UserType = TaggedUserTypes.Facebook;
					taggedUser.AltUserID = id.Substring(4, id.LastIndexOf("::") - 4);
					taggedUser.Name = id.Substring(id.LastIndexOf("::") + 2);
				} else {
					var user = db.Users.Find(id);
					if (user == null) {
						continue;
					}
					taggedUser.UserType = TaggedUserTypes.Winerack;
					taggedUser.UserID = id;
					taggedUser.Name = user.Name;
				}

				db.TaggedUsers.Add(taggedUser);
				taggedUsers.Add(taggedUser);
			}

			db.SaveChanges();

			return taggedUsers;
		}

		#endregion Public Methods

	}
}