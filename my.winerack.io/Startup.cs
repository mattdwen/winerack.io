using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(winerack.Startup))]

namespace winerack {

	public partial class Startup {

		public void Configuration(IAppBuilder app) {
			ConfigureAuth(app);
		}
	}
}