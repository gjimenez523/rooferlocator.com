using Abp.Application.Services;
using Abp.Dependency;
using System.Threading.Tasks;

namespace rooferlocator.com.Common
{
    public interface ICreditsHeroConnect : ITransientDependency
    {
        object CallCreditsHeroService<T>(object results, object input, string servicePostFix);
    }
}
