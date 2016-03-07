using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using rooferlocator.com.Authorization;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Common.Members;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using rooferlocator.com.Common.Types;
using rooferlocator.com.Web.Models.Member;
using CreditsHero.Common.Companies;
using CreditsHero.Subscribers.Dtos;
using CreditsHero.Messaging.Dtos;
using rooferlocator.com.Common.Types.Dtos;
using CreditsHero.Common.Companies.Dtos;
using CreditsHero.Subscribers.Dtos.PaymentGatewayDtos;
using System.Linq;
using CreditsHero.Common.Dtos;

namespace rooferlocator.com.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Tenants)]
    public class CustomersController : comControllerBase
    {
        private readonly IMemberAppService _memberAppService;
        private readonly rooferlocator.com.Users.UserManager _userManager;
        private readonly IRoofTypeAppService _roofTypeService;
        private readonly IServiceTypeAppService _serviceTypeService;
        private readonly rooferlocator.com.Common.Companies.ICompanyAppService _companyService2;
        private readonly rooferlocator.com.Common.Types.ILocationAppService _locationService;

        public CustomersController(IMemberAppService memberAppService,
            rooferlocator.com.Users.UserManager userManager,
            IRoofTypeAppService roofTypeService,
            IServiceTypeAppService serviceTypeService,
            rooferlocator.com.Common.Companies.ICompanyAppService companyService2,
            rooferlocator.com.Common.Types.ILocationAppService locationService)
        {
            _memberAppService = memberAppService;
            _userManager = userManager;
            _roofTypeService = roofTypeService;
            _serviceTypeService = serviceTypeService;
            _companyService2 = companyService2;
            _locationService = locationService;
        }

        private string GetUserRole()
        {
            //Get the users Identity
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims2 = identity.Claims;
            var role = identity.FindFirst(ClaimTypes.Role).Value;
            return role;
        }

        private async Task<Users.User> GetUser()
        {
            //Get the ConciergesWorldwide user
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            return user;
        }

        private async Task<CreditsHero.Subscribers.Dtos.GetSubscribersInput> BuildCreditsHeroSubscriberInput(Users.User appUser)
        {
            //Build the Credits Hero Subscriber/User input object.  This object is used throughout the site in order
            //  to retrieve CreditsHeros' version of the user/subscriber
            CreditsHero.Subscribers.Dtos.GetSubscribersInput input = new CreditsHero.Subscribers.Dtos.GetSubscribersInput()
            {
                SubscribersId = Int32.Parse(appUser.Id.ToString()),
                CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:APIKey"]),
                SubscribersEmail = appUser.EmailAddress,
                SubscribersName = appUser.UserName
            };

            var creditsHeroSubscriber = _memberAppService.GetMember(input);
            input.SubscribersId = creditsHeroSubscriber.Id;

            return input;
        }

        public async Task<ActionResult> Index()
        {
            string role = GetUserRole();
            Users.User user = await GetUser();

            if (role == "Member")
            {
                CreditsHero.Subscribers.Dtos.GetSubscribersInput input = await BuildCreditsHeroSubscriberInput(user);
                //Get Member Info (credits hero)
                var outputSubscriber = _memberAppService.GetMember(input);

                //Get Member Info
                var outputMember = _memberAppService.GetMembers(new Common.Members.Dtos.GetMemberInput()
                {
                    MemberId = Int32.Parse(user.Id.ToString())
                });

                //Get Member Subscriptions
                var outputSubscriptions = _memberAppService.GetMemberSubscriptions(input);

                rooferlocator.com.Common.Members.Dtos.MemberDto output = outputMember.Members[0];
                output.SubscriberSkills = outputSubscriptions;

                return View("Detail", output);
            }
            else
            {
                var output = _memberAppService.GetMembers(
                    new Common.Members.Dtos.GetMemberInput()
                    {
                        CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:APIKey"]),
                        MemberId = null
                    });
                return View("Index", output);
            }
        }
    }
}