using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using winerack.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net;
using winerack.Logic;
using System.Configuration;
using System;
using RestSharp;
using RestSharp.Authenticators;

namespace winerack.Controllers {

	[Authorize]
	public class AccountController : Controller {

		#region Constructor

		public AccountController() {
		}

		public AccountController(ApplicationUserManager userManager) {
			UserManager = userManager;
		}

		#endregion Constructor

		#region Declarations

		private ApplicationDbContext context = new ApplicationDbContext();
		private ApplicationUserManager _userManager;

		#endregion Declarations

		#region Properties

		public ApplicationUserManager UserManager {
			get {
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set {
				_userManager = value;
			}
		}

		#endregion Properties

		#region Private Methods

		#region Settings

		private SettingsViewModel GetSettingsViewModel(User user, SettingsViewModel model = null) {
			if (model == null) {
				model = new SettingsViewModel {
					FirstName = user.FirstName,
					LastName = user.LastName,
					Location = user.Location,
					Country = user.Country,
                    Username = user.UserName
				};	
			}

			model.ImageID = user.ImageID;
			model.SocialTwitter = (user.Credentials.Where(c => c.CredentialType == CredentialTypes.Twitter).FirstOrDefault() != null);
			model.SocialFacebook = (user.Credentials.Where(c => c.CredentialType == CredentialTypes.Facebook).FirstOrDefault() != null);
			model.SocialTumblr = (user.Credentials.Where(c => c.CredentialType == CredentialTypes.Tumblr).FirstOrDefault() != null);

			return model;
		}

		#endregion Settings

		#endregion Private Methods

		#region Actions

		#region Sign In

		// GET: /Account/SignIn
		[AllowAnonymous]
		public ActionResult SignIn(string returnUrl) {
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

    // POST: /Account/SignIn
    [HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SignIn(LoginViewModel model, string returnUrl) {
			if (ModelState.IsValid) {
				var user = await UserManager.FindAsync(model.Username, model.Password);
				if (user != null) {
					await SignInAsync(user, model.RememberMe);
					return RedirectToLocal(returnUrl);
				} else {
					ModelState.AddModelError("", "Invalid username or password.");
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		#endregion Login

		#region Register

		// GET: /Account/Register
		[AllowAnonymous]
		public ActionResult Register() {
			return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			ViewBag.Country = new SelectList(Country.GetCountries(), "ID", "Name");
			return View();
		}

		// POST: /Account/Register
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model) {
			return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			if (ModelState.IsValid) {
				var user = new User() {
					UserName = model.Username,
					Email = model.Email,
					FirstName = model.FirstName,
					LastName =model.LastName,
					Location = model.Location,
					Country = model.Country,
					CreatedOn = DateTime.Now
				};

				IdentityResult result = await UserManager.CreateAsync(user, model.Password);
				if (result.Succeeded) {
					await SignInAsync(user, isPersistent: false);
					// For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
					// Send an email with this link

					//string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
					//var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

					//await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

					return RedirectToAction("Index", "Home");
				} else {
					AddErrors(result);
				}
			}

			ViewBag.Country = new SelectList(Country.GetCountries(), "ID", "Name");

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		#endregion Register

		#region Confirm Email

		// GET: /Account/ConfirmEmail
		[AllowAnonymous]
		public async Task<ActionResult> ConfirmEmail(string userId, string code) {
			if (userId == null || code == null) {
				return View("Error");
			}

			IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);
			if (result.Succeeded) {
				return View("ConfirmEmail");
			} else {
				AddErrors(result);
				return View();
			}
		}

		#endregion Confirm Email

		#region Forgot Password

		// GET: /Account/ForgotPassword
		[AllowAnonymous]
		public ActionResult ForgotPassword() {
			return View();
		}

		// POST: /Account/ForgotPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model) {
			if (ModelState.IsValid) {
				var user = await UserManager.FindByEmailAsync(model.Email);
				if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id))) {
					ModelState.AddModelError("", "The user either does not exist or is not confirmed.");
					return View();
				}

				// For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
				// Send an email with this link
				string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
				var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
				await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
				return RedirectToAction("ForgotPasswordConfirmation", "Account");
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		// GET: /Account/ForgotPasswordConfirmation
		[AllowAnonymous]
		public ActionResult ForgotPasswordConfirmation() {
			return View();
		}

		#endregion Forgot Password

		#region Forgot Username

		// GET: /Account/ForgotUsername
		[AllowAnonymous]
		public ActionResult ForgotUsername() {
			return View();
		}

		// POST: /Account/ForgotPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ForgotUsername(ForgotUsernameViewModel model) {
			if (ModelState.IsValid) {
				var user = await UserManager.FindByEmailAsync(model.Email);
				if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id))) {
					ModelState.AddModelError("", "The user either does not exist or is not confirmed.");
					return View();
				}

				string username = user.UserName;
				await UserManager.SendEmailAsync(user.Id, "Username", "Your username is <strong>" + username + "</strong>");
				return RedirectToAction("ForgotUsernameConfirmation", "Account");
			}

			return View(model);
		}

		// GET: /Account/ForgotUsernameConfirmation
		[AllowAnonymous]
		public ActionResult ForgotUsernameConfirmation() {
			return View();
		}

		#endregion Forgot Username

		#region Reset Password

		//
		// GET: /Account/ResetPassword
		[AllowAnonymous]
		public ActionResult ResetPassword(string code) {
			if (code == null) {
				return View("Error");
			}
			return View();
		}

		//
		// POST: /Account/ResetPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model) {
			if (ModelState.IsValid) {
				var user = await UserManager.FindByNameAsync(model.Username);
				if (user == null) {
					ModelState.AddModelError("", "No user found.");
					return View();
				}
				IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
				if (result.Succeeded) {
					return RedirectToAction("ResetPasswordConfirmation", "Account");
				} else {
					AddErrors(result);
					return View();
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//
		// GET: /Account/ResetPasswordConfirmation
		[AllowAnonymous]
		public ActionResult ResetPasswordConfirmation() {
			return View();
		}

		#endregion Reset Password

		#region Disassociate

		//
		// POST: /Account/Disassociate
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Disassociate(string loginProvider, string providerKey) {
			ManageMessageId? message = null;
			IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
			if (result.Succeeded) {
				var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
				await SignInAsync(user, isPersistent: false);
				message = ManageMessageId.RemoveLoginSuccess;
			} else {
				message = ManageMessageId.Error;
			}
			return RedirectToAction("Manage", new { Message = message });
		}

		#endregion Disassociate

		#region Manage

		//
		// GET: /Account/Manage
		public ActionResult Manage(ManageMessageId? message) {
			ViewBag.StatusMessage =
				message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
				: message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
				: message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
				: message == ManageMessageId.Error ? "An error has occurred."
				: "";
			ViewBag.HasLocalPassword = HasPassword();
			ViewBag.ReturnUrl = Url.Action("Manage");
			return View();
		}

		//
		// POST: /Account/Manage
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Manage(ManageUserViewModel model) {
			bool hasPassword = HasPassword();
			ViewBag.HasLocalPassword = hasPassword;
			ViewBag.ReturnUrl = Url.Action("Manage");
			if (hasPassword) {
				if (ModelState.IsValid) {
					IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
					if (result.Succeeded) {
						var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
						await SignInAsync(user, isPersistent: false);
						return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
					} else {
						AddErrors(result);
					}
				}
			} else {
				// User does not have a password so remove any validation errors caused by a missing OldPassword field
				ModelState state = ModelState["OldPassword"];
				if (state != null) {
					state.Errors.Clear();
				}

				if (ModelState.IsValid) {
					IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
					if (result.Succeeded) {
						return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
					} else {
						AddErrors(result);
					}
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		#endregion Manage

		#region External Login

		//
		// POST: /Account/ExternalLogin
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLogin(string provider, string returnUrl) {
			// Request a redirect to the external login provider
			return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
		}

		//
		// GET: /Account/ExternalLoginCallback
		[AllowAnonymous]
		public async Task<ActionResult> ExternalLoginCallback(string returnUrl) {
			var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
			if (loginInfo == null) {
				return RedirectToAction("Login");
			}

			// Sign in the user with this external login provider if the user already has a login
			var user = await UserManager.FindAsync(loginInfo.Login);
			if (user != null) {
				await SignInAsync(user, isPersistent: false);
				return RedirectToLocal(returnUrl);
			} else {
				// If the user does not have an account, then prompt the user to create an account
				ViewBag.ReturnUrl = returnUrl;
				ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
				return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
			}
		}

		//
		// POST: /Account/LinkLogin
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LinkLogin(string provider) {
			// Request a redirect to the external login provider to link a login for the current user
			return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
		}

		//
		// GET: /Account/LinkLoginCallback
		public async Task<ActionResult> LinkLoginCallback() {
			var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
			if (loginInfo == null) {
				return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
			}
			IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
			if (result.Succeeded) {
				return RedirectToAction("Manage");
			}
			return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
		}

		//
		// POST: /Account/ExternalLoginConfirmation
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl) {
			if (User.Identity.IsAuthenticated) {
				return RedirectToAction("Manage");
			}

			if (ModelState.IsValid) {
				// Get the information about the user from the external login provider
				var info = await AuthenticationManager.GetExternalLoginInfoAsync();
				if (info == null) {
					return View("ExternalLoginFailure");
				}
				var user = new User() { UserName = model.Email, Email = model.Email };
				IdentityResult result = await UserManager.CreateAsync(user);
				if (result.Succeeded) {
					result = await UserManager.AddLoginAsync(user.Id, info.Login);
					if (result.Succeeded) {
						await SignInAsync(user, isPersistent: false);

						// For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
						// Send an email with this link
						// string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
						// var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
						// SendEmail(user.Email, callbackUrl, "Confirm your account", "Please confirm your account by clicking this link");

						return RedirectToLocal(returnUrl);
					}
				}
				AddErrors(result);
			}

			ViewBag.ReturnUrl = returnUrl;
			return View(model);
		}

		//
		// POST: /Account/LogOff
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff() {
			AuthenticationManager.SignOut();
			return RedirectToAction("Index", "Home");
		}

		//
		// GET: /Account/ExternalLoginFailure
		[AllowAnonymous]
		public ActionResult ExternalLoginFailure() {
			return View();
		}

		#endregion External Login

		#region Remove Account List

		[ChildActionOnly]
		public ActionResult RemoveAccountList() {
			var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
			ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
			return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
		}

		#endregion Remove Account List

		#region AcceptInvite

		// GET: /Account/AcceptInvite
		[AllowAnonymous]
		public async Task<ActionResult> AcceptInvite(string userId, string code) {
			if (userId == null || code == null) {
				return View("Error");
			}

			IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);
			if (result.Succeeded) {
				// Redirect to password reset
				string resetCode = await UserManager.GeneratePasswordResetTokenAsync(userId);
				var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = userId, code = resetCode }, protocol: Request.Url.Scheme);
				return Redirect(callbackUrl);
			} else {
				AddErrors(result);
				return View();
			}
		}
		#endregion

		#region Settings

		// GET: /Account/Settings
		public ActionResult Settings(SettingsMessageId? message, AuthControllerMessages? authMessage) {
			var user = UserManager.FindById(User.Identity.GetUserId());
			var model = GetSettingsViewModel(user);			

			ViewBag.SuccessMessage =
				message == SettingsMessageId.UpdateProfileSuccess ? "Your profile has been updated"
				: message == SettingsMessageId.UpdateProfileSuccess ? "Your profile picture has been updated"
				: authMessage == AuthControllerMessages.TwitterConnected ? "Connected your account with Twitter"
				: authMessage == AuthControllerMessages.FacebookConnected ? "Connected your account with Facebook"
				: authMessage == AuthControllerMessages.TumblrConnected ? "Connected your account with Tumblr"
				: null;

			ViewBag.DangerMessage =
				authMessage == AuthControllerMessages.TwitterRemoved ? "Disconnected your account from Twitter"
				: authMessage == AuthControllerMessages.FacebookRemoved ? "Disconnected your account from Facebook"
				: authMessage == AuthControllerMessages.TumblrRemoved ? "Disconnected your account from Tumblr"
				: null;

			ViewBag.Country = new SelectList(Country.GetCountries(), "ID", "Name");

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Settings(SettingsViewModel model) {
			var user = UserManager.FindById(User.Identity.GetUserId());
			model = GetSettingsViewModel(user, model);

			if (ModelState.IsValid) {
				user.FirstName = model.FirstName;
				user.LastName = model.LastName;
                user.Location = model.Location;
                user.Country = model.Country;

				var result = UserManager.Update(user);
				if (result.Succeeded) {
					return RedirectToAction("Settings", new { Message = SettingsMessageId.UpdateProfileSuccess });
				}

				AddErrors(result);
			}

			ViewBag.Country = new SelectList(Country.GetCountries(), "ID", "Name");

			return View(model);
		}

		#endregion Settings

		#region Profile Picture

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ProfilePicture(HttpPostedFileBase photo) {
			// Save the photo
			if (photo != null && photo.ContentLength > 0) {
				var user = UserManager.FindById(User.Identity.GetUserId());
				var blobHandler = new Logic.BlobHandler("profiles");

				user.ImageID = blobHandler.UploadImage(photo, Images.GetSizes(ImageSizeSets.Profile));

				var result = UserManager.Update(user);
				if (result.Succeeded) {
					return RedirectToAction("Settings", new { Message = SettingsMessageId.UpdatePictureSuccess });
				}
			}

			return RedirectToAction("Settings");
		}

		#endregion Profile Picture

		#endregion Actions

		#region Public Methods

		protected override void Dispose(bool disposing) {
			if (disposing && UserManager != null) {
				UserManager.Dispose();
				UserManager = null;
			}
			base.Dispose(disposing);
		}

		#endregion Public Methods

		#region Helpers

		// Used for XSRF protection when adding external logins
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager {
			get {
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private async Task SignInAsync(User user, bool isPersistent) {
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
			AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
		}

		private void AddErrors(IdentityResult result) {
			foreach (var error in result.Errors) {
				ModelState.AddModelError("", error);
			}
		}

		private bool HasPassword() {
			var user = UserManager.FindById(User.Identity.GetUserId());
			if (user != null) {
				return user.PasswordHash != null;
			}
			return false;
		}

		private void SendEmail(string email, string callbackUrl, string subject, string message) {
			// For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
		}

		public enum ManageMessageId {
			ChangePasswordSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
			Error
		}

		public enum SettingsMessageId {
			UpdateProfileSuccess,
			UpdatePictureSuccess
		};

		private ActionResult RedirectToLocal(string returnUrl) {
			if (Url.IsLocalUrl(returnUrl)) {
				return Redirect(returnUrl);
			} else {
				return RedirectToAction("Index", "Home");
			}
		}

		private class ChallengeResult : HttpUnauthorizedResult {

			public ChallengeResult(string provider, string redirectUri)
				: this(provider, redirectUri, null) {
			}

			public ChallengeResult(string provider, string redirectUri, string userId) {
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }

			public string RedirectUri { get; set; }

			public string UserId { get; set; }

			public override void ExecuteResult(ControllerContext context) {
				var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
				if (UserId != null) {
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}

		#endregion Helpers
	}
}