using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using rooferlocator.com.Authorization;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Common.Members;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rooferlocator.com.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Tenants)]
    public class MembersController : comControllerBase
    {
        private readonly IMemberAppService _memberAppService;
        private readonly rooferlocator.com.Users.UserManager _userManager;

        public MembersController(IMemberAppService memberAppService,
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

            //return View(new rooferlocator.com.Web.Models.Member.MemberSummaryViewModel());
            if (role == "Member")
            {
                var outputSubscriber = _memberAppService.GetMember(input);

                rooferlocator.com.Common.Members.Dtos.MemberDto output = new Common.Members.Dtos.MemberDto()
                {
                    Id = creditsHeroSubscriber.Id,
                    Email = creditsHeroSubscriber.Email,
                    Phone = creditsHeroSubscriber.SmsNumber,
                    FullName = creditsHeroSubscriber.FullName
                };
                return View("Detail", output);
            }
            else
            {
                var output = _memberAppService.GetMembers(
                    new Common.Members.Dtos.GetMemberInput()
                    {
                        MemberId = null
                    });
                return View("Index", output);
            }
        }

        public ActionResult ServicesOffered()
        {
            var output = _memberAppService.GetMembers(
                new Common.Members.Dtos.GetMemberInput()
                {
                    MemberId = null
                });
            return View(output);
        }

        public ActionResult Payment()
        {
            var output = _memberAppService.GetMembers(
                new Common.Members.Dtos.GetMemberInput()
                {
                    MemberId = null
                });
            return View(output);
        }

        public ActionResult Email()
        {
            var output = _memberAppService.GetMembers(
                new Common.Members.Dtos.GetMemberInput()
                {
                    MemberId = null
                });
            return View(output);
        }

        public ActionResult Location()
        {
            var output = _memberAppService.GetMembers(
                new Common.Members.Dtos.GetMemberInput()
                {
                    MemberId = null
                });
            return View(output);
        }
    }
}