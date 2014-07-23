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
using Facebook;

namespace winerack.Controllers {

	[Authorize]
    public class AuthController : Controller {

		#region Declarations

		private ApplicationDbContext context = new ApplicationDbContext();

		#endregion Delcarations

		#region Private Methods

		#region Tumblr

		private DontPanic.TumblrSharp.OAuth.OAuthClient GetTumblrClient() {
			var consumerKey = ConfigurationManager.AppSettings["tumblr:consumerKey"];
			var consumerSecret = ConfigurationManager.AppSettings["tumblr:consumerSecret"];
			var factory = new DontPanic.TumblrSharp.OAuthClientFactory();
			var oauthClient = factory.Create(consumerKey, consumerSecret);
			return oauthClient;
		}

		#endregion Tumblr

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

		#region Facebook

		public ActionResult Facebook() {
			var appId = ConfigurationManager.AppSettings["facebook:appId"];
			var appSecret = ConfigurationManager.AppSettings["facebook:appSecret"];
			var redirectUrl = "http://localhost:3890/auth/facebook_callback/";
			var scope = "publish_actions";

			var url = "https://www.facebook.com/dialog/oauth?" +
				"client_id=" + appId +
				"&redirect_uri=" + redirectUrl;

			return Redirect(url);
		}

		public ActionResult Facebook_Callback(string code) {
			// Exchange the code for an access token
			var client = new Facebook.FacebookClient();
			var appId = ConfigurationManager.AppSettings["facebook:appId"];
			var appSecret = ConfigurationManager.AppSettings["facebook:appSecret"];
			var redirectUri = "http://localhost:3890/auth/facebook_callback/";
			dynamic result = client.Get("oauth/access_token", new {
				client_id = appId,
				redirect_uri = redirectUri,
				client_secret = appSecret,
				code = code
			});

			// Search for an existing credential set
			var userId = User.Identity.GetUserId();
			var credentials = context.Credentials
				.Where(c => c.UserID == userId && c.CredentialType == CredentialTypes.Facebook)
				.FirstOrDefault();

			if (credentials == null) {
				credentials = new Credentials {
					UserID = userId,
					CredentialType = CredentialTypes.Facebook
				};
			}

			credentials.Key = code;
			credentials.Secret = result.access_token;

			if (credentials.ID < 1) {
				context.Credentials.Add(credentials);
			}

			context.SaveChanges();

			return RedirectToAction("Settings", "Account", new { AuthMessage = AuthControllerMessages.FacebookConnected });
		}

		public ActionResult Facebook_Remove() {
			// Search for an existing credential set
			var userId = User.Identity.GetUserId();
			var credentials = context.Credentials
				.Where(c => c.UserID == userId && c.CredentialType == CredentialTypes.Facebook)
				.FirstOrDefault();

			if (credentials != null) {
				context.Credentials.Remove(credentials);
				context.SaveChanges();
			}

			return RedirectToAction("Settings", "Account", new { AuthMessage = AuthControllerMessages.FacebookRemoved });
		}

		#endregion Facebook

		#region Tumblr

		public ActionResult Tumblr() {
			var client = GetTumblrClient();
			var callbackUrl = "http://localhost:3890/Auth/Tumblr_Callback";
			var requestToken = client.GetRequestTokenAsync(callbackUrl).Result;
			Session["TumblrRequestToken"] = requestToken;
			var authorizeUrl = client.GetAuthorizeUrl(requestToken);
			return Redirect(authorizeUrl.ToString());
		}

		public ActionResult Tumblr_Callback(string oauth_token, string oauth_verifier) {
			var requestToken = Session["TumblrRequestToken"] as DontPanic.TumblrSharp.OAuth.Token;
			var client = GetTumblrClient();
			var authToken = client.GetAccessTokenAsync(requestToken, Request.Url.ToString()).Result;

			var userId = User.Identity.GetUserId();
			var credentials = context.Credentials
				.Where(c => c.UserID == userId && c.CredentialType == CredentialTypes.Tumblr)
				.FirstOrDefault();

			if (credentials == null) {
				credentials = new Credentials {
					UserID = userId,
					CredentialType = CredentialTypes.Tumblr
				};
			}

			credentials.Key = authToken.Key;
			credentials.Secret = authToken.Secret;

			if (credentials.ID < 1) {
				context.Credentials.Add(credentials);
			}

			context.SaveChanges();

			return RedirectToAction("Settings", "Account", new { AuthMessage = AuthControllerMessages.TumblrConnected });
		}

		public ActionResult Tumblr_Remove() {
			var userId = User.Identity.GetUserId();
			var credentials = context.Credentials
				.Where(c => c.UserID == userId && c.CredentialType == CredentialTypes.Tumblr)
				.FirstOrDefault();

			if (credentials != null) {
				context.Credentials.Remove(credentials);
				context.SaveChanges();
			}

			return RedirectToAction("Settings", "Account", new { AuthMessage = AuthControllerMessages.TumblrRemoved });
		}

		#endregion Tumblr

		#endregion Actions
	}

	public enum AuthControllerMessages {
		TwitterConnected,
		TwitterRemoved,
		FacebookConnected,
		FacebookRemoved,
		TumblrConnected,
		TumblrRemoved
	}
}