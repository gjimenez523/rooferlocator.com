using Abp.Application.Features;
using rooferlocator.com.Authorization.Roles;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;

namespace rooferlocator.com.Features
{
    public class FeatureValueStore : AbpFeatureValueStore<Tenant, Role, User>
    {
        public FeatureValueStore(TenantManager tenantManager)
            : base(tenantManager)
        {
        }
    }
}