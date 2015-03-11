using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using winerack.Models;

namespace winerack.Logic.Social {
	public class Twitter : ISocial {

		#region Constructor

		public Twitter(ApplicationDbContext context) {
			this.context = context;
			this.consumer_key = ConfigurationManager.AppSettings["twitter:consumerKey"];
			this.consumer_secret = ConfigurationManager.AppSettings["twitter:consumerSecret"];
		}

		#endregion Constructor

		#region Declarations

		private ApplicationDbContext context;
		private string consumer_key;
		private string consumer_secret;

		#endregion Declarations

		#region Private Methods

		private RestClient GetApiClient(string userId) {
			var credentials = GetCredentials(userId);
			var baseUrl = "https://api.twitter.com/1.1";
			var client = new RestClient(baseUrl);
			client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumer_key, consumer_secret, credentials.Key, credentials.Secret);
			return client;
		}

		private Credentials GetCredentials(string userId) {
			return context.Credentials
				.Where(c => c.UserID == userId && c.CredentialType == CredentialTypes.Twitter)
				.First();
		}

		private RestClient GetOAuthClient() {
			var baseUrl = "https://api.twitter.com";
			var client = new RestClient(baseUrl);
			return client;
		}

		#endregion Private Methods

		#region Public Methods

		public string GetAuthorizationUrl() {
			var client = this.GetOAuthClient();
			client.Authenticator = OAuth1Authenticator.ForRequestToken(consumer_key, consumer_secret);
			var request = new RestRequest("oauth/request_token", Method.POST);
			var response = client.Execute(request);

			var qs = HttpUtility.ParseQueryString(response.Content);
			var oauth_token = qs["oauth_token"];
			var oauth_token_secret = qs["oauth_token_secret"];

			HttpContext.Current.Session["twitter_oauth_token"] = oauth_token;
			HttpContext.Current.Session["twitter_oauth_token_secret"] = oauth_token_secret;

			request = new RestRequest("oauth/authorize");
			request.AddParameter("oauth_token", oauth_token);
			var url = client.BuildUri(request).ToString();
			return url;
		}

		public Credentials ProcessAccessToken(string verifier, string userId) {
			if (string.IsNullOrWhiteSpace(verifier)) {
				throw new NullReferenceException("verifier is required");
			}

			if (string.IsNullOrWhiteSpace(userId)) {
				throw new NullReferenceException("userId is required");
			}

			var oauth_token = HttpContext.Current.Session["twitter_oauth_token"] as string;
			var oauth_token_secret = HttpContext.Current.Session["twitter_oauth_token_secret"] as string;
			var client = GetOAuthClient();

			var request = new RestRequest("oauth/access_token", Method.POST);
			client.Authenticator = OAuth1Authenticator.ForAccessToken(
				consumer_key, consumer_secret, oauth_token, oauth_token_secret, verifier
			);
			var response = client.Execute(request);

			if (response.StatusCode != System.Net.HttpStatusCode.OK) {
				throw new Exception("Invalid response: " + response.ErrorMessage);
			}

			var qs = HttpUtility.ParseQueryString(response.Content);
			oauth_token = qs["oauth_token"];
			oauth_token_secret = qs["oauth_token_secret"];

			var credentials = context.Credentials
				.Where(c => c.UserID == userId && c.CredentialType == CredentialTypes.Twitter)
				.FirstOrDefault();

			if (credentials == null) {
				credentials = new Credentials {
					UserID = userId,
					CredentialType = CredentialTypes.Twitter
				};
			}

			credentials.Key = oauth_token;
			credentials.Secret = oauth_token_secret;

			if (credentials.ID < 1) {
				context.Credentials.Add(credentials);
			}

			context.SaveChanges();

			return credentials;
		}

		public void Tweet(string userId, string message, string url = null) {
			var client = GetApiClient(userId);
			var request = new RestRequest("statuses/update.json", Method.POST);
			if (!string.IsNullOrWhiteSpace(url)) {
				message += " " + url;
			}
			request.AddParameter("status", message);
			client.Execute(request);
		}

		#endregion Public Methods

	}
}