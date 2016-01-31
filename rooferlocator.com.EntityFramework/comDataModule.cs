using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using rooferlocator.com.EntityFramework;

namespace rooferlocator.com
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(comCoreModule))]
    public class comDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
