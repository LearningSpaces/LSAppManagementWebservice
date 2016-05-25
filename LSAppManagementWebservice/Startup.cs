using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LSAppManagementWebservice.Startup))]
namespace LSAppManagementWebservice
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
