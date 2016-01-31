using Abp.Authorization;
using rooferlocator.com.Authorization.Roles;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;

namespace rooferlocator.com.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
