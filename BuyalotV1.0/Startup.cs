using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BuyalotV1._0.Startup))]
namespace BuyalotV1._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           // ConfigureAuth(app);
        }
    }
}
