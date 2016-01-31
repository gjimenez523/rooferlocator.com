using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using rooferlocator.com.Authorization.Roles;
using rooferlocator.com.Editions;
using rooferlocator.com.Users;

namespace rooferlocator.com.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, Role, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository, 
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository, 
            EditionManager editionManager) 
            : base(
                tenantRepository, 
                tenantFeatureRepository, 
                editionManager
            )
        {
        }
    }
}