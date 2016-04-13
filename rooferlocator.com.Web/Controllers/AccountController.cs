using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Threading;
using Abp.UI;
using Abp.Web.Mvc.Models;
using rooferlocator.com.Authorization.Roles;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;
using rooferlocator.com.Web.Controllers.Results;
using rooferlocator.com.Web.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using rooferlocator.com.Common.Types;
using System.IO;
using System.Text;
using System.Net;
using System.Web.Configuration;

namespace rooferlocator.com.Web.Controllers
{
    public class AccountController : comControllerBase
    {
        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IRoofTypeAppService _roofTypeService;
        private readonly IServiceTypeAppService _serviceTypeService;
        private readonly ILocationAppService _locationService;


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public AccountController(
            TenantManager tenantManager,
            UserManager userManager,
            RoleManager roleManager,
            IUnitOfWorkManager unitOfWorkManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRoofTypeAppService roofTypeAppService,
            IServiceTypeAppService serviceTypeAppService,
            ILocationAppService locationAppService)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWorkManager = unitOfWorkManager;
            _multiTenancyConfig = multiTenancyConfig;

            _roofTypeService = roofTypeAppService;
            _serviceTypeService = serviceTypeAppService;
            _locationService = locationAppService;
        }

        #region RooferLocator
        [HttpGet]
        //public ActionResult GetCities(FormCollection collection, LoginFormViewModel inquiryModel, string returnUrl = "", string state = "")
        public ActionResult GetCities(string state)
        {
            LoginFormViewModel loginFormModel = new LoginFormViewModel()
            {
                InquiryResults = new CreditsHero.Messaging.Requests.Dtos.GetInquiryResults(),
                RequestResults = new CreditsHero.Messaging.Dtos.RequestsDto(),
                ReturnUrl = "/",
                IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled
            };

            List<KeyValuePair<string, string>> inquiryValues = new List<KeyValuePair<string, string>>();

            BuildInquiry(ref inquiryValues);

            SetupLoginModel(ref loginFormModel, new CreditsHero.Messaging.Requests.Dtos.GetInquiryResults(),
                new CreditsHero.Messaging.Dtos.RequestsDto(),
                state);

            return Json(loginFormModel, JsonRequestBehavior.AllowGet);
        }

        public object SendEmail(CreditsHero.Messaging.Requests.Dtos.GetInquiryInput inquiryInput,
            string emailFrom, string emailSubject, string emailTo, string emailBodyOverride)
        {
            MemoryStream jsonStream = new MemoryStream();
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(inquiryInput);
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);

            //Send Inquiry Email to Admin
            var creditsHeroFormat = String.Format("{0}api/services/app/Notification/SendEmailNotification", WebConfigurationManager.AppSettings["creditsHero:WebServiceApiPrefix"]);
            CreditsHero.Messaging.Dtos.NotificationInput emailInput = new CreditsHero.Messaging.Dtos.NotificationInput();
            var timelineUrl = string.Format(creditsHeroFormat);
            CreditsHero.Messaging.Requests.Dtos.GetInquiryResults inquiryResults;
            List<KeyValuePair<string, string>> inquiryValues = new List<KeyValuePair<string, string>>();
            inquiryValues = inquiryInput.QueryRequest;

            if(emailBodyOverride == string.Empty)
            {
                emailBodyOverride = String.Format("<!DOCTYPE html><html lang=en xmlns='http://www.w3.org/1999/xhtml'><head><meta charset=utf-8 /><title></title><link rel='stylesheet' href='/Content/bootstrap-cosmo.min.css' /></head><body style='background-color:gainsboro;text-align:center;padding:0px;margin:0px'><div class='row' style='padding:0px;margin:0px;'> <div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'></div><div class='col-lg-8 col-md-8 col-sm-8 col-xs-8' style='padding:0px;margin:0px'> <div class='row'> <div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'></div><div class='col-lg-8 col-md-8 col-sm-8 col-xs-8' style='box-shadow: 5px 0px 5px -4px rgba(31,73,125,0.8), -5px 0px 8px -5px rgba(31,73,125,0.8); background-image: -ms-linear-gradient(rgb(25, 68, 125) 20%, rgb(32, 128, 190) 100%);'> <img src='/images/rooferlocatorlogo.gif' /> <div class='row' style='padding:0px;margin:0px;height:50px;font-family:Arial;font-size:24pt'> <div class='col-lg-12 col-md-12 col-sm-12 col-xs-12' style='border:2px solid white;border-radius:5px; background:white;'> <div class='row' style='padding-left:13px;padding-right:13px;border-radius:5px;'> <div class='col-lg-12 col-md-12 col-sm-12 col-xs-12' style='background:#fe7900;'>Inquiry</div></div><div class='row' style='color:black'> <div class='col-lg-12 col-md-12 col-sm-12 col-xs-12' style='font-size:16px;'>Hello Administrator: <br/> An inquiry/search has been submitted at RooferLocator.com.<p/> Following are the details of the inquiry:<p/> <strong>Roof Type = {0}</strong> <br/> <strong>Service Type = {1}</strong> <br/> <strong>Time Of Repair = {2}</strong> <br/> <strong>State = {3}</strong> <br/> <strong>City = {4}</strong></div></div><div class='row' style='padding-left:13px;padding-right:13px;border-radius:5px;'> <div class='col-lg-12 col-md-12 col-sm-12 col-xs-12' style='background:#fe7900;'>Comment</div></div><div class='col-lg-12 col-md-12 col-sm-12 col-xs-12' style='border-top:1px solid gainsboro;padding:10px;color:black'> <div>{1}</div></div></div></div></div><div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'></div></div></div><div class='col-lg-2 col-md-2 col-sm-2 col-xs-2' style='padding:0px;margin:0px'></div></div></body></html>",
                    inquiryValues[0].Value,
                    inquiryValues[1].Value,
                    inquiryValues[2].Value,
                    inquiryValues[3].Value,
                    inquiryValues[4].Value);
            }

            emailInput = new CreditsHero.Messaging.Dtos.NotificationInput()
            {
                CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:CompanyId"]),
                EmailFrom = emailFrom,
                EmailSubject = String.Format(emailSubject),
                EmailMessage = emailBodyOverride,
                EmailTo = emailTo
            };

            jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(emailInput);
            byteArray = Encoding.UTF8.GetBytes(jsonData);
            HttpWebRequest creditsHeroRequest = (HttpWebRequest)WebRequest.Create(timelineUrl);

            creditsHeroRequest.ContentType = "application/json;charset=utf-8";
            creditsHeroRequest.ContentLength = byteArray.Length;
            creditsHeroRequest.Method = "POST";
            Stream newStream = creditsHeroRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();
            WebResponse timeLineResponse = creditsHeroRequest.GetResponse();
            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    var results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());

                    Newtonsoft.Json.Linq.JObject jObject2 = results.result;
                    var itemResult = Newtonsoft.Json.JsonConvert.DeserializeObject<CreditsHero.Messaging.Requests.Dtos.GetInquiryResults>(jObject2.ToString());
                    inquiryResults = itemResult;
                    return inquiryResults;
                }
            }
        }

        [HttpPost]
        [DisableAuditing]
        public ActionResult Inquiry(FormCollection collection, LoginFormViewModel inquiryModel, string returnUrl = "")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            LoginFormViewModel loginFormModel = new LoginFormViewModel()
            {
                InquiryResults = new CreditsHero.Messaging.Requests.Dtos.GetInquiryResults(),
                RequestResults = new CreditsHero.Messaging.Dtos.RequestsDto(),
                ReturnUrl = returnUrl,
                IsMultiTenancyEnabled = false
            };

            #region Inquiry
            var creditsHeroFormat = String.Format("{0}api/services/app/Inquiry/MakeInquiry", WebConfigurationManager.AppSettings["creditsHero:WebServiceApiPrefix"]);
            //var creditsHeroFormat = "http://CreditsHero.azurewebsites.net/api/services/cd/Inquiry/MakeInquiry";
            //var creditsHeroFormat = "http://localhost:6234/api/services/cd/Inquiry/MakeInquiry";
            var timelineUrl = string.Format(creditsHeroFormat);
            CreditsHero.Messaging.Requests.Dtos.GetInquiryInput inquiryInput;
            CreditsHero.Messaging.Requests.Dtos.GetInquiryResults inquiryResults;
            List<KeyValuePair<string, string>> inquiryValues = new List<KeyValuePair<string, string>>();

            BuildInquiry(ref inquiryValues);

            //Serialize object to JSON
            MemoryStream jsonStream = new MemoryStream();

            inquiryInput = new CreditsHero.Messaging.Requests.Dtos.GetInquiryInput()
            {
                CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:CompanyId"]),
                QueryRequest = inquiryValues
            };

            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(inquiryInput);
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);
            
            HttpWebRequest creditsHeroRequest = (HttpWebRequest)WebRequest.Create(timelineUrl);
            creditsHeroRequest.ContentType = "application/json;charset=utf-8";
            creditsHeroRequest.ContentLength = byteArray.Length;
            creditsHeroRequest.Method = "POST";
            Stream newStream = creditsHeroRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();
            WebResponse timeLineResponse = creditsHeroRequest.GetResponse();
            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    var results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());

                    Newtonsoft.Json.Linq.JObject jObject2 = results.result;
                    var itemResult = Newtonsoft.Json.JsonConvert.DeserializeObject<CreditsHero.Messaging.Requests.Dtos.GetInquiryResults>(jObject2.ToString());
                    inquiryResults = itemResult;
                }
            }
            #endregion

            SetupLoginModel(ref loginFormModel, inquiryResults, new CreditsHero.Messaging.Dtos.RequestsDto(), string.Empty);// , inquiryValues[3].Key);

            #region Email
            //Send administrator email for Inquiry
            SendEmail(inquiryInput, "no-reply@rooferlocator.com", "New Inquiry at RooferLocator.com",
                System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:AdminEmailAddress"], string.Empty);
            #endregion
            
            return View("CustomerRequest", loginFormModel.InquiryResults);
        }

        [HttpPost]
        [UnitOfWork]
        public virtual async Task<ActionResult> SendRequest(FormCollection collection, LoginFormViewModel inquiryModel,
            string returnUrl = "")
        {
            string generatedPassword = System.Web.Security.Membership.GeneratePassword(8, 2);

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            LoginFormViewModel loginFormModel = new LoginFormViewModel()
            {
                InquiryResults = new CreditsHero.Messaging.Requests.Dtos.GetInquiryResults(),
                RequestResults = new CreditsHero.Messaging.Dtos.RequestsDto(),
                ReturnUrl = returnUrl,
                IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled
            };

            #region Request
            var creditsHeroFormat = String.Format("{0}api/services/app/Requests/CreateRequests", WebConfigurationManager.AppSettings["creditsHero:WebServiceApiPrefix"]);
            //var creditsHeroFormat = "http://CreditsHero.azurewebsites.net/api/services/cd/Requests/CreateRequests";
            //var creditsHeroFormat = "http://localhost:6234/api/services/cd/Requests/CreateRequests";
            var timelineUrl = string.Format(creditsHeroFormat);
            CreditsHero.Messaging.Dtos.CreateRequestsInput requestInput = new CreditsHero.Messaging.Dtos.CreateRequestsInput();
            CreditsHero.Messaging.Dtos.RequestsDto requestResults;
            List<KeyValuePair<string, string>> inquiryValues = new List<KeyValuePair<string, string>>();

            BuildInquiry(ref inquiryValues);

            //Serialize object to JSON
            MemoryStream jsonStream = new MemoryStream();

            requestInput.QueryRequest = inquiryValues;
            requestInput.CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:CompanyId"]);
            requestInput.FullName = String.Format("{0} {1}", Request.Form["FirstName"], Request.Form["LastName"]);
            requestInput.InquiryId = Int32.Parse(Request.Form["InquiryId"]);
            requestInput.SmsNumber = Request.Form["PhoneNumber"];
            requestInput.RequestsId = 0;
            requestInput.Email = Request.Form["EmailAddress"];
            requestInput.ReplyToEmail = System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:EmailReply"];

            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(requestInput);
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);

            HttpWebRequest creditsHeroRequest = (HttpWebRequest)WebRequest.Create(timelineUrl);
            creditsHeroRequest.ContentType = "application/json;charset=utf-8";
            creditsHeroRequest.ContentLength = byteArray.Length;
            creditsHeroRequest.Method = "POST";
            Stream newStream = creditsHeroRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();
            WebResponse timeLineResponse = creditsHeroRequest.GetResponse();
            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    var results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());

                    Newtonsoft.Json.Linq.JObject jObject2 = results.result;
                    if (jObject2!= null)
                    {
                        var itemResult = Newtonsoft.Json.JsonConvert.DeserializeObject<CreditsHero.Messaging.Dtos.RequestsDto>(jObject2.ToString());
                        requestResults = itemResult;
                    }
                    else
                    {
                        requestResults = null;
                    }
                    
                }
            }
            #endregion

            SetupLoginModel(ref loginFormModel, inquiryModel.InquiryResults, requestResults, string.Empty);

            #region User
            try
            {
                User user;

                //See if user exists (based on email address)
                user = _userManager.FindByEmail(requestInput.Email);

                if (user == null)
                {
                    var fullName = requestInput.FullName.Split(' ');
                    //Create user
                    user = new User
                    {
                        TenantId = null,
                        Name = fullName[0],
                        Surname = fullName[fullName.Length-1],
                        EmailAddress = requestInput.Email,
                        IsActive = true,
                        UserName = requestInput.Email,
                        Password = new PasswordHasher().HashPassword(generatedPassword),
                        IsEmailConfirmed = false,
                        IsDeleted = false,
                        CreationTime = DateTime.Now
                    };

                    //Switch to the tenant
                    _unitOfWorkManager.Current.EnableFilter(AbpDataFilters.MayHaveTenant);
                    _unitOfWorkManager.Current.SetFilterParameter(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId, null);

                    //Add default roles
                    user.Roles = new List<UserRole>();
                    foreach (var customerRole in _roleManager.Roles.Where(r => r.Name == "Customer").ToList())
                    {
                        user.Roles.Add(new UserRole { RoleId = customerRole.Id });
                    }

                    //Save user
                    var result = _userManager.Create(user);

                    //Send email to user with password
                    SendEmail(
                        new CreditsHero.Messaging.Requests.Dtos.GetInquiryInput()
                        {
                            CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:CompanyId"]),
                            QueryRequest = inquiryValues
                        },
                        System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:EmailReply"],
                        "Welcome to Roofer Locator",
                        user.EmailAddress,
                        String.Format("<!DOCTYPE html><html lang=en xmlns='http://www.w3.org/1999/xhtml'><head><meta charset=utf-8 /><title></title><link rel='stylesheet' href='/Content/bootstrap-cosmo.min.css' /></head><body style='background-color:gainsboro;text-align:center;padding:0px;margin:0px'><div class='row' style='padding:0px;margin:0px;'> <div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'></div><div class='col-lg-8 col-md-8 col-sm-8 col-xs-8' style='padding:0px;margin:0px'> <div class='row'> <div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'></div><div class='col-lg-8 col-md-8 col-sm-8 col-xs-8' style='box-shadow: 5px 0px 5px -4px rgba(31,73,125,0.8), -5px 0px 8px -5px rgba(31,73,125,0.8); background-image: -ms-linear-gradient(rgb(25, 68, 125) 20%, rgb(32, 128, 190) 100%);'> <img src='{0}/images/rooferlocatorlogo.gif' /> <div class='row' style='padding:0px;margin:0px;height:50px;font-family:Arial;font-size:24pt'> <div class='col-lg-12 col-md-12 col-sm-12 col-xs-12' style='border:2px solid white;border-radius:5px; background:white;'> <div class='row' style='padding-left:13px;padding-right:13px;border-radius:5px;'> <div class='col-lg-12 col-md-12 col-sm-12 col-xs-12' style='background:#fe7900;'>Welcome</div></div><div class='row' style='color:black'> <div class='col-lg-12 col-md-12 col-sm-12 col-xs-12' style='font-size:16px;'>Welcome to RooferLocator.com! <p/><div style='font-size:22px;'> The following are your credentials<p/> Username = {1} <br/> Password = {2}</div></div></div><div class='row' style='padding-left:13px;padding-right:13px;border-radius:5px;'> <div class='col-lg-12 col-md-12 col-sm-12 col-xs-12' style='background:#fe7900;'></div></div><div class='col-lg-12 col-md-12 col-sm-12 col-xs-12' style='border-top:1px solid gainsboro;padding:10px;color:black'> <div></div></div></div></div></div><div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'></div></div></div><div class='col-lg-2 col-md-2 col-sm-2 col-xs-2' style='padding:0px;margin:0px'></div></div></body></html>"
                        , System.Web.Configuration.WebConfigurationManager.AppSettings["dashboardHero:DashboardPrefix"], user.EmailAddress, generatedPassword));

                    AbpUserManager<Tenant, Role, User>.AbpLoginResult loginResult;
                    loginResult = await GetLoginResultAsync(user.UserName, generatedPassword, null);

                    if (loginResult.Result == AbpLoginResultType.Success)
                    {
                        await SignInAsync(loginResult.User, loginResult.Identity);
                        return Redirect(Url.Action("Index", "Customers"));
                    }
                }

                //Directly login if possible
                if (user.IsActive)
                {
                    AbpUserManager<Tenant, Role, User>.AbpLoginResult loginResult;
                    loginResult = await GetLoginResultAsync(user.UserName, user.Password, null);
                    
                    if (loginResult.Result == AbpLoginResultType.Success)
                    {
                        await SignInAsync(loginResult.User, loginResult.Identity);
                        //return Redirect(Url.Action("Index", "Home"));
                        //return Redirect(Url.Action("RegisterMember", "Register"));
                        return Redirect(Url.Action("Index", "Customers"));
                    }

                    Logger.Warn("New registered user could not be login. This should not be normally. login result: " + loginResult.Result);
                }

                //If can not login, show a register result page
                return View("CustomerRequestResult", new CreditsHero.Messaging.Dtos.RequestsDto
                {
                    Email = user.EmailAddress,
                    FullName = user.Name + " " + user.Surname,
                    Id = Int32.Parse(user.Id.ToString())
                });
            }
            catch (UserFriendlyException ex)
            {
                ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
                ViewBag.ErrorMessage = ex.Message;

                //If can not login, show a register result page
                return View("CustomerRequestResult", new CreditsHero.Messaging.Dtos.RequestsDto());
            }
            #endregion
        }

        private void BuildInquiry(ref List<KeyValuePair<string, string>> inquiryValues)
        {
            //Build Inquiry Request - Type Of Roof
            inquiryValues.Add(new KeyValuePair<string, string>("Type Of Roof", Request.Form["RoofTypeValues"]));

            //Build Inquiry Request - Type of Service
            inquiryValues.Add(new KeyValuePair<string, string>("Type Of Service", Request.Form["TypeOfService"]));

            //Build Inquiry Request - Time Of Repair
            inquiryValues.Add(new KeyValuePair<string, string>("Time Of Repair", Request.Form["TimeOfRepairValues"]));

            //Build Inquiry Request - State
            inquiryValues.Add(new KeyValuePair<string, string>("State", Request.Form["StateValues"]));

            //Build Inquiry Request - Cities
            inquiryValues.Add(new KeyValuePair<string, string>("City", Request.Form["City"]));
        }

        private void SetupLoginModel(ref LoginFormViewModel loginFormModel,
            CreditsHero.Messaging.Requests.Dtos.GetInquiryResults inquiryResults,
            CreditsHero.Messaging.Dtos.RequestsDto requestResults,
            string selectedState)
        {
            //Get RoofType
            var roofTypes = _roofTypeService.GetRoofTypes(new CreditsHero.Common.Dtos.GetCriteriaInput()
            {
                CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:CompanyId"]),
                CriteriaId = Int32.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:RoofTypeId"])
            });
            //Get TypeOfService
            var serviceTypes = _serviceTypeService.GetServiceTypes(new CreditsHero.Common.Dtos.GetCriteriaInput()
            {
                CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:CompanyId"]),
                CriteriaId = Int32.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:ServiceTypeId"])
            });
            //TODO: Get TimeOfRepair

            //TODO: Get State

            //TODO: Get City
            var cities = _locationService.GetCities(new CreditsHero.Common.Dtos.GetCriteriaInput()
            {
                CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:CompanyId"]),
                CriteriaId = Int32.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:CityLocationTypeId"])
            }, selectedState);

            loginFormModel.InquiryResults = inquiryResults;
            loginFormModel.RequestResults = requestResults;
            loginFormModel.RoofTypeValues = roofTypes.RoofTypes.Cast<CreditsHero.Common.Dtos.CriteriaValuesDto>().ToList();
            loginFormModel.TypeOfService = serviceTypes.ServiceTypes.Cast<CreditsHero.Common.Dtos.CriteriaValuesDto>().ToList();
            loginFormModel.City = cities.Locations.Cast<CreditsHero.Common.Dtos.CriteriaValuesDto>().ToList();
        }
        #endregion

        #region Login / Logout

        public ActionResult Login(string returnUrl = "")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            //LoginFormViewModel loginFormModel = new LoginFormViewModel()
            //{
            //    InquiryResults = new CreditsHero.Messaging.Requests.Dtos.GetInquiryResults(),
            //    RequestResults = new CreditsHero.Messaging.Dtos.RequestsDto(),
            //    ReturnUrl = returnUrl,
            //    IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled
            //};

            //SetupLoginModel(ref loginFormModel,
            //    new CreditsHero.Messaging.Requests.Dtos.GetInquiryResults(),
            //    new CreditsHero.Messaging.Dtos.RequestsDto(),
            //    String.Empty);

            //return View(loginFormModel);

            return View(
                new LoginFormViewModel
                {
                    ReturnUrl = returnUrl,
                    IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled
                });
        }

        [HttpPost]
        [DisableAuditing]
        public async Task<JsonResult> Login(LoginViewModel loginModel, string returnUrl = "", string returnUrlHash = "")
        {
            CheckModelState();

            var loginResult = await GetLoginResultAsync(
                loginModel.UsernameOrEmailAddress,
                loginModel.Password,
                loginModel.TenancyName
                );

            await SignInAsync(loginResult.User, loginResult.Identity, loginModel.RememberMe);

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            if (!string.IsNullOrWhiteSpace(returnUrlHash))
            {
                returnUrl = returnUrl + returnUrlHash;
            }

            return Json(new MvcAjaxResponse { TargetUrl = returnUrl });
        }

        private async Task<AbpUserManager<Tenant, Role, User>.AbpLoginResult> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _userManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private async Task SignInAsync(User user, ClaimsIdentity identity = null, bool rememberMe = false)
        {
            if (identity == null)
            {
                identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, identity);
        }

        private Exception CreateExceptionForFailedLoginAttempt(AbpLoginResultType result, string usernameOrEmailAddress, string tenancyName)
        {
            switch (result)
            {
                case AbpLoginResultType.Success:
                    return new ApplicationException("Don't call this method with a success result!");
                case AbpLoginResultType.InvalidUserNameOrEmailAddress:
                case AbpLoginResultType.InvalidPassword:
                    return new UserFriendlyException(L("LoginFailed"), L("InvalidUserNameOrPassword"));
                case AbpLoginResultType.InvalidTenancyName:
                    return new UserFriendlyException(L("LoginFailed"), L("ThereIsNoTenantDefinedWithName{0}", tenancyName));
                case AbpLoginResultType.TenantIsNotActive:
                    return new UserFriendlyException(L("LoginFailed"), L("TenantIsNotActive", tenancyName));
                case AbpLoginResultType.UserIsNotActive:
                    return new UserFriendlyException(L("LoginFailed"), L("UserIsNotActiveAndCanNotLogin", usernameOrEmailAddress));
                case AbpLoginResultType.UserEmailIsNotConfirmed:
                    return new UserFriendlyException(L("LoginFailed"), "Your email address is not confirmed. You can not login"); //TODO: localize message
                default: //Can not fall to default actually. But other result types can be added in the future and we may forget to handle it
                    Logger.Warn("Unhandled login fail reason: " + result);
                    return new UserFriendlyException(L("LoginFailed"));
            }
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        #endregion

        #region Register

        public ActionResult Register()
        {
            return RegisterView(new RegisterViewModel());
        }

        private ActionResult RegisterView(RegisterViewModel model)
        {
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;

            return View("Register", model);
        }

        [HttpPost]
        [UnitOfWork]
        public virtual async Task<ActionResult> Register(RegisterViewModel model)
        {
            try
            {
                CheckModelState();

                //Get tenancy name and tenant
                //if (!_multiTenancyConfig.IsEnabled)
                //{
                //    model.TenancyName = Tenant.DefaultTenantName;
                //}
                //else if (model.TenancyName.IsNullOrEmpty())
                //{
                //    throw new UserFriendlyException(L("TenantNameCanNotBeEmpty"));
                //}

                //var tenant = await GetActiveTenantAsync(model.TenancyName);

                //Create user
                var user = new User
                {
                    TenantId = null,
                    Name = model.Name,
                    Surname = model.Surname,
                    EmailAddress = model.EmailAddress,
                    IsActive = true
                };

                //Get external login info if possible
                ExternalLoginInfo externalLoginInfo = null;
                if (model.IsExternalLogin)
                {
                    externalLoginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
                    if (externalLoginInfo == null)
                    {
                        throw new ApplicationException("Can not external login!");
                    }

                    user.Logins = new List<UserLogin>
                    {
                        new UserLogin
                        {
                            LoginProvider = externalLoginInfo.Login.LoginProvider,
                            ProviderKey = externalLoginInfo.Login.ProviderKey
                        }
                    };

                    model.UserName = model.EmailAddress;
                    model.Password = Users.User.CreateRandomPassword();

                    if (string.Equals(externalLoginInfo.Email, model.EmailAddress, StringComparison.InvariantCultureIgnoreCase))
                    {
                        user.IsEmailConfirmed = true;
                    }
                }
                else
                {
                    //Username and Password are required if not external login
                    if (model.UserName.IsNullOrEmpty() || model.Password.IsNullOrEmpty())
                    {
                        throw new UserFriendlyException(L("FormIsNotValidMessage"));
                    }
                }

                user.UserName = model.UserName;
                user.Password = new PasswordHasher().HashPassword(model.Password);

                //Switch to the tenant
                _unitOfWorkManager.Current.EnableFilter(AbpDataFilters.MayHaveTenant);
                _unitOfWorkManager.Current.SetFilterParameter(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId, null);

                //Add default roles
                user.Roles = new List<UserRole>();
                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    user.Roles.Add(new UserRole { RoleId = defaultRole.Id });
                }

                //Save user
                CheckErrors(await _userManager.CreateAsync(user));
                await _unitOfWorkManager.Current.SaveChangesAsync();

                //Directly login if possible
                if (user.IsActive)
                {
                    AbpUserManager<Tenant, Role, User>.AbpLoginResult loginResult;
                    if (externalLoginInfo != null)
                    {
                        //loginResult = await _userManager.LoginAsync(externalLoginInfo.Login, tenant.TenancyName);
                        loginResult = await _userManager.LoginAsync(externalLoginInfo.Login, null);
                    }
                    else
                    {
                        //loginResult = await GetLoginResultAsync(user.UserName, model.Password, tenant.TenancyName);
                        loginResult = await GetLoginResultAsync(user.UserName, model.Password, null);
                    }

                    if (loginResult.Result == AbpLoginResultType.Success)
                    {
                        await SignInAsync(loginResult.User, loginResult.Identity);
                        //return Redirect(Url.Action("Index", "Home"));
                        //return Redirect(Url.Action("RegisterMember", "Register"));
                        return View("RegisterMember", new RegisterMemberViewModel
                        {
                            Email = model.EmailAddress,
                            FullName = model.Name,
                            UserRefId = user.Id
                        });
                    }

                    Logger.Warn("New registered user could not be login. This should not be normally. login result: " + loginResult.Result);
                }

                //If can not login, show a register result page
                return View("RegisterResult", new RegisterResultViewModel
                {
                    //TenancyName = tenant.TenancyName,
                    TenancyName = null,
                    NameAndSurname = user.Name + " " + user.Surname,
                    UserName = user.UserName,
                    EmailAddress = user.EmailAddress,
                    IsActive = user.IsActive,
                    IsEmailConfirmationRequired = false
                });
            }
            catch (UserFriendlyException ex)
            {
                ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
                ViewBag.ErrorMessage = ex.Message;

                return View("Register", model);
            }
        }

        #endregion

        #region External Login

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(
                provider,
                Url.Action(
                    "ExternalLoginCallback",
                    "Account",
                    new
                    {
                        ReturnUrl = returnUrl
                    })
                );
        }

        [UnitOfWork]
        public virtual async Task<ActionResult> ExternalLoginCallback(string returnUrl, string tenancyName = "")
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            //Try to find tenancy name
            if (tenancyName.IsNullOrEmpty())
            {
                var tenants = await FindPossibleTenantsOfUserAsync(loginInfo.Login);
                switch (tenants.Count)
                {
                    case 0:
                        return await RegisterView(loginInfo);
                    case 1:
                        tenancyName = tenants[0].TenancyName;
                        break;
                    default:
                        return View("TenantSelection", new TenantSelectionViewModel
                        {
                            Action = Url.Action("ExternalLoginCallback", "Account", new { returnUrl }),
                            Tenants = tenants.MapTo<List<TenantSelectionViewModel.TenantInfo>>()
                        });
                }
            }

            var loginResult = await _userManager.LoginAsync(loginInfo.Login, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    await SignInAsync(loginResult.User, loginResult.Identity, false);

                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        returnUrl = Url.Action("Index", "Home");
                    }

                    return Redirect(returnUrl);
                case AbpLoginResultType.UnknownExternalLogin:
                    return await RegisterView(loginInfo, tenancyName);
                default:
                    throw CreateExceptionForFailedLoginAttempt(loginResult.Result, loginInfo.Email ?? loginInfo.DefaultUserName, tenancyName);
            }
        }

        private async Task<ActionResult> RegisterView(ExternalLoginInfo loginInfo, string tenancyName = null)
        {
            var name = loginInfo.DefaultUserName;
            var surname = loginInfo.DefaultUserName;

            var extractedNameAndSurname = TryExtractNameAndSurnameFromClaims(loginInfo.ExternalIdentity.Claims.ToList(), ref name, ref surname);

            var viewModel = new RegisterViewModel
            {
                TenancyName = tenancyName,
                EmailAddress = loginInfo.Email,
                Name = name,
                Surname = surname,
                IsExternalLogin = true
            };

            if (!tenancyName.IsNullOrEmpty() && extractedNameAndSurname)
            {
                return await Register(viewModel);
            }

            return RegisterView(viewModel);
        }

        [UnitOfWork]
        protected virtual async Task<List<Tenant>> FindPossibleTenantsOfUserAsync(UserLoginInfo login)
        {
            List<User> allUsers;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                allUsers = await _userManager.FindAllAsync(login);
            }

            return allUsers
                .Where(u => u.TenantId != null)
                .Select(u => AsyncHelper.RunSync(() => _tenantManager.FindByIdAsync(u.TenantId.Value)))
                .ToList();
        }

        private static bool TryExtractNameAndSurnameFromClaims(List<Claim> claims, ref string name, ref string surname)
        {
            string foundName = null;
            string foundSurname = null;

            var givennameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
            if (givennameClaim != null && !givennameClaim.Value.IsNullOrEmpty())
            {
                foundName = givennameClaim.Value;
            }

            var surnameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname);
            if (surnameClaim != null && !surnameClaim.Value.IsNullOrEmpty())
            {
                foundSurname = surnameClaim.Value;
            }

            if (foundName == null || foundSurname == null)
            {
                var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (nameClaim != null)
                {
                    var nameSurName = nameClaim.Value;
                    if (!nameSurName.IsNullOrEmpty())
                    {
                        var lastSpaceIndex = nameSurName.LastIndexOf(' ');
                        if (lastSpaceIndex < 1 || lastSpaceIndex > (nameSurName.Length - 2))
                        {
                            foundName = foundSurname = nameSurName;
                        }
                        else
                        {
                            foundName = nameSurName.Substring(0, lastSpaceIndex);
                            foundSurname = nameSurName.Substring(lastSpaceIndex);
                        }
                    }
                }
            }

            if (!foundName.IsNullOrEmpty())
            {
                name = foundName;
            }

            if (!foundSurname.IsNullOrEmpty())
            {
                surname = foundSurname;
            }

            return foundName != null && foundSurname != null;
        }

        #endregion

        #region Common private methods

        private async Task<Tenant> GetActiveTenantAsync(string tenancyName)
        {
            var tenant = await _tenantManager.FindByTenancyNameAsync(tenancyName);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("ThereIsNoTenantDefinedWithName{0}", tenancyName));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIsNotActive", tenancyName));
            }

            return tenant;
        }

        #endregion
    }
}