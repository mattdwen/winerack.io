using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(my.winerack.io.Startup))]
namespace my.winerack.io
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
