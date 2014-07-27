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
	public class Tumblr : ISocial {

		#region Constructor

		public Tumblr(ApplicationDbContext context) {
			this.context = context;
			this.consumer_key = ConfigurationManager.AppSettings["tumblr:consumerKey"];
			this.consumer_secret = ConfigurationManager.AppSettings["tumblr:consumerSecret"];
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
			var baseUrl = "http://api.tumblr.com/v2";
			var client = new RestClient(baseUrl);
			client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumer_key, consumer_secret, credentials.Key, credentials.Secret);
			return client;
		}

		private Credentials GetCredentials(string userId) {
			return context.Credentials
				.Where(c => c.UserID == userId && c.CredentialType == CredentialTypes.Tumblr)
				.FirstOrDefault();
		}

		private RestClient GetOAuthClient() {
			var baseUrl = "http://www.tumblr.com";
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

			HttpContext.Current.Session["tumblr_oauth_token"] = oauth_token;
			HttpContext.Current.Session["tumblr_oauth_token_secret"] = oauth_token_secret;

			request = new RestRequest("oauth/authorize");
			request.AddParameter("oauth_token", oauth_token);
			var url = client.BuildUri(request).ToString();
			return url;
		}

		public IList<string> GetBlogs(string userId) {
			var client = GetApiClient(userId);
			var request = new RestRequest("user/info", Method.GET);
			request.RequestFormat = DataFormat.Json;
			var response = client.Execute<Models.Social.Tumblr.UserResponse>(request);

			if (response.StatusCode != System.Net.HttpStatusCode.OK) {
				throw new Exception(response.ErrorMessage);
			}

			var blogs = new List<string>();
			foreach (var blog in response.Data.response.user.blogs) {
				blogs.Add(blog.name);
			}

			return blogs;
		}

		public Credentials ProcessAccessToken(string verifier, string userId) {
			if (string.IsNullOrWhiteSpace(verifier)) {
				throw new NullReferenceException("verifier is required");
			}

			if (string.IsNullOrWhiteSpace(userId)) {
				throw new NullReferenceException("userId is required");
			}

			var oauth_token = HttpContext.Current.Session["tumblr_oauth_token"] as string;
			var oauth_token_secret = HttpContext.Current.Session["tumblr_oauth_token_secret"] as string;
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
				.Where(c => c.UserID == userId && c.CredentialType == CredentialTypes.Tumblr)
				.FirstOrDefault();

			if (credentials == null) {
				credentials = new Credentials {
					UserID = userId,
					CredentialType = CredentialTypes.Tumblr
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

		public void SetBlog(string userId, string blog) {
			var credentials = GetCredentials(userId);
			credentials.Data1 = blog;
			context.SaveChanges();
		}

		public void PostPhoto(string userId, string imageUrl, string caption) {
			var credentials = GetCredentials(userId);
			var client = GetApiClient(userId);
			var request = new RestRequest("blog/" + credentials.Data1 + ".tumblr.com/post", Method.POST);
			request.AddParameter("type", "photo");
			request.AddParameter("source", imageUrl);
			request.AddParameter("caption", caption);
			request.AddParameter("tags", "wine");
			client.Execute(request);
		}

		#endregion Public Methods

	}
}