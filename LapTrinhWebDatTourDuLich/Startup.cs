using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LapTrinhWebDatTourDuLich.Startup))]
namespace LapTrinhWebDatTourDuLich
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
