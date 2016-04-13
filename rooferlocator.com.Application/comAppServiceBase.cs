using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;
using Microsoft.AspNet.Identity;
using Abp.AutoMapper;

namespace rooferlocator.com
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class comAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }
        //public MyDesignHero.MultiTenancy.TenantManager MdhTenantManager { get; set; }

        public UserManager UserManager { get; set; }
        //public MyDesignHero.Users.UserManager MdhUserManager { get; set; }

        protected comAppServiceBase()
        {
            LocalizationSourceName = comConsts.LocalizationSourceName;
        }

        protected async virtual Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId());
            if (user == null)
            {
                throw new ApplicationException("There is no current user!");
            }

            return user;
            
        }

        protected async virtual Task<Tenant> GetCurrentTenantAsync()
        {
            var tenant = await TenantManager.GetByIdAsync(AbpSession.GetTenantId());

            return tenant;
            
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}