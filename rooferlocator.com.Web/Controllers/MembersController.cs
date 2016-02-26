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

        public async Task<ActionResult> ServicesOffered()
        {
            string role = GetUserRole();
            Users.User user = await GetUser();
            TempData.Add("Role", role);

            #region Get Category List
            var categories = _memberAppService.GetCriteria(new GetSubscribersInput() { CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:APIKey"]) });
            TempData.Add("Categories", categories);
            #endregion

            #region Get Criteria List
            System.Collections.Hashtable criteriaList = new System.Collections.Hashtable();
            List<CreditsHero.Common.Dtos.CriteriaValuesDto> criteriaList2 = new List<CreditsHero.Common.Dtos.CriteriaValuesDto>();
            foreach (var item in categories.Criteria)
            {
                var service = _memberAppService.GetCriteriaValues(new CreditsHero.Common.Dtos.GetCriteriaInput()
                {
                    CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:APIKey"]),
                    CriteriaId = item.Id
                });
                criteriaList.Add(item.Id, service.CriteriaValues);
                //criteriaList2.Concat<CreditsHero.Common.Dtos.CriteriaValuesDto>(service.CriteriaValues);
                criteriaList2.AddRange(service.CriteriaValues);
            }
            TempData.Add("CriteriaList", criteriaList);
            #endregion

            #region Get members Criteria List
            if (role != "Admin")
            {
                CreditsHero.Subscribers.Dtos.GetSubscribersInput input = await BuildCreditsHeroSubscriberInput(user);

                var selectedMemberCriteria = _memberAppService.GetMemberSubscriptions(input);

                foreach (var item in selectedMemberCriteria.SubscriberSkills)
                {
                    foreach (var item2 in criteriaList2)
                    {
                        item2.IsSelected = item.Value.Exists(x => x.Name == item2.Name);
                    }

                }
            }
            #endregion

            Common.Types.Dtos.GetCriteriaValuesOutput listResults = new Common.Types.Dtos.GetCriteriaValuesOutput();
            listResults.CriteriaList = criteriaList2;

            MemberServicesViewModel output = new MemberServicesViewModel()
            {
                CriteriaValues = listResults
            };

            return View(output);
        }

        public async Task<ActionResult> Payment()
        {
            string role = GetUserRole();
            Users.User user = await GetUser();
            CreditsHero.Subscribers.Dtos.GetSubscribersInput input = await BuildCreditsHeroSubscriberInput(user);

            //Get Company entity so we can use the cost per credits
            var company = _companyService2.GetCompany(new CreditsHero.Common.Companies.Dtos.GetCompanyInput() { CompanyId = input.CompanyId.Value.ToString() });

            //Build Payment entity
            MemberPaymentViewModel memberPayment = new MemberPaymentViewModel()
            {
                SubscriberId = input.SubscribersId.Value,
                CompanyId = input.CompanyId.Value,
                CostPerCredit = company.CostPerCredit,
                Credits = 0
            };

            return View(memberPayment);
        }

        public async Task<ActionResult> PurchaseCredits()
        {
            string role = GetUserRole();
            Users.User user = await GetUser();
            CreditsHero.Subscribers.Dtos.GetSubscribersInput input = await BuildCreditsHeroSubscriberInput(user);

            var companyInput = new CreditsHero.Common.Companies.Dtos.GetCompanyInput() { CompanyId = input.CompanyId.Value.ToString() };
            //Get Company entity so we can use the cost per credits
            var company = _companyService2.GetCompany(companyInput);

            //Get Company Configuration
            var companyConfig = _companyService2.GetCompanyConfig(companyInput);

            //TODO:Determine type of payment

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
            string role = GetUserRole();
            Users.User user = await GetUser();
            CreditsHero.Subscribers.Dtos.GetSubscribersInput input = await BuildCreditsHeroSubscriberInput(user);

            CreditsHero.Messaging.Dtos.GetQuotesInput inputQuote = new CreditsHero.Messaging.Dtos.GetQuotesInput()
            {
                Cost = Decimal.Parse(Request.Form["txtPrice"]),
                TotalCredits = Decimal.Parse(Request.Form["totalCredits"]),
                Message = Request.Form["txtMessage"],
                RequestRefId = Int32.Parse(Request.Form["RequestId"]),
                SubscriberRefId = input.SubscribersId.Value,
                CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:APIKey"])
            };

            GetQuotesResults results = _memberAppService.SendQuote(inputQuote);

            return Redirect((Url.Action("Index", "Home")));
        }

        public async Task<ActionResult> UpdateQuote()
        {
            string role = GetUserRole();
            Users.User user = await GetUser();
            CreditsHero.Subscribers.Dtos.GetSubscribersInput input = await BuildCreditsHeroSubscriberInput(user);

            CreditsHero.Messaging.Dtos.GetQuotesInput inputQuote = new CreditsHero.Messaging.Dtos.GetQuotesInput()
            {
                QuoteId = Int32.Parse(Request.Form.Keys[0].ToString()),
                QuoteStatus = "Hired",
                SubscriberRefId = input.SubscribersId.Value,
                CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:APIKey"])
            };

            GetQuotesResults results = _memberAppService.UpdateQuote(inputQuote);

            return Redirect((Url.Action("Index", "Home")));
        }

        public async Task<ActionResult> SendAdminEmail()
        {
            string role = GetUserRole();
            Users.User user = await GetUser();
            CreditsHero.Subscribers.Dtos.GetSubscribersInput input = await BuildCreditsHeroSubscriberInput(user);

            CreditsHero.Messaging.Dtos.NotificationInput inputNotification = new CreditsHero.Messaging.Dtos.NotificationInput()
            {
                EmailFrom = input.SubscribersEmail,
                EmailTo = null,
                EmailMessage = String.Format("{0}<p/>Company:{1}<br/>SubscriberId:{2}", Request.Form["txtMessage"], input.CompanyId, input.SubscribersId),
                EmailSubject = String.Format("Notification From {0}", input.SubscribersName),
                CompanyId = input.CompanyId.Value
            };

            var notificationResults = _memberAppService.SendEmail(inputNotification);
            rooferlocator.com.Web.Models.ServiceResponse response = new Models.ServiceResponse()
            {
                FriendlyMessage = notificationResults.ResponseMessage
            };

            return Redirect((Url.Action("Index", "Home", response)));
        }

        public async Task<ActionResult> AddSubscribersValue()
        {
            string role = GetUserRole();
            Users.User user = await GetUser();
            CreditsHero.Subscribers.Dtos.GetSubscribersInput input = await BuildCreditsHeroSubscriberInput(user);

            CreateSubscribersValuesInput inputValues = new CreateSubscribersValuesInput();

            var selectedMemberCriteria = _memberAppService.GetMemberSubscriptions(input);

            foreach (var requestItem in Request.Form.Keys)
            {
                foreach (var item in selectedMemberCriteria.SubscriberSkills)
                {
                    var selectedItem = item.Value.Find(x => x.Id == Int32.Parse(requestItem.ToString()));
                    inputValues.SubscribersId = input.SubscribersId.Value;

                    if (selectedItem == null)
                    {
                        inputValues.CriteriaValuesRefId = Int32.Parse(requestItem.ToString());
                        inputValues.IsDeleted = Request.Form.GetValues(requestItem.ToString())[0] == "on" ? false : true;
                    }
                    else
                    {
                        inputValues.CriteriaValuesRefId = selectedItem.Id;
                        inputValues.IsDeleted = Request.Form.GetValues(selectedItem.Id.ToString())[0] == "on" ? false : true;
                    }

                    _memberAppService.AddSubscribersValue(inputValues);
                }
            }

            //Need to do this to clean up the skills that have been removed from the list
            foreach (var item in selectedMemberCriteria.SubscriberSkills)
            {
                var unselectedItems = item.Value.Where(x => !Request.Form.AllKeys.Any(i => i == x.Id.ToString())).ToList();
                foreach (var unselectedItem in unselectedItems)
                {
                    inputValues.SubscribersId = input.SubscribersId.Value;
                    inputValues.CriteriaValuesRefId = unselectedItem.Id;
                    inputValues.IsDeleted = true;
                    _memberAppService.AddSubscribersValue(inputValues);
                }
            }


            rooferlocator.com.Web.Models.ServiceResponse response = new Models.ServiceResponse()
            {
                FriendlyMessage = "Service Updated."
            };

            return Redirect((Url.Action("ServicesOffered", "Members", response)));
        }

        public async Task<ActionResult> AddCriteriaValue()
        {
            //TODO:  Need to update CriteriaRefId
            var criteriaResults = _memberAppService.AddCriteriaValue(new CreditsHero.Common.Dtos.CreateCriteriaValuesInput()
            {
                CreditCount = Int32.Parse(Request.Form[2].ToString()),
                Name = Request.Form[1].ToString(),
                CriteriaRefId = Int32.Parse(Request.Form[0].ToString())
            });

            rooferlocator.com.Web.Models.ServiceResponse response = new Models.ServiceResponse()
            {
                FriendlyMessage = "Service Updated."
            };

            return Redirect((Url.Action("ServicesOffered", "Members", response)));
        }
        
        public async Task<ActionResult> AddCriteria()
        {
            Company currentCompany = new Company() { Id = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:APIKey"]) };
            var criteriaResults = _memberAppService.AddCriteria(new CreditsHero.Common.Dtos.CreateCriteriaInput()
            {
                Company = currentCompany,
                Name = Request["txtCriteriaName"].ToString()
            });

            rooferlocator.com.Web.Models.ServiceResponse response = new Models.ServiceResponse()
            {
                FriendlyMessage = "Service Updated."
            };

            return Redirect((Url.Action("ServicesOffered", "Members", response)));
        }
    }
}