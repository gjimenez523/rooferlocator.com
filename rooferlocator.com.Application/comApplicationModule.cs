using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;

namespace rooferlocator.com
{
    [DependsOn(typeof(comCoreModule), typeof(AbpAutoMapperModule))]
    public class comApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
