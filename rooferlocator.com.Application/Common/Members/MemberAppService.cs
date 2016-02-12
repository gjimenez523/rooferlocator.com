using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using AutoMapper;
using rooferlocator.com.Common;
using CreditsHero.Subscribers.Dtos;
using CreditsHero.Common.Dtos;
using CreditsHero.Messaging.Dtos;
using CreditsHero.Messaging.Requests.Dtos;
using System.IO;
using System.Net;

namespace rooferlocator.com.Common.Members
{
    public class MemberAppService : ApplicationService, IMemberAppService
    {
        //These members set in constructor using constructor injection.    
        private readonly IMemberRepository _memberRepository;
        
        /// <summary>
        ///In constructor, we can get needed classes/interfaces.
        ///They are sent here by dependency injection system automatically.
        /// </summary>
        public MemberAppService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public Dtos.GetMembersOutput GetMembers(Dtos.GetMemberInput input)
        {
            if (input.MemberId != null)
            {
                var members = _memberRepository.GetMembersWithCompany(input.MemberId.Value);
                
                return new Dtos.GetMembersOutput
                {
                    Members = Mapper.Map<List<Dtos.MemberDto>>(members)
                };
            }
            else
            {
                //Called specific GetAllWithPeople method of task repository.
                var members = _memberRepository.GetMembersWithCompany();
                
                return new Dtos.GetMembersOutput
                {
                    Members = Mapper.Map<List<Dtos.MemberDto>>(members)
                };
            }
        }

        public SubscribersSkillsDto GetMemberSubscriptions(GetSubscribersInput input)
        {
            var creditsHeroFormat = String.Format("{0}api/services/app/Subscriber/GetSubscribersSkills", System.Configuration.ConfigurationSettings.AppSettings["creditsHero:WebServiceApiPrefix"]);
            var timelineUrl = string.Format(creditsHeroFormat);
            CreditsHero.Subscribers.Dtos.SubscribersSkillsDto subscriberSkillsResults;
            List<KeyValuePair<string, string>> inquiryValues = new List<KeyValuePair<string, string>>();

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
            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    var results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());

                    Newtonsoft.Json.Linq.JObject jObject2 = results.result;
                    var itemResult = Newtonsoft.Json.JsonConvert.DeserializeObject<SubscribersSkillsDto>(jObject2.ToString());
                    subscriberSkillsResults = itemResult;
                }
            }
            return subscriberSkillsResults;
        }

        public CreditsHero.Subscribers.Dtos.SubscribersDto GetMember(GetSubscribersInput input)
        {
            var creditsHeroFormat = String.Format("{0}api/services/app/Subscriber/GetSubscriber", System.Configuration.ConfigurationSettings.AppSettings["creditsHero:WebServiceApiPrefix"]);
            //var creditsHeroFormat = "http://CreditsHero.azurewebsites.net/api/services/cd/Subscriber/GetSubscriber";
            //var creditsHeroFormat = "http://localhost:6234/api/services/cd/Subscriber/GetSubscriber";
            var timelineUrl = string.Format(creditsHeroFormat);
            CreditsHero.Subscribers.Dtos.SubscribersDto subscriberSkillsResults;

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
            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    var results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());

                    Newtonsoft.Json.Linq.JObject jObject2 = results.result;
                    var itemResult = Newtonsoft.Json.JsonConvert.DeserializeObject<SubscribersDto>(jObject2.ToString());
                    subscriberSkillsResults = itemResult;
                }
            }
            return subscriberSkillsResults;
        }

        public CreditsHero.Subscribers.Dtos.SubscribersInquiriesDto GetMemberInquiries(GetSubscribersInput input)
        {
            var creditsHeroFormat = String.Format("{0}api/services/app/Subscriber/GetSubscribersInquiries", System.Configuration.ConfigurationSettings.AppSettings["creditsHero:WebServiceApiPrefix"]);
            //var creditsHeroFormat = "http://CreditsHero.azurewebsites.net/api/services/cd/Subscriber/GetSubscribersInquiries";
            //var creditsHeroFormat = "http://localhost:6234/api/services/cd/Subscriber/GetSubscribersInquiries";
            var timelineUrl = string.Format(creditsHeroFormat);
            CreditsHero.Subscribers.Dtos.SubscribersInquiriesDto subscriberSkillsResults;

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
            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    var results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());

                    Newtonsoft.Json.Linq.JObject jObject2 = results.result;
                    var itemResult = Newtonsoft.Json.JsonConvert.DeserializeObject<SubscribersInquiriesDto>(jObject2.ToString());
                    subscriberSkillsResults = itemResult;
                }
            }
            return subscriberSkillsResults;
        }

        public CreditsHero.Subscribers.Dtos.SubscribersRequestsDto GetMemberRequests(GetSubscribersInput input)
        {
            var creditsHeroFormat = String.Format("{0}api/services/app/Subscriber/GetSubscribersRequests", System.Configuration.ConfigurationSettings.AppSettings["creditsHero:WebServiceApiPrefix"]);
            //var creditsHeroFormat = "http://CreditsHero.azurewebsites.net/api/services/cd/Subscriber/GetSubscribersRequests";
            //var creditsHeroFormat = "http://localhost:6234/api/services/cd/Subscriber/GetSubscribersRequests";
            var timelineUrl = string.Format(creditsHeroFormat);
            CreditsHero.Subscribers.Dtos.SubscribersRequestsDto subscriberSkillsResults;

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
            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    var results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());

                    Newtonsoft.Json.Linq.JObject jObject2 = results.result;
                    var itemResult = Newtonsoft.Json.JsonConvert.DeserializeObject<SubscribersRequestsDto>(jObject2.ToString());
                    subscriberSkillsResults = itemResult;
                }
            }
            return subscriberSkillsResults;
        }

        public CreditsHero.Subscribers.Dtos.SubscribersRequestDetailsDto GetMemberRequestDetails(GetSubscribersRequestDetailInput input)
        {
            var creditsHeroFormat = String.Format("{0}api/services/app/Subscriber/GetSubscribersRequestDetails", System.Configuration.ConfigurationSettings.AppSettings["creditsHero:WebServiceApiPrefix"]);
            var timelineUrl = string.Format(creditsHeroFormat);
            CreditsHero.Subscribers.Dtos.SubscribersRequestDetailsDto subscriberSkillsResults;

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
            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    var results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());

                    Newtonsoft.Json.Linq.JObject jObject2 = results.result;
                    var itemResult = Newtonsoft.Json.JsonConvert.DeserializeObject<SubscribersRequestDetailsDto>(jObject2.ToString());
                    subscriberSkillsResults = itemResult;
                }
            }
            return subscriberSkillsResults;
        }

        public CreditsHero.Messaging.Dtos.SubscriberQuotesDto GetMemberQuotes(GetSubscribersInput input)
        {
            var creditsHeroFormat = String.Format("{0}api/services/app/Quotes/GetSubscriberQuotes", System.Configuration.ConfigurationSettings.AppSettings["creditsHero:WebServiceApiPrefix"]);
            var timelineUrl = string.Format(creditsHeroFormat);
            CreditsHero.Messaging.Dtos.SubscriberQuotesDto subscriberQuotes;

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
            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    var results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());

                    Newtonsoft.Json.Linq.JObject jObject2 = results.result;
                    var itemResult = Newtonsoft.Json.JsonConvert.DeserializeObject<SubscriberQuotesDto>(jObject2.ToString());
                    subscriberQuotes = itemResult;
                }
            }
            return subscriberQuotes;
        }

        public GetQuotesResults SendQuote(GetQuotesInput input)
        {
            var creditsHeroFormat = String.Format("{0}api/services/app/Quotes/SendQuote", System.Configuration.ConfigurationSettings.AppSettings["creditsHero:WebServiceApiPrefix"]);
            var timelineUrl = string.Format(creditsHeroFormat);
            CreditsHero.Messaging.Dtos.GetQuotesResults quoteResults;

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
            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    var results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());

                    Newtonsoft.Json.Linq.JObject jObject2 = results.result;
                    var itemResult = Newtonsoft.Json.JsonConvert.DeserializeObject<GetQuotesResults>(jObject2.ToString());
                    quoteResults = itemResult;
                }
            }
            return quoteResults;
        }

        public void SendEmail(NotificationInput input)
        {
            var creditsHeroFormat = String.Format("{0}api/services/app/Notification/SendEmailNotification", System.Configuration.ConfigurationSettings.AppSettings["creditsHero:WebServiceApiPrefix"]);
            //var creditsHeroFormat = "http://CreditsHero.azurewebsites.net/api/services/cd/Notification/SendEmailNotification";
            var timelineUrl = string.Format(creditsHeroFormat);
            CreditsHero.Messaging.Dtos.NotificationInput emailInput = new CreditsHero.Messaging.Dtos.NotificationInput();

            //Serialize object to JSON
            MemoryStream jsonStream = new MemoryStream();

            emailInput = new CreditsHero.Messaging.Dtos.NotificationInput()
            {
                EmailFrom = "no-reply@rooferlocator.com",
                EmailSubject = String.Format("Subscriber email at RooferLocator.com"),
                EmailMessage = String.Format("Hello Administrator: \n  A Subscriber has submit this note from RooferLocator.com.\n\n {0}",
                    input.EmailMessage),
                EmailTo = input.EmailTo
            };

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
            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    var results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());

                    Newtonsoft.Json.Linq.JObject jObject2 = results.result;
                    var itemResult = Newtonsoft.Json.JsonConvert.DeserializeObject<SubscribersSkillsDto>(jObject2.ToString());
                }
            }
        }

        public void UpdateMember(Dtos.UpdateMemberInput input)
        {
            //We can use Logger, it's defined in ApplicationService base class.
            
            //Retrieving a task entity with given id using standard Get method of repositories.
            var member = _memberRepository.Get(input.MemberId);

            //Updating changed properties of the retrieved task entity.
            //if (input.FirstName != string.Empty) customer.FirstName = input.FirstName.Value;
            member.CellPhone = input.CellPhone;
            member.CompanyRefId = input.CompanyRefId;
           member.Credential = input.Credential;
            member.Email = input.Email;
            member.Fax = input.Fax;
            member.FullName = input.FullName;
            member.JobTitle = input.JobTitle;
            member.LicenseDescription = input.LicenseDescription;
            member.LicenseNumber = input.LicenseNumber;
            member.UserRefId = input.UserRefId;
            member.Phone = input.Phone;

            //We even do not call Update method of the repository.
            //Because an application service method is a 'unit of work' scope as default.
            //ABP automatically saves all changes when a 'unit of work' scope ends (without any exception).
        }

        public void CreateMember(Dtos.CreateMemberInput input)
        {
            //We can use Logger, it's defined in ApplicationService class.
            //ERROR:  Logger.Info("Creating a task for input: " + input);

            //Creating a new Task entity with given input's properties
            var member = new Member { 
                CellPhone = input.CellPhone,
                CompanyRefId = input.CompanyRefId,
                Credential = input.Credential,
                Email = input.Email,
                Fax = input.Fax,
                FullName = input.FullName,
                JobTitle = input.JobTitle,
                LicenseDescription = input.LicenseDescription,
                LicenseNumber = input.LicenseNumber,
                UserRefId = input.UserRefId,
                Phone = input.Phone
            };

            //Saving entity with standard Insert method of repositories.
            _memberRepository.Insert(member);
        }
    }
}
