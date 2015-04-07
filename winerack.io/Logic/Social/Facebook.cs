using Facebook;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using winerack.Models;

namespace winerack.Logic.Social {
	public class Facebook {

		#region Constructor

		public Facebook(ApplicationDbContext context) {
			this.context = context;

            this.appId = ConfigurationManager.AppSettings["facebook:appId"];
            this.appSecret = ConfigurationManager.AppSettings["facebook:appSecret"];
		}

		#endregion Constructor

		#region Declarations

		private ApplicationDbContext context;

        private string appId;
        private string appSecret;

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
			var client = new FacebookClient(credentials.Secret);
            client.AppId = appId;
            client.AppSecret = appSecret;

            return client;
		}

		#region Public Methods

		public void OpenWine(string userId, int openingId) {
			var client = GetClient(userId);
			var wineUrl = "http://www.winerack.io/openings/" + openingId.ToString();
			client.Post("me/winerackio:open", new { wine = wineUrl });
		}

		public void PurchaseWine(string userId, int purchaseId) {
			var client = GetClient(userId);
			var wineUrl = "http://www.winerack.io/purchases/" + purchaseId.ToString();
			var result = client.Post("me/winerackio:purchase", new { wine = wineUrl });
		}

		public void TasteWine(string userId, int tastingId) {
			var client = GetClient(userId);
			var wineUrl = "http://www.winerack.io/tastings/" + tastingId.ToString();
			client.Post("me/winerackio:taste", new { wine = wineUrl });
		}

		#endregion Public Methods

	}
}