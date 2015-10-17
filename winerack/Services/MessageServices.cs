using System.Threading.Tasks;
using Mandrill;
using Mandrill.Model;
using Microsoft.Framework.Configuration;

namespace winerack.Services
{
  // This class is used by the application to send Email and SMS
  // when you turn on two-factor authentication in ASP.NET Identity.
  // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
  public class AuthMessageSender : IEmailSender, ISmsSender
  {
    #region Constructor

    public AuthMessageSender(IConfiguration config)
    {
      var mandrillApiKey = config["Authentication:Mandrill:ApiKey"];
      _mandrill = new MandrillApi(mandrillApiKey);
    }

    #endregion Constructor

    #region Declarations

    private readonly MandrillApi _mandrill;

    #endregion Declarations

    #region Public Methods

    public Task SendEmailAsync(string email, string subject, string message)
    {
      var mandrillMessage = new MandrillMessage("no-reply@winerack.io", email, subject, message);
      return _mandrill.Messages.SendAsync(mandrillMessage);
    }

    public Task SendSmsAsync(string number, string message)
    {
      // Plug in your SMS service here to send a text message.
      return Task.FromResult(0);
    }

    #endregion Public Methods
  }
}