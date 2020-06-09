using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SpitTree.Startup))]
namespace SpitTree
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
