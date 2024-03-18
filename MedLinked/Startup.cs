using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MedLinked.Startup))]
namespace MedLinked
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
