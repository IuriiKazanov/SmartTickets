using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SmartTickets.Startup))]
namespace SmartTickets
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
