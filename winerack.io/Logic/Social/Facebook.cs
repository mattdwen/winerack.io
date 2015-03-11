using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using winerack.Models;

namespace winerack.Logic.Social {
	public class Facebook {

		#region Constructor

		public Facebook(ApplicationDbContext context) {
			this.context = context;
		}

		#endregion Constructor

		#region Declarations

		private ApplicationDbContext context;

		#endregion Declarations

		#region Private Methods

		#endregion Private Methods

		private Credentials GetCredentials(string userId) {
			return context.Credentials
				.Where(c => c.UserID == userId && c.CredentialType == CredentialTypes.Facebook)
				.FirstOrDefault();
		}

		private FacebookClient GetClient(string userId) {
			var credentials = GetCredentials(userId);
			return new FacebookClient(credentials.Secret);
		}

		#region Public Methods

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
			client.Post("me/winerackio:taste", new { wine = wineUrl });
		}

		#endregion Public Methods

	}
}