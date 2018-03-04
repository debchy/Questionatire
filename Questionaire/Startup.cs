using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Questionaire.Startup))]
namespace Questionaire
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
