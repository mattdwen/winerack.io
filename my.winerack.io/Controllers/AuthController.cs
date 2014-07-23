using Spring.Social.OAuth1;
using Spring.Social.Twitter.Connect;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using winerack.Models;
using Microsoft.AspNet.Identity;

namespace winerack.Controllers {

	[Authorize]
    public class AuthController : Controller {

		#region Declarations

		private ApplicationDbContext context = new ApplicationDbContext();

		#endregion Delcarations

		#region Private Methods

		#region Twitter

		private TwitterServiceProvider GetTwitterServiceProvider() {
			var consumerKey = ConfigurationManager.AppSettings["twitter:ConsumerKey"];
			var consumerSecret = ConfigurationManager.AppSettings["twitter:ConsumerSecret"];
			var serviceProvider = new TwitterServiceProvider(consumerKey, consumerSecret);
			return serviceProvider;
		}

		private OAuthToken GetTwitterConsumerToken() {
			var provider = GetTwitterServiceProvider();
			var callbackUrl = "http://localhost:3890/auth/twitter_callback";
			var requestToken = provider.OAuthOperations.FetchRequestTokenAsync(callbackUrl, null).Result;
			return requestToken;
		}

		#endregion Twitter

		#endregion Private Methods

		#region Actions

		#region Twitter

		public ActionResult Twitter() {
			var provider = GetTwitterServiceProvider();
			var requestToken = GetTwitterConsumerToken();
			Session["TwitterRequestToken"] = requestToken;
			var url = provider.OAuthOperations.BuildAuthorizeUrl(requestToken.Value, null);

			return Redirect(url);
		}

		public ActionResult Twitter_Callback(string oauth_verifier) {
			// Process the response
			var provider = GetTwitterServiceProvider();
			var requestToken = Session["TwitterRequestToken"] as OAuthToken;
			var authorizedRequestToken = new AuthorizedRequestToken(requestToken, oauth_verifier);
			var accessToken = provider.OAuthOperations.ExchangeForAccessTokenAsync(authorizedRequestToken, null).Result;

			// Search for an existing credential set
			var userId = User.Identity.GetUserId();
			var credentials = context.Credentials
				.Where(c => c.UserID == userId && c.CredentialType == CredentialTypes.Twitter)
				.FirstOrDefault();

			if (credentials == null) {
				credentials = new Credentials {
					UserID = userId,
					CredentialType = CredentialTypes.Twitter
				};
			}

			credentials.Key = accessToken.Value;
			credentials.Secret = accessToken.Secret;

			if (credentials.ID < 1) {
				context.Credentials.Add(credentials);
			}

			context.SaveChanges();

			Session["TwitterRequestToken"] = null;

			return RedirectToAction("Settings", "Account", new { AuthMessage = AuthControllerMessages.TwitterConnected });
		}

		public ActionResult Twitter_Remove() {
			var userId = User.Identity.GetUserId();
			var credentials = context.Credentials
				.Where(c => c.UserID == userId && c.CredentialType == CredentialTypes.Twitter)
				.FirstOrDefault();

			if (credentials != null) {
				context.Credentials.Remove(credentials);
				context.SaveChanges();
			}

			return RedirectToAction("Settings", "Account", new { AuthMessage = AuthControllerMessages.TwitterRemoved });
		}

		#endregion Twitter

		#endregion Actions
	}

	public enum AuthControllerMessages {
		TwitterConnected,
		TwitterRemoved
	}
}