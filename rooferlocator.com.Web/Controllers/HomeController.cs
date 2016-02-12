using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using System;
using rooferlocator.com.Common.Members;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using CreditsHero.Subscribers.Dtos;
using CreditsHero.Messaging.Dtos;

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
                Models.Member.MemberQuotesViewModel resultsQuotes = new Models.Member.MemberQuotesViewModel();
                List<SubscribersInquiryDto> subscriberInquiries = _memberAppService.GetMemberInquiries(input).SubscriberInquiryList;
                List<SubscribersRequestDto> subscriberRequests = _memberAppService.GetMemberRequests(input).SubscriberRequestList;
                List<SubscriberQuoteDto> subscriberQuotes = _memberAppService.GetMemberQuotes(input).SubscriberQuotesList;

                results.SubscriberInquiries.SubscriberInquiryList = new List<SubscribersInquiryDto>();

                results.CreditsRemainingCount = 0;
                results.SubscriberInquiriesCount = subscriberInquiries.Count;
                //Build Inquiry Model
                results.SubscriberInquiries.SubscriberInquiryList = subscriberInquiries;
                //Build Requests Model
                results.SubscriberRequests = subscriberRequests;
                foreach(var item in subscriberQuotes)
                {
                    var subscriberRequestItem = results.SubscriberRequests.Find(x => x.RequestId == item.RequestId);
                    if (subscriberRequestItem != null)
                    {
                        results.SubscriberRequests.Remove(subscriberRequestItem);
                    }
                }
                //Build Quotes Model
                resultsQuotes.SubscriberQuotes = subscriberQuotes;
                results.SubscriberQuotes = resultsQuotes;


                return View(results);
            }
            else
            {
                return View();
            }
        }
	}
}