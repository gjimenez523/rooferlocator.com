using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Zero.Configuration;
using Abp.Modules;
using Abp.Web.Mvc;
using rooferlocator.com.Api;

namespace rooferlocator.com.Web
{
    [DependsOn(
        typeof(comDataModule), 
        typeof(comApplicationModule), 
        typeof(comWebApiModule),
        typeof(AbpWebMvcModule))]
    public class comWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Enable database based localization
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<comNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
