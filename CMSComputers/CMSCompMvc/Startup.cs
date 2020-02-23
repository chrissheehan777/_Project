using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CMSCompMvc.Startup))]
namespace CMSCompMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
