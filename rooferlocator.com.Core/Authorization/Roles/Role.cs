using Abp.Authorization.Roles;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;

namespace rooferlocator.com.Authorization.Roles
{
    public class Role : AbpRole<Tenant, User>
    {

    }
}