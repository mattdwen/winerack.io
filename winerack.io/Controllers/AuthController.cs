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
using RestSharp;
using RestSharp.Authenticators;

namespace winerack.Controllers {

	[Authorize]
    public class AuthController : Controller
    {

        #region Constants

        private const string BASE_URL = "http://localhost:3890";
        //private const string BASE_URL = "http://www.winerack.io";

        #endregion

        #region Declarations

        private ApplicationDbContext context = new ApplicationDbContext();

		#endregion Delcarations

		#region Private Methods

		#endregion Private Methods

		#region Actions

		#region Twitter

		public ActionResult Twitter() {
			var client = new Logic.Social.Twitter(context);
			var url = client.GetAuthorizationUrl();
			return Redirect(url);
		}

		public ActionResult Twitter_Callback(string oauth_verifier) {
			var client = new Logic.Social.Twitter(context);
			var credentials = client.ProcessAccessToken(oauth_verifier, User.Identity.GetUserId());

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
            var redirectUrl = Request.Url.AbsoluteUri;
			var scope = "publish_actions";

            if (Request["code"] == null) {
                return Redirect(string.Format("https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&scope={2}",
                    appId, redirectUrl, scope));
            } else {
                var code = Request["code"];
                var client = new Facebook.FacebookClient();

                dynamic result = client.Get("oauth/access_token", new {
                    client_id = appId,
                    redirect_uri = redirectUrl,
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
		}

		public ActionResult Facebook_Callback(string code) {
			// Exchange the code for an access token
			var client = new Facebook.FacebookClient();
			var appId = ConfigurationManager.AppSettings["facebook:appId"];
			var appSecret = ConfigurationManager.AppSettings["facebook:appSecret"];
			var redirectUri = BASE_URL + "/auth/facebook_callback/";
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
			var client = new Logic.Social.Tumblr(context);
			var url = client.GetAuthorizationUrl();
			return Redirect(url);
		}

		public ActionResult Tumblr_Callback(string oauth_verifier) {
			var client = new Logic.Social.Tumblr(context);
			var credentials = client.ProcessAccessToken(oauth_verifier, User.Identity.GetUserId());

			// How many blogs
			var blogs = client.GetBlogs(User.Identity.GetUserId());
			if (blogs.Count < 1) {
				throw new Exception("No blogs");
			} else if (blogs.Count > 1) {
				throw new Exception("Too many blogs");
			} else {
				var blog = blogs[0];
				client.SetBlog(User.Identity.GetUserId(), blog);
			}

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