using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using System;
using rooferlocator.com.Common.Members;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;

namespace rooferlocator.com.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : comControllerBase
    {
        private readonly IMemberAppService _memberAppService;
        private readonly rooferlocator.com.Users.UserManager _userManager;

        public HomeController(IMemberAppService memberAppService, 
            rooferlocator.com.Users.UserManager userManager)
        {
            _memberAppService = memberAppService;
            _userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims2 = identity.Claims;
            var role = identity.FindFirst(ClaimTypes.Role).Value;

            if (role == "Member")
            {
                //Get the subscribers email to pass into CreditsHero in order to retreive summary
                var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);

                CreditsHero.Subscribers.Dtos.GetSubscribersInput input = new CreditsHero.Subscribers.Dtos.GetSubscribersInput()
                {
                    SubscribersId = Int32.Parse(user.Id.ToString()),
                    CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:APIKey"]),
                    SubscribersEmail = user.EmailAddress,
                    SubscribersName = user.UserName
                };

                var creditsHeroSubscriber = _memberAppService.GetMember(input);
                input.SubscribersId = creditsHeroSubscriber.Id;

                Models.Member.MemberSummaryViewModel results = new Models.Member.MemberSummaryViewModel();

                results.CreditsRemainingCount = 0;
                results.SubscriberInquiriesCount = _memberAppService.GetMemberInquiries(input).SubscriberInquiryList.Count;

                return View(results);
            }
            else
            {
                return View();
            }
        }
	}
}