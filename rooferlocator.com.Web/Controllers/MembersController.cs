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

namespace rooferlocator.com.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Tenants)]
    public class MembersController : comControllerBase
    {
        private readonly IMemberAppService _memberAppService;
        private readonly rooferlocator.com.Users.UserManager _userManager;
        private readonly IRoofTypeAppService _roofTypeService;
        private readonly IServiceTypeAppService _serviceTypeService;
        private readonly rooferlocator.com.Common.Companies.ICompanyAppService _companyService2;
        private readonly rooferlocator.com.Common.Types.ILocationAppService _locationService;

        public MembersController(IMemberAppService memberAppService,
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

            if (role == "Member")
            {
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
                //rooferlocator.com.Common.Members.Dtos.MemberDto output = new Common.Members.Dtos.MemberDto()
                //{
                //    Id = creditsHeroSubscriber.Id,
                //    Email = creditsHeroSubscriber.Email,
                //    Phone = creditsHeroSubscriber.SmsNumber,
                //    FullName = creditsHeroSubscriber.FullName
                //};

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
            var roofTypes = _roofTypeService.GetRoofTypes(new CreditsHero.Common.Dtos.GetCriteriaInput()
            {
                CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:APIKey"]),
                CriteriaId = 0
            });

            var serviceTypes = _serviceTypeService.GetServiceTypes(new CreditsHero.Common.Dtos.GetCriteriaInput()
            {
                CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:APIKey"]),
                CriteriaId = 0
            });

            MemberServicesViewModel output = new MemberServicesViewModel()
            {
                RoofTypes = roofTypes,
                ServiceTypes = serviceTypes
            };

            return View(output);
        }

        public async Task<ActionResult> Payment()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims2 = identity.Claims;
            var role = identity.FindFirst(ClaimTypes.Role).Value;

            //Get Subscriber entity
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

            //Get Company entity so we can use the cost per credits
            var company = _companyService2.GetCompany(new CreditsHero.Common.Companies.Dtos.GetCompanyInput() { CompanyId = input.CompanyId.Value.ToString() });

            //Build Payment entity
            MemberPaymentViewModel memberPayment = new MemberPaymentViewModel()
            {
                SubscriberId = creditsHeroSubscriber.Id,
                CompanyId = input.CompanyId.Value,
                CostPerCredit = company.CostPerCredit,
                Credits = 0
            };

            return View(memberPayment);
        }
        public async Task<ActionResult> PurchaseCredits()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims2 = identity.Claims;
            var role = identity.FindFirst(ClaimTypes.Role).Value;

            //Get Subscriber entity
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

            var companyInput = new CreditsHero.Common.Companies.Dtos.GetCompanyInput() { CompanyId = input.CompanyId.Value.ToString() };
            //Get Company entity so we can use the cost per credits
            var company = _companyService2.GetCompany(companyInput);

            //Get Company Configuration
            var companyConfig = _companyService2.GetCompanyConfig(companyInput);

            //Determine type of payment

            //Build PaymentAuthorize.NET data object
            PaymentAuthorizeNetDto payment = new PaymentAuthorizeNetDto()
            {
                Credits = Int32.Parse(Request.Form["txtCredits"]),
                SubscribersId = input.SubscribersId,
                SubscribersEmail = input.SubscribersEmail,
                SubscribersName = input.SubscribersName,
                Amount = Decimal.Parse(Request.Form["txtTotal"]),
                CardCode = Request.Form["txtCardCode"],
                CompanyId = company.Id,
                MarketType = 0, //Request.Form[],
                ExpirationDate = Request.Form["txtExpirationDate"],
                PaymentGatewayType = "AuthorizeNET", //Request.Form[],
                PaymentMethod = "ChargeCreditCard", //Request.Form[],
                PurchaseDescription = String.Format("Purchase made by {0}, email {1}, ID {2}", input.SubscribersName, input.SubscribersEmail, input.SubscribersId.ToString()),
                TaxAmount = Decimal.Parse(Request.Form["txtAmount"]),
                TransactionType = "AuthorizeAndCapture",
                CardNumber = Request.Form["txtCardNumber"],
                CompanyConfigurationSettings = companyConfig
            };

            var output = _memberAppService.MakePayment(payment);
            return Redirect((Url.Action("Index", "Home")));
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
            MemberLocationsViewModel output = new MemberLocationsViewModel();

            //Get States
            GetLocationOutput statesList = _locationService.GetStates();
            List<SelectListItem> listItem = new List<SelectListItem>();
            for (int rows = 0; rows <= statesList.Locations.Count - 1; rows++)
            {
                listItem.Add(new SelectListItem { Text = statesList.Locations[rows].State, Value = statesList.Locations[rows].State });
            }
            ViewData["States"] = listItem;

            output.States = statesList;
            return View(output);
        }

        public async Task<ActionResult> Quote(string requestId)
        {
            //Get the subscribers email to pass into CreditsHero in order to retreive summary
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);

            CreditsHero.Subscribers.Dtos.GetSubscribersRequestDetailInput inputRequest = new CreditsHero.Subscribers.Dtos.GetSubscribersRequestDetailInput()
            {
                SubscribersId = Int32.Parse(user.Id.ToString()),
                CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:APIKey"]),
                SubscribersEmail = user.EmailAddress,
                SubscribersName = user.UserName,
                RequestId = Int32.Parse(requestId)
            };
            CreditsHero.Subscribers.Dtos.GetSubscribersInput input = new CreditsHero.Subscribers.Dtos.GetSubscribersInput()
            {
                SubscribersId = Int32.Parse(user.Id.ToString()),
                CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:APIKey"]),
                SubscribersEmail = user.EmailAddress,
                SubscribersName = user.UserName
            };

            var creditsHeroSubscriber = _memberAppService.GetMember(input);
            input.SubscribersId = creditsHeroSubscriber.Id;
            inputRequest.SubscribersId = creditsHeroSubscriber.Id;

            Models.Member.MemberRequestDetailViewModel results = new Models.Member.MemberRequestDetailViewModel();
            List<SubscribersRequestDetailDto> subscriberRequestDetails = _memberAppService.GetMemberRequestDetails(inputRequest).SubscriberRequestDetailsList;
            results.SubscriberRequestDetails = new List<SubscribersRequestDetailDto>();

            results.SubscriberRequestDetails = subscriberRequestDetails;
            results.RequestId = Int32.Parse(requestId);
            results.SubscriberId = creditsHeroSubscriber.Id;

            return View(results);
        }
        public async Task<ActionResult> SendQuote()
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

            CreditsHero.Messaging.Dtos.GetQuotesInput inputQuote = new CreditsHero.Messaging.Dtos.GetQuotesInput()
            {
                Cost = Decimal.Parse(Request.Form["txtPrice"]),
                Message = Request.Form["txtMessage"],
                RequestRefId = Int32.Parse(Request.Form["RequestId"]),
                SubscriberRefId = creditsHeroSubscriber.Id
            };

            //Models.Member.MemberRequestDetailViewModel results = new Models.Member.MemberRequestDetailViewModel();
            GetQuotesResults results = _memberAppService.SendQuote(inputQuote);
            //results.SubscriberRequestDetails = new List<SubscribersRequestDetailDto>();

            //results.SubscriberRequestDetails = quoteResults;

            return Redirect((Url.Action("Index", "Home")));
        }
    }
}