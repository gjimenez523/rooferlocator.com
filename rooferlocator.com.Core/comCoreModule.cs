using System.Reflection;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Zero;
using Abp.Zero.Configuration;
using rooferlocator.com.Authorization;
using rooferlocator.com.Authorization.Roles;

namespace rooferlocator.com
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class comCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Remove the following line to disable multi-tenancy.
            //Configuration.MultiTenancy.IsEnabled = true;

            //Add/remove localization sources here
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    comConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "rooferlocator.com.Localization.Source"
                        )
                    )
                );

            //The following is commented out because of collision with CreditsHero Tenancy
            //AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            //The following is commented out because of collision with CreditsHero Tenancy
            //Configuration.Authorization.Providers.Add<comAuthorizationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
