using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using winerack.Models;
using winerack.Models.Social;

namespace winerack.Logic.Social {
	public class Facebook {

		#region Constructor

		public Facebook(ApplicationDbContext db) {
			this.db = db;
		}

		#endregion Constructor

		#region Declarations

		private ApplicationDbContext db;

		#endregion Declarations

		#region Private Methods

		private Credentials GetCredentials(string userId) {
			return db.Credentials
				.Where(c => c.UserID == userId && c.CredentialType == CredentialTypes.Facebook)
				.FirstOrDefault();
		}

		private FacebookClient GetClient(string userId) {
			var credentials = GetCredentials(userId);
			return new FacebookClient(credentials.Secret);
		}

		#endregion Private Methods

		#region Public Methods

		public IList<FacebookFriend> GetFriends(string userId) {
			var client = GetClient(userId);
			dynamic result = client.Get("me/taggable_friends");
			var friends = new List<FacebookFriend>();
			foreach (var person in result.data) {
				friends.Add(new FacebookFriend {
					id = person.id,
					name = person.name
				});
			}
			return friends;
		}

		public string GetName(string userId, string facebookUserId) {
			var client = GetClient(userId);
			dynamic result = client.Get(facebookUserId);
			return result.first_name + " " + result.last_name;
		}

		public void OpenWine(string userId, int openingId) {
			var client = GetClient(userId);
			var wineUrl = "http://winerack.io/openings/" + openingId.ToString();
			client.Post("me/winerackio:open", new { wine = wineUrl });
		}

		public void PurchaseWine(string userId, int purchaseId) {
			var client = GetClient(userId);
			var wineUrl = "http://winerack.io/purchases/" + purchaseId.ToString();
			client.Post("me/winerackio:purchase", new { wine = wineUrl });
		}

		public void TasteWine(string userId, int tastingId) {
			var client = GetClient(userId);
			var wineUrl = "http://winerack.io/tastings/" + tastingId.ToString();
			var facebookFriends = db.TaggedUsers
				.Where(
					t => t.ParentID == tastingId
					&& t.ActivityVerb == ActivityVerbs.Tasted
					&& t.UserType == TaggedUserTypes.Facebook
				);

			var tags = "";
			if (facebookFriends.Count() > 0) {
				tags = string.Join(",", facebookFriends.Select(t => t.AltUserID));
			}

			client.Post("me/winerackio:taste", new { wine = wineUrl, tags = tags });
		}

		#endregion Public Methods

	}
}