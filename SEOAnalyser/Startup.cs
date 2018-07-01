using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SEOAnalyser.Web.Startup))]
namespace SEOAnalyser.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
