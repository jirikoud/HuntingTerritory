using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HuntingApplication.Startup))]
namespace HuntingApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
