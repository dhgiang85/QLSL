using System.Data.Entity;
using Microsoft.Owin;
using Owin;
using QLSL.DAL;
using QLSL.Migrations;

[assembly: OwinStartupAttribute(typeof(QLSL.Startup))]
namespace QLSL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<QLSLContext, Configuration>());
            ConfigureAuth(app);


        }
    }
}
