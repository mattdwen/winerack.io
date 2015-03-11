using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using winerack.Models;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Mime;
using System;
using System.Configuration;

namespace winerack {
	// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

	public class ApplicationUserManager : UserManager<User> {

		#region Constructor
		public ApplicationUserManager(IUserStore<User> store)
			: base(store) {
		}
		#endregion

		#region Private Methods
		private static ApplicationUserManager ConfigureManager(ApplicationUserManager manager, IDataProtectionProvider dataProtectionProvider = null) {
			// Configure validation logic for usernames
			manager.UserValidator = new UserValidator<User>(manager) {
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};
			// Configure validation logic for passwords
			manager.PasswordValidator = new PasswordValidator {
				RequiredLength = 6,
				RequireNonLetterOrDigit = true,
				RequireDigit = true,
				RequireLowercase = true,
				RequireUppercase = true,
			};
			// Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
			// You can write your own provider and plug in here.
			manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<User> {
				MessageFormat = "Your security code is: {0}"
			});
			manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<User> {
				Subject = "Security Code",
				BodyFormat = "Your security code is: {0}"
			});
			manager.EmailService = new EmailService();
			manager.SmsService = new SmsService();
			if (dataProtectionProvider != null) {
				manager.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
			}
			return manager;
		}
		#endregion

		#region Public Methods
		public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) {
			var manager = new ApplicationUserManager(new UserStore<User>(context.Get<ApplicationDbContext>()));
			var dataProtectionProvider = options.DataProtectionProvider;

			return ConfigureManager(manager, dataProtectionProvider);
		}

		public static ApplicationUserManager Create(ApplicationDbContext context) {
			var manager = new ApplicationUserManager(new UserStore<User>(context));

			return ConfigureManager(manager);
		}
		#endregion
	}

	public class ApplicationRoleManager : RoleManager<IdentityRole> {
		#region Constructor
		public ApplicationRoleManager(RoleStore<IdentityRole> store)
			: base(store) {

		}
		#endregion

		#region Public Methods
		public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context) {
			var store = new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>());
			var manager = new ApplicationRoleManager(store);

			return manager;
		}
		#endregion
	}

	public class EmailService : IIdentityMessageService {

		public Task SendAsync(IdentityMessage message) {
			string text = message.Body;
			string html = message.Body;
			//do whatever you want to the message        
			MailMessage msg = new MailMessage();
			msg.From = new MailAddress("hello@winerack.io", "winerack.io");
			msg.To.Add(new MailAddress(message.Destination));
			msg.Subject = message.Subject;
			msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
			msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

			SmtpClient smtpClient = new SmtpClient("smtp.mailgun.org");
			string emailUser = ConfigurationManager.AppSettings["smtp:user"];
			string emailPassword = ConfigurationManager.AppSettings["smtp:password"];
			System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(emailUser, emailPassword);
			smtpClient.Credentials = credentials;
			smtpClient.Send(msg);

			return Task.FromResult(0);
		}
	}

	public class SmsService : IIdentityMessageService {

		public Task SendAsync(IdentityMessage message) {
			// Plug in your sms service here to send a text message.
			return Task.FromResult(0);
		}
	}
}