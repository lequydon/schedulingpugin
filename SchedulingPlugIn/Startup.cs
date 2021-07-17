using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SchedulingPlugIn.Startup))]
namespace SchedulingPlugIn
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
