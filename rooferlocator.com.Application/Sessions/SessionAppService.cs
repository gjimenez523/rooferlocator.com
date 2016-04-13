using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Authorization;
using Abp.AutoMapper;
using rooferlocator.com.Sessions.Dto;

namespace rooferlocator.com.Sessions
{
    [AbpAuthorize]
    public class SessionAppService : comAppServiceBase, ISessionAppService
    {
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                User = (await GetCurrentUserAsync()).MapTo<UserLoginInfoDto>()
            };

            //NOTE:  This was commented becuase of integration with MyDesignHeroes
            //if (AbpSession.TenantId.HasValue)
            //{
            //    output.Tenant = (await GetCurrentTenantAsync()).MapTo<TenantLoginInfoDto>();
            //}

            return output;
        }
    }
}