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

        [HttpPost]
        public async Task<ActionResult> Detail()
        {
            //Get UserId
            long userId = long.Parse(Request.Form["Command"].Split('_')[1]);
            rooferlocator.com.Web.Models.ServiceResponse response = new Models.ServiceResponse();
            var user = await _userManager.GetUserByIdAsync(userId);

            //Determine if we are Deleting, Resetting, or Activating user
            if (Request.Form["Command"].Contains("ResetPassword"))
            {
                #region ResetPassword Command
                //Generate password reset code for user
                string resetCode = System.Web.Security.Membership.GeneratePassword(5, 2);

                //Save user with password reset code
                user.PasswordResetCode = resetCode;
                await _userManager.UpdateAsync(user);

                //Send email to user with password reset code
                string role = GetUserRole();
                CreditsHero.Subscribers.Dtos.GetSubscribersInput input = await BuildCreditsHeroSubscriberInput(user);

                //NOTE: If EmailTo/EmailFrom is null then email is submitted/replyto to configured admin
                CreditsHero.Messaging.Dtos.NotificationInput inputNotification = new CreditsHero.Messaging.Dtos.NotificationInput()
                {
                    EmailFrom = null,
                    EmailTo = user.EmailAddress,
                    EmailMessage = String.Format("<!DOCTYPE html><html lang=en xmlns=http://www.w3.org/1999/xhtml><head><meta charset=utf-8 /><title></title></head><body style='background-color:#3d1617;text-align:center;padding:0px;margin:0px'><div class='col-lg-3 col-md-3 col-sm-3 col-xs-3' style='background:white'></div><div class='col-lg-4 col-md-4 col-sm-4 col-xs-4' style='padding:0px;margin:0px'><div class=row><div class='col-lg-12 col-md-12 col-sm-12 col-xs-12'><img style='width:100%' src=http://www.conciergesworldwide.com/images/imgHeader.jpg /></div></div><div class=row style='padding:10px;height:50px;font-family:Arial;font-size:24pt'><div class='col-lg-12 col-md-12 col-sm-12 col-xs-12' style='border:2px solid white;border-radius:5px'><div class=row style='background-color:gainsboro'><div>Password Reset</div></div><div class='col-lg-12 col-md-12 col-sm-12 col-xs-12' style='padding:10px;color:white'><div style='font-size:16;'>Hello {0}, </p> To reset your password, you will need to enter the following Password Reset Code using the link below. Your password reset code is:<p/><div style='font-size:22;padding:10px;margin:10px;'>{1}</div> <p/></div></div><div class=row style='background-color:gainsboro'><div><a href='{2}{3}'>Click here to reset your password</a></div></div><div class='col-lg-12 col-md-12 col-sm-12 col-xs-12' style='border-top:1px solid gainsboro;padding:10px;color:white'><div></div></div></div></div><div class=row></div></div><div class='col-lg-3 col-md-3 col-sm-3 col-xs-3' style='padding:0px;margin:0px'></div></body></html>"
                        , user.Name
                        , user.PasswordResetCode
                        , System.Web.Configuration.WebConfigurationManager.AppSettings["dashboardHero:DashboardPrefix"]
                        , "/Account/ResetPassword"),
                    EmailSubject = String.Format("Password Reset", input.SubscribersName),
                    CompanyId = input.CompanyId.Value
                };

                var notificationResults = _memberAppService.SendEmail(inputNotification);
                response = new Models.ServiceResponse()
                {
                    FriendlyMessage = notificationResults.ResponseMessage
                };

                return Redirect((Url.Action("Index", "Members", response)));
                #endregion
            }
            else if (Request.Form["Command"].Contains("UserDetails"))
            {
                #region ShowDetails Command
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
                #endregion
            }
            else if (Request.Form["Command"].Contains("Activate"))
            {
                #region ActivateUser Command
                user.IsDeleted = false;
                var results = await _userManager.UpdateAsync(user);

                if (results.Succeeded)
                {
                    response = new Models.ServiceResponse()
                    {
                        FriendlyMessage = "Member has been removed."
                    };
                }

                return Redirect((Url.Action("Index", "Members", response)));
                #endregion
            }
            else if (Request.Form["Command"].Contains("Delete"))
            {
                #region DeleteUser Command
                user.IsDeleted = true;
                var results = await _userManager.UpdateAsync(user);

                if (results.Succeeded)
                {
                    response = new Models.ServiceResponse()
                    {
                        FriendlyMessage = "Member has been removed."
                    };
                }

                return Redirect((Url.Action("Index", "Members", response)));
                #endregion
            }
            return View();
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

        public async Task<ActionResult> PostProcessPayment()
        {
            string role = GetUserRole();
            Users.User user = await GetUser();
            CreditsHero.Subscribers.Dtos.GetSubscribersInput input = await BuildCreditsHeroSubscriberInput(user);

            var companyInput = new CreditsHero.Common.Companies.Dtos.GetCompanyInput() { CompanyId = input.CompanyId.Value.ToString() };
            //Get Company entity so we can use the cost per credits
            var company = _companyService2.GetCompany(companyInput);

            //Get Company Configuration
            var companyConfig = _companyService2.GetCompanyConfig(companyInput);

            PaymentResponseDto paymentResponse = new PaymentResponseDto();
            PaymentDto payment = new PaymentDto();

            //TODO: Determine type of payment
            //PayPal Post Payment Process
            payment = new PaymentPaypalDto()
            {
                CompanyConfigurationSettings = companyConfig,
                CompanyId = company.Id,
                PayerId = Request.QueryString["PayerID"],
                PaymentGuid = Request.QueryString["Guid"],
                PaymentId = Request.QueryString["PaymentId"],
                Token = Request.QueryString["Token"],
                PaymentGatewayType = "PayPal",
                PaymentMethod = "PayPal",
                SubscribersEmail = input.SubscribersEmail,
                SubscribersId = input.SubscribersId,
                SubscribersName = input.SubscribersName,
                TaxAmount = 0,
                TransactionType = "PayPalExecute"
            };
            paymentResponse = _memberAppService.MakePayment(payment as PaymentPaypalDto);
            if (paymentResponse.AuthCode == "created")
            {
                return Redirect(paymentResponse.ResponseCode);
            }
            else
            {
                rooferlocator.com.Web.Models.ServiceResponse response = new Models.ServiceResponse()
                {
                    FriendlyMessage = paymentResponse.MessageCode == "sale"
                        ? String.Format("Payment accepted.  Transaction #: ", paymentResponse.TransactionId)
                        : String.Format("Processing Payment.  Transaction #: ", paymentResponse.TransactionId)
                };

                return Redirect((Url.Action("Index", "Home", response)));
            }
            //?guid = 3772 & paymentId = PAY - 5UN73352KF9557105K3KJ2JY & token = EC - 1BC22799SN423381H & PayerID = WWWCHTZE5U74L
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

            //Build Stripe data payment
            PaymentStripeDto payment = new PaymentStripeDto()
            {
                AddressCity = Request.Form[""],
                AddressLine1 = Request.Form[""],
                AddressLine2 = Request.Form[""],
            };

            //Build PaymentAuthorize.NET data object
            //PaymentAuthorizeNetDto payment = new PaymentAuthorizeNetDto()
            //{
            //    Credits = Int32.Parse(Request.Form["txtCredits"]),
            //    SubscribersId = input.SubscribersId,
            //    SubscribersEmail = input.SubscribersEmail,
            //    SubscribersName = input.SubscribersName,
            //    Amount = Decimal.Parse(Request.Form["txtTotal"]),
            //    CardCode = Request.Form["txtCardCode"],
            //    CompanyId = company.Id,
            //    MarketType = 0, //Request.Form[],
            //    ExpirationDate = Request.Form["txtExpirationDate"],
            //    PaymentGatewayType = "AuthorizeNET", //Request.Form[],
            //    PaymentMethod = "ChargeCreditCard", //Request.Form[],
            //    PurchaseDescription = String.Format("Purchase made by {0}, email {1}, ID {2}", input.SubscribersName, input.SubscribersEmail, input.SubscribersId.ToString()),
            //    TaxAmount = Decimal.Parse(Request.Form["txtAmount"]),
            //    TransactionType = "AuthorizeAndCapture",
            //    CardNumber = Request.Form["txtCardNumber"],
            //    CompanyConfigurationSettings = companyConfig
            //};

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
            requestId = requestId == null ? Request.Form["PassRequestId"] : requestId;
            Models.Member.MemberRequestDetailViewModel results = new Models.Member.MemberRequestDetailViewModel();
            //Get the subscribers email to pass into CreditsHero in order to retreive summary
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);

            CreditsHero.Subscribers.Dtos.GetSubscribersInput input = new CreditsHero.Subscribers.Dtos.GetSubscribersInput()
            {
                SubscribersId = Int32.Parse(user.Id.ToString()),
                CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:APIKey"]),
                SubscribersEmail = user.EmailAddress,
                SubscribersName = user.UserName
            };

            CreditsHero.Subscribers.Dtos.GetSubscribersRequestDetailInput inputRequest = new CreditsHero.Subscribers.Dtos.GetSubscribersRequestDetailInput()
            {
                SubscribersId = Int32.Parse(user.Id.ToString()),
                CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:APIKey"]),
                SubscribersEmail = user.EmailAddress,
                SubscribersName = user.UserName,
                RequestId = requestId != null ? Int32.Parse(requestId) : 0
            };

            var creditsHeroSubscriber = _memberAppService.GetMember(input);
            input.SubscribersId = creditsHeroSubscriber.Id;
            inputRequest.SubscribersId = creditsHeroSubscriber.Id;

            if (Request.Form.Keys[0] != null && Request.Form.Keys[0] == "PassRequestId")
            {
                _memberAppService.UpdateSubscriberRequestState(
                    new CreateSubscriberRequestStateInput()
                    {
                        CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:APIKey"]),
                        RequestId = Int32.Parse(requestId),
                        SubscribersId = input.SubscribersId,
                        Status = "Pass"
                    });
                return Redirect(Url.Action("Index", "Home"));
            }
            else
            {
                List<SubscribersRequestDetailDto> subscriberRequestDetails = _memberAppService.GetMemberRequestDetails(inputRequest).SubscriberRequestDetailsList;
                results.SubscriberRequestDetails = new List<SubscribersRequestDetailDto>();

                results.SubscriberRequestDetails = subscriberRequestDetails;
                results.RequestId = Int32.Parse(requestId);
                results.SubscriberId = creditsHeroSubscriber.Id;
                return View(results);
            }
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

        public async Task<ActionResult> UpdateCriteriaValue()
        {
            int criteriaValueId = int.Parse(Request.Form["Command"].Split('_')[1]);

            //Need to update CriteriaRefId
            var criteriaResults = _memberAppService.UpdateCriteriaValue(new CreditsHero.Common.Dtos.CreateCriteriaValuesInput()
            {
                CreditCount = Int32.Parse(Request.Form["txtAdminCredits_" + criteriaValueId].ToString()),
                Name = Request.Form["txtAdminCriteriaName_" + criteriaValueId].ToString(),
                CriteriaValuesId = criteriaValueId,
                CriteriaRefId = Int32.Parse(Request.Form["txtAdminCriteriaId_" + criteriaValueId].ToString())
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