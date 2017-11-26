using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RaspberryPIWebApp.Startup))]
namespace RaspberryPIWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
