using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using rooferlocator.com.Authorization;
using rooferlocator.com.MultiTenancy;

namespace rooferlocator.com.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantsController : comControllerBase
    {
        private readonly ITenantAppService _tenantAppService;

        public TenantsController(ITenantAppService tenantAppService)
        {
            _tenantAppService = tenantAppService;
        }

        public ActionResult Index()
        {
            var output = _tenantAppService.GetTenants();
            return View(output);
        }
    }
}