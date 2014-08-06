using Facebook;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using winerack.Models;
using winerack.Models.Social;

namespace winerack.Logic.Social {
	public class Facebook {

		#region Constructor

		public Facebook(ApplicationDbContext db) {
			this.db = db;
			this.appNamespace = ConfigurationManager.AppSettings["facebook:appNamespace"];
		}

		#endregion Constructor

		#region Declarations

		private ApplicationDbContext db;
		private string appNamespace;

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
			var tasting = db.Tastings.Find(tastingId);
			var client = GetClient(userId);
			var baseUrl = ConfigurationManager.AppSettings["baseUrl"];
			var tastingUrl = baseUrl + "/tastings/" + tastingId.ToString();
			var facebookFriends = db.TaggedUsers
				.Where(
					t => t.ParentID == tastingId
					&& t.ActivityVerb == ActivityVerbs.Tasted
					&& t.UserType == TaggedUserTypes.Facebook
				);

			var parameters = new Dictionary<string, object>();
			parameters.Add("wine", tastingUrl);
			parameters.Add("fb:explicitly_shared", true);

			if (facebookFriends.Count() > 0) {
				var tags = string.Join(",", facebookFriends.Select(t => t.AltUserID));
				parameters.Add("tags", tags);
			}

			if (!string.IsNullOrWhiteSpace(tasting.Notes)) {
				parameters.Add("message", tasting.Notes);
			}

			if (tasting.ImageID.HasValue) {
				var blobHandler = new BlobHandler(BlobImageDirectories.tastings);
				var imageUrl = blobHandler.GetImageUrl(tasting.ImageID.Value, "lg");
				parameters.Add("image[0][url]", imageUrl);
				parameters.Add("image[0][user_generated]", true);
			}

			client.Post("me/" + appNamespace + ":taste", parameters);
		}

		#endregion Public Methods

	}
}