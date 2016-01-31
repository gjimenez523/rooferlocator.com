using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace rooferlocator.com.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : comControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}