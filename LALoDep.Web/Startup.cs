using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LALoDep.Startup))]
namespace LALoDep
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
