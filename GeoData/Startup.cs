using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GeoData.Startup))]
namespace GeoData
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
