using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using rooferlocator.com.MultiTenancy.Dto;

namespace rooferlocator.com.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        ListResultOutput<TenantListDto> GetTenants();

        Task CreateTenant(CreateTenantInput input);
    }
}
