using System.Threading.Tasks;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using Microsoft.Data.Entity;
using winerack.Models;
using winerack.Services;

namespace winerack.TagHelpers
{
  [HtmlTargetElement("img", Attributes = UserIdAttributeName)]
  public class UserProfileImageTagHelper : TagHelper
  {
    #region Constructor

    public UserProfileImageTagHelper(ApplicationDbContext dbContext, AzureService azure)
    {
      _dbContext = dbContext;
      _azure = azure;
    }

    #endregion Constructor

    #region Constants

    private const string UserIdAttributeName = "user-profile-id";

    #endregion Constants

    #region Declarations

    private readonly ApplicationDbContext _dbContext;
    private readonly AzureService _azure;

    #endregion Declarations

    #region Implementation

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
      var userId = context.AllAttributes[UserIdAttributeName].Value.ToString();
      var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

      output.Attributes["src"] = "/images/profile-default.png";

      if (user != null)
      {
        output.Attributes["alt"] = user.Name;

        if (user.ImageID.HasValue)
        {
          output.Attributes["src"] = _azure.GetUrl("profiles", $"{user.ImageID.Value}.jpg");
        }
      }

      await base.ProcessAsync(context, output);
    }

    #endregion Implementation
  }
}