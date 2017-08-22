using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASPnetPhishing.Startup))]
namespace ASPnetPhishing
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
