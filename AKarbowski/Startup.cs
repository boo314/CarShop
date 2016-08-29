using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AKarbowski.Startup))]
namespace AKarbowski
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
