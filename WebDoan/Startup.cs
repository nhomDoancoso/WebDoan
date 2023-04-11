using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebDoan.Startup))]
namespace WebDoan
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
