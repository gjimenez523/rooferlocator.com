using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using Abp.UI;
using rooferlocator.com.Web.Models.Account;
using rooferlocator.com.Common;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Abp.Web.Mvc.Models;
using rooferlocator.com.Common.Types;
using System;
using System.Linq;
using rooferlocator.com.Web.Controllers;
using System.Web.Configuration;

namespace rooferlocator.com.Web.Controllers
{
    
    public class RegisterController : comControllerBase
    {
        private readonly IMemberRepository _memberManager;
        private readonly ICompanyRepository _companyManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRoofTypeAppService _roofTypeService;
        private readonly IServiceTypeAppService _serviceTypeService;
        
        public RegisterController(
            IMemberRepository memberManager,
            ICompanyRepository companyManager,
            IUnitOfWorkManager unitOfWorkManager,
            IRoofTypeAppService roofTypeService,
            IServiceTypeAppService serviceTypeService)
        {
            _memberManager = memberManager;
            _companyManager = companyManager;
            _unitOfWorkManager = unitOfWorkManager;
            _roofTypeService = roofTypeService;
            _serviceTypeService = serviceTypeService;
        }

        public ActionResult Index()
        {
            //return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.

            return View("RegisterMember");
        }

        [HttpPost]
        [UnitOfWork]
        public virtual async Task<ActionResult> RegisterMember(RegisterMemberViewModel model)
        {
            try
            {
                //Create Company
                var company = new Company
                {
                    Address1 = model.Company.Address1,
                    Address2 = model.Company.Address2,
                    City = model.Company.City,
                    Name = model.Company.Name,
                    ServiceArea = model.Company.ServiceArea,
                    State = model.Company.State,
                    YearEstablished = model.Company.YearEstablished,
                    Zip = model.Company.Zip
                };

                int companyId = await _companyManager.InsertAndGetIdAsync(company);

                //Create Member
                var member = new Member
                {
                    CellPhone = model.CellPhone,
                    CompanyRefId = companyId,
                    Credential = model.Credential,
                    Email = model.Email,
                    Fax = model.Fax,
                    FullName = model.FullName,
                    JobTitle = model.JobTitle,
                    LicenseDescription = model.LicenseDescription,
                    LicenseNumber = model.LicenseNumber,
                    UserRefId = model.UserRefId
                };

                int memberId = await _memberManager.InsertAndGetIdAsync(member);


                //Call CreditsHero Subscriber
                var subscriberResponse = await RegisterCreditsHero(member);

                //Call CreditsHero Get Criteria List for RooferLocator
                var roofTypes = _roofTypeService.GetRoofTypes(new CreditsHero.Common.Dtos.GetCriteriaInput()
                {
                    CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:CompanyId"]),
                    CriteriaId = 0
                });

                //Call CreditsHero Get Criteria List for RooferLocator
                var serviceTypes = _serviceTypeService.GetServiceTypes(new CreditsHero.Common.Dtos.GetCriteriaInput()
                {
                    CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:CompanyId"]),
                    CriteriaId = 0
                });
                
                //CreditsHero.Common.Dtos.GetCriteriaValuesOutput criteriaValues = new CreditsHero.Common.Dtos.GetCriteriaValuesOutput();
                var roofTypeCriteriaValues = roofTypes.RoofTypes.Cast<CreditsHero.Common.Dtos.CriteriaValuesDto>().ToList();
                var serviceTypeCriteriaValues = serviceTypes.ServiceTypes.Cast<CreditsHero.Common.Dtos.CriteriaValuesDto>().ToList();

                RegisterCreditsHeroViewModel subscriberCriteriaModel = new RegisterCreditsHeroViewModel()
                {
                    RoofTypeCriteria = new CreditsHero.Common.Dtos.GetCriteriaValuesOutput() { CriteriaValues = roofTypeCriteriaValues },
                    ServiceTypeCriteria = new CreditsHero.Common.Dtos.GetCriteriaValuesOutput() { CriteriaValues = serviceTypeCriteriaValues },
                    SubscriberEmail = subscriberResponse.Email,
                    SubscriberName = subscriberResponse.FullName,
                    SubscriberSms = subscriberResponse.SmsNumber,
                    SuscriberId = subscriberResponse.Id
                };

                //Send Email Notification to Administrator
                var serviceUrl = String.Format("{0}api/services/app/Notification/SendEmailNotification", WebConfigurationManager.AppSettings["creditsHero:WebServiceApiPrefix"]);
                //var serviceUrl = string.Format("http://CreditsHero.azurewebsites.net/api/services/cd/Notification/SendEmailNotification");
                CreditsHero.Messaging.Dtos.NotificationInput emailInput = new CreditsHero.Messaging.Dtos.NotificationInput();

                //Serialize object to JSON
                MemoryStream jsonStream2 = new MemoryStream();

                emailInput = new CreditsHero.Messaging.Dtos.NotificationInput()
                {
                    CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:APIKey"]),
                    EmailFrom = "no-reply@rooferlocator.com",
                    EmailSubject = String.Format("New Subscriber at RooferLocator.com"),
                    EmailMessage = String.Format("Hello Administrator: \n  A subscriber has been submitted at RooferLocator.com.\n\n Following are the Subscribers details:\n\n User Details \n\n FullName = {0} \n Email = {1} \n SMS Number (Cell Phone) = {2} \n\n  Company Details \n\n Company Name = {3} \n State = {4} \n City = {5}",
                        member.FullName,
                        member.Email,
                        member.CellPhone,
                        company.Name,
                        company.State,
                        company.City),
                    EmailTo = System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:AdminEmailAddress"]
                };

                string jsonData2 = Newtonsoft.Json.JsonConvert.SerializeObject(emailInput);
                byte[] byteArray2 = Encoding.UTF8.GetBytes(jsonData2);

                HttpWebRequest serviceUrlRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
                serviceUrlRequest.ContentType = "application/json;charset=utf-8";
                serviceUrlRequest.ContentLength = byteArray2.Length;
                serviceUrlRequest.Method = "POST";
                Stream newStream2 = serviceUrlRequest.GetRequestStream();
                newStream2.Write(byteArray2, 0, byteArray2.Length);
                newStream2.Close();
                WebResponse timeLineResponse2 = serviceUrlRequest.GetResponse();
                using (timeLineResponse2)
                {
                    using (var reader = new StreamReader(timeLineResponse2.GetResponseStream()))
                    {
                        var results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());

                        Newtonsoft.Json.Linq.JObject jObject2 = results.result;
                        var itemResult = Newtonsoft.Json.JsonConvert.DeserializeObject<CreditsHero.Messaging.Requests.Dtos.GetInquiryResults>(jObject2.ToString());
                    }
                }

                return View("RegisterCreditsHero", subscriberCriteriaModel);
                
                //return Redirect(Url.Action("Index", "Home"));
            }
            catch (UserFriendlyException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Register", model);
            }
        }

        [HttpPost]
        public virtual async Task<CreditsHero.Subscribers.Dtos.SubscribersDto> RegisterCreditsHero(Member subscriber)
        {
            CreditsHero.Subscribers.Dtos.SubscribersDto subscriberResponse = new CreditsHero.Subscribers.Dtos.SubscribersDto();

            try
            {
                var creditsHeroFormat = String.Format("{0}api/services/app/Subscriber/CreateSubscribers", WebConfigurationManager.AppSettings["creditsHero:WebServiceApiPrefix"]);
                //var creditsHeroFormat = "http://creditshero.azurewebsites.net/api/services/cd/Subscriber/CreateSubscribers";
                //var creditsHeroFormat = "http://localhost:6234/api/services/cd/Subscriber/CreateSubscribers"; //DEBUG
                var timelineUrl = string.Format(creditsHeroFormat);
                
                //Serialize object to JSON
                MemoryStream jsonStream = new MemoryStream();
                CreditsHero.Subscribers.Dtos.CreateSubscribersInput subscriberInput = new CreditsHero.Subscribers.Dtos.CreateSubscribersInput()
                {
                    Email = subscriber.Email,
                    FullName = subscriber.FullName,
                    SmsNumber = subscriber.CellPhone,
                    SubscribersId = 0
                };
                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(subscriberInput);
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);
                    //string.Format("{{subscribersId: null; fullName: '{0}', smsNumber: '{1}', email: '{2}'}}", subscriber.FullName, subscriber.CellPhone, subscriber.Email));

                HttpWebRequest creditsHeroRequest = (HttpWebRequest)WebRequest.Create(timelineUrl);
                creditsHeroRequest.ContentType = "application/json;charset=utf-8";
                creditsHeroRequest.ContentLength = byteArray.Length;
                creditsHeroRequest.Method = "POST";
                Stream newStream = creditsHeroRequest.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();
                WebResponse timeLineResponse = creditsHeroRequest.GetResponse();
                var timeLineJson = string.Empty;
                using (timeLineResponse)
                {
                    using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                    {
                        var results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());
                        subscriberResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CreditsHero.Subscribers.Dtos.SubscribersDto>(results.result.ToString());
                    }
                }

                return subscriberResponse;
            }
            catch (UserFriendlyException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return subscriberResponse;
            }
        }

        public virtual async Task<rooferlocator.com.Common.Types.Dtos.GetRoofTypeOutput> GetCreditsHeroValues()
        {
            try
            {
                //TODO:  Remove Hardcoded company for RooferLocator
                var roofTypes = _roofTypeService.GetRoofTypes(new CreditsHero.Common.Dtos.GetCriteriaInput()
                {
                    CompanyId = Guid.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["creditsHero:CompanyId"]),
                    CriteriaId = 0
                });

                return roofTypes;
                //return Redirect(Url.Action("Index", "Home"));
            }
            catch (UserFriendlyException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return null;
            }
        }

        [HttpPost]
        public virtual async Task<ActionResult> RegisterCreditsHeroValues(CreditsHero.Common.Dtos.GetCriteriaInput input)
        {
            try
            {
                var creditsHeroFormat = String.Format("{0}api/services/app/Criteria/GetCriteriaValues", WebConfigurationManager.AppSettings["creditsHero:WebServiceApiPrefix"]);
                //var creditsHeroFormat = "http://creditshero.azurewebsites.net/api/services/cd/Criteria/GetCriteriaValues";
                var timelineUrl = string.Format(creditsHeroFormat);

                //Serialize object to JSON
                MemoryStream jsonStream = new MemoryStream();
                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(input);
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);

                HttpWebRequest creditsHeroRequest = (HttpWebRequest)WebRequest.Create(timelineUrl);
                creditsHeroRequest.ContentType = "application/json;charset=utf-8";
                creditsHeroRequest.ContentLength = byteArray.Length;
                creditsHeroRequest.Method = "POST";
                Stream newStream = creditsHeroRequest.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();
                WebResponse timeLineResponse = creditsHeroRequest.GetResponse();
                var timeLineJson = string.Empty;
                using (timeLineResponse)
                {
                    using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                    {
                        timeLineJson = reader.ReadToEnd();
                    }
                }

                //return View("Register", subscriber);
                return Redirect(Url.Action("Index", "Home"));
            }
            catch (UserFriendlyException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Register", input);
            }
        }

        [HttpPost]
        public virtual async Task<ActionResult> SubscribeCreditsHeroValues(
            List<CreditsHero.Subscribers.Dtos.CreateSubscribersValuesInput> input)
        {
            try
            {
                var creditsHeroFormat = String.Format("{0}api/services/app/Subscriber/AddSubscribersValue", WebConfigurationManager.AppSettings["creditsHero:WebServiceApiPrefix"]);
                //var creditsHeroFormat = "http://creditshero.azurewebsites.net/api/services/cd/Subscriber/AddSubscribersValue";
                //var creditsHeroFormat = "http://localhost:6234/api/services/cd/Subscriber/AddSubscribersValue";
                var timelineUrl = string.Format(creditsHeroFormat);

                foreach (CreditsHero.Subscribers.Dtos.CreateSubscribersValuesInput item in input)
                {
                    //Serialize object to JSON
                    MemoryStream jsonStream = new MemoryStream();
                    string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                    byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);

                    HttpWebRequest creditsHeroRequest = (HttpWebRequest)WebRequest.Create(timelineUrl);
                    creditsHeroRequest.ContentType = "application/json;charset=utf-8";
                    creditsHeroRequest.ContentLength = byteArray.Length;
                    creditsHeroRequest.Method = "POST";
                    Stream newStream = creditsHeroRequest.GetRequestStream();
                    newStream.Write(byteArray, 0, byteArray.Length);
                    newStream.Close();
                    WebResponse timeLineResponse = creditsHeroRequest.GetResponse();
                    var timeLineJson = string.Empty;
                    using (timeLineResponse)
                    {
                        using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                        {
                            timeLineJson = reader.ReadToEnd();
                        }
                    }
                }

                return Json(new MvcAjaxResponse { TargetUrl = Request.ApplicationPath });
            }
            catch (UserFriendlyException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return Json(new MvcAjaxResponse { TargetUrl = Request.ApplicationPath });
            }
        }

        public ActionResult RegisterCreditsHeroValues()
        {
            return View("RegisterCreditsHero");
        }

        public ActionResult RegisterCreditsHero()
        {
            return View("RegisterCreditsHero");
        }
	}
}