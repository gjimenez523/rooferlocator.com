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
using CreditsHero.Subscribers.Dtos.PaymentGatewayDtos;
using rooferlocator.com.Common.Members.Dtos;

namespace rooferlocator.com.Common.Members
{
    public class MemberAppService : ApplicationService, IMemberAppService
    {
        //These members set in constructor using constructor injection.    
        private readonly IMemberRepository _memberRepository;
        private readonly ICreditsHeroConnect _creditsHeroConnect;

        /// <summary>
        ///In constructor, we can get needed classes/interfaces.
        ///They are sent here by dependency injection system automatically.
        /// </summary>
        public MemberAppService(IMemberRepository memberRepository,
            ICreditsHeroConnect creditsHeroConnect)
        {
            _memberRepository = memberRepository;
            _creditsHeroConnect = creditsHeroConnect;
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
                if (input.CompanyId.HasValue)
                {
                    CreditsHero.Subscribers.Dtos.GetSubscribersInput inputSubscriber = new GetSubscribersInput() { CompanyId = input.CompanyId };

                    CreditsHero.Subscribers.Dtos.GetSubscribersOutput results = new GetSubscribersOutput();
                    var membersCreditsHero = (GetSubscribersOutput)_creditsHeroConnect.CallCreditsHeroService<GetSubscribersOutput>(results, inputSubscriber,
                        "api/services/app/Subscriber/GetSubscribers");

                    var membersLocal = _memberRepository.GetMembersWithCompany();

                    Dtos.GetMembersOutput members = new Dtos.GetMembersOutput();
                    members.Members = new List<Dtos.MemberDto>();

                    foreach (var item in membersCreditsHero.Subscribers)
                    {
                        SubscribersDto subscriberExt = new SubscribersDto()
                        {
                            Email = item.Email,
                            FullName = item.FullName,
                            Id = item.Id,
                            SmsNumber = item.SmsNumber,
                            SubscriberId = item.SubscriberId,
                            TotalCredits = item.TotalCredits,
                            TotalSpend = item.TotalSpend
                        };
                        var member = membersLocal.Find(c => c.Email == item.Email);
                        if (member != null)
                        {
                            Dtos.MemberDto subscriber = new Dtos.MemberDto()
                            {
                                SubscriberExt = subscriberExt,
                                FullName = member != null ? member.FullName : "",
                                Id = member != null ? member.Id : 0,
                                CellPhone = member != null ? member.CellPhone : "",
                                Company = member.Company != null ? Mapper.Map<Companies.Dtos.CompanyDto>(member.Company) : new Companies.Dtos.CompanyDto(),
                                CompanyRefId = member != null ? member.CompanyRefId : 0,
                                Email = member != null ? member.Email : "",
                                Fax = member != null ? member.Fax : "",
                                JobTitle = member != null ? member.JobTitle : "",
                                Phone = member != null ? member.Phone : "",
                                UserRefId = member != null ? member.UserRefId : 0
                            };
                            members.Members.Add(subscriber);
                        }
                    }
                    return members;
                }
                else {
                    var members = _memberRepository.GetMembersWithCompany();

                    return new Dtos.GetMembersOutput
                    {
                        Members = Mapper.Map<List<Dtos.MemberDto>>(members)
                    };
                }
            }
        }

        public CreditsHero.Common.Dtos.GetCriteriaOutput GetCriteria(GetSubscribersInput input)
        {
            CreditsHero.Common.Dtos.GetCriteriaOutput results = new CreditsHero.Common.Dtos.GetCriteriaOutput();
            return (CreditsHero.Common.Dtos.GetCriteriaOutput)_creditsHeroConnect.CallCreditsHeroService<CreditsHero.Common.Dtos.GetCriteriaOutput>(results, input,
                "api/services/app/Criteria/GetCriteria");
        }

        public CreditsHero.Common.Dtos.GetCriteriaValuesOutput GetCriteriaValues(CreditsHero.Common.Dtos.GetCriteriaInput input)
        {
            CreditsHero.Common.Dtos.GetCriteriaValuesOutput results = new CreditsHero.Common.Dtos.GetCriteriaValuesOutput();
            return (CreditsHero.Common.Dtos.GetCriteriaValuesOutput)_creditsHeroConnect.CallCreditsHeroService<CreditsHero.Common.Dtos.GetCriteriaValuesOutput>(results, input,
                "api/services/app/Criteria/GetCriteriaValues");
        }

        public SubscribersSkillsDto GetMemberSubscriptions(GetSubscribersInput input)
        {
            CreditsHero.Subscribers.Dtos.SubscribersSkillsDto results = new SubscribersSkillsDto();
            return (CreditsHero.Subscribers.Dtos.SubscribersSkillsDto)_creditsHeroConnect.CallCreditsHeroService<SubscribersSkillsDto>(results, input,
                "api/services/app/Subscriber/GetSubscribersSkills");
        }

        public CreditsHero.Subscribers.Dtos.SubscribersDto GetMember(GetSubscribersInput input)
        {
            CreditsHero.Subscribers.Dtos.SubscribersDto results = new CreditsHero.Subscribers.Dtos.SubscribersDto();
            return (CreditsHero.Subscribers.Dtos.SubscribersDto)_creditsHeroConnect.CallCreditsHeroService<SubscribersDto>(results, input,
                "api/services/app/Subscriber/GetSubscriber");
        }

        public GetMemberVisitsOutput GetMemberVisits()
        {
            var resultVisits = _memberRepository.GetMemberVisits();

            return new Dtos.GetMemberVisitsOutput
            {
                MemberVisits = Mapper.Map<List<Dtos.MemberVisitDto>>(resultVisits)
            };
        }

        public CreditsHero.Subscribers.Dtos.SubscribersInquiriesDto GetMemberInquiries(GetSubscribersInput input)
        {
            CreditsHero.Subscribers.Dtos.SubscribersInquiriesDto results = new CreditsHero.Subscribers.Dtos.SubscribersInquiriesDto();
            return (CreditsHero.Subscribers.Dtos.SubscribersInquiriesDto)_creditsHeroConnect.CallCreditsHeroService<SubscribersInquiriesDto>(results, input,
                "api/services/app/Subscriber/GetSubscribersInquiries");
        }

        public CreditsHero.Subscribers.Dtos.SubscribersRequestsDto GetMemberRequests(GetSubscribersInput input)
        {
            CreditsHero.Subscribers.Dtos.SubscribersRequestsDto results = new CreditsHero.Subscribers.Dtos.SubscribersRequestsDto();
            return (CreditsHero.Subscribers.Dtos.SubscribersRequestsDto)_creditsHeroConnect.CallCreditsHeroService<SubscribersRequestsDto>(results, input,
                "api/services/app/Subscriber/GetSubscribersRequests");
        }

        public CreditsHero.Subscribers.Dtos.SubscribersRequestDetailsDto GetMemberRequestDetails(GetSubscribersRequestDetailInput input)
        {
            CreditsHero.Subscribers.Dtos.SubscribersRequestDetailsDto results = new CreditsHero.Subscribers.Dtos.SubscribersRequestDetailsDto();
            return (CreditsHero.Subscribers.Dtos.SubscribersRequestDetailsDto)_creditsHeroConnect.CallCreditsHeroService<SubscribersRequestDetailsDto>(results, input,
                "api/services/app/Subscriber/GetSubscribersRequestDetails");
        }

        public CreditsHero.Messaging.Dtos.SubscriberQuotesDto GetMemberQuotes(GetSubscribersInput input)
        {
            GetQuotesInput inputQuote = new GetQuotesInput()
            {
                CompanyId = input.CompanyId.Value,
                SubscriberRefId = input.SubscribersId.Value,
                QuoteStatus = ""
            };
            CreditsHero.Messaging.Dtos.SubscriberQuotesDto results = new CreditsHero.Messaging.Dtos.SubscriberQuotesDto();
            return (CreditsHero.Messaging.Dtos.SubscriberQuotesDto)_creditsHeroConnect.CallCreditsHeroService<SubscriberQuotesDto>(results, inputQuote,
                "api/services/app/Quotes/GetSubscriberQuotesByStatus");
        }

        public GetInquiryResults MakeInquiry(GetInquiryInput input)
        {
            GetInquiryResults results = new GetInquiryResults();
            return (GetInquiryResults)_creditsHeroConnect.CallCreditsHeroService<GetInquiryResults>(results, input,
                "api/services/app/Inquiry/MakeInquiry");
        }

        public GetRequestsExtOutput CreateRequestExt(CreateRequestsExtInput input)
        {
            GetRequestsExtOutput results = new GetRequestsExtOutput();
            return (GetRequestsExtOutput)_creditsHeroConnect.CallCreditsHeroService<GetRequestsExtOutput>(results, input,
                "api/services/app/Requests/CreateRequestsExt");
        }

        public RequestsDto CreateRequest(CreateRequestsInput input)
        {
            RequestsDto results = new RequestsDto();
            return (RequestsDto)_creditsHeroConnect.CallCreditsHeroService<RequestsDto>(results, input,
                "api/services/app/Requests/CreateRequests");
        }

        public SubscribersCreditsDto GetMemberCredits(GetSubscribersInput input)
        {
            SubscribersCreditsDto results = new SubscribersCreditsDto();
            return (SubscribersCreditsDto)_creditsHeroConnect.CallCreditsHeroService<SubscribersCreditsDto>(results, input,
                "api/services/app/Subscriber/GetSubscriberCredits");
        }

        public PaymentResponseDto MakePayment(PaymentAuthorizeNetDto input)
        {
            PaymentResponseDto results = new PaymentResponseDto();
            return (PaymentResponseDto)_creditsHeroConnect.CallCreditsHeroService<PaymentResponseDto>(results, input,
                "api/services/app/Subscriber/MakeAuthorizationNetPurchase");
        }

        public PaymentResponseDto MakePayment(PaymentPaypalDto input)
        {
            PaymentResponseDto results = new PaymentResponseDto();
            return (PaymentResponseDto)_creditsHeroConnect.CallCreditsHeroService<PaymentResponseDto>(results, input,
                "api/services/app/Susbcribers/MakePaypalPurchase");
        }
        public PaymentResponseDto MakePayment(PaymentStripeDto input)
        {
            PaymentResponseDto results = new PaymentResponseDto();
            return (PaymentResponseDto)_creditsHeroConnect.CallCreditsHeroService<PaymentResponseDto>(results, input,
                "api/services/app/Subscriber/MakeStripePurchase");
        }

        public GetQuotesResults SendQuote(GetQuotesInput input)
        {
            CreditsHero.Messaging.Dtos.GetQuotesResults results = new CreditsHero.Messaging.Dtos.GetQuotesResults();
            return (CreditsHero.Messaging.Dtos.GetQuotesResults)_creditsHeroConnect.CallCreditsHeroService<GetQuotesResults>(results, input,
                "api/services/app/Quotes/SendQuote");
        }

        public GetQuotesResults UpdateQuote(GetQuotesInput input)
        {
            CreditsHero.Messaging.Dtos.GetQuotesResults results = new CreditsHero.Messaging.Dtos.GetQuotesResults();
            return (CreditsHero.Messaging.Dtos.GetQuotesResults)_creditsHeroConnect.CallCreditsHeroService<GetQuotesResults>(results, input,
                "api/services/app/Quotes/UpdateSubscriberQuote");
        }

        public NotificationResults SendEmail(NotificationInput input)
        {
            CreditsHero.Messaging.Dtos.NotificationResults results = new CreditsHero.Messaging.Dtos.NotificationResults();
            return (CreditsHero.Messaging.Dtos.NotificationResults)_creditsHeroConnect.CallCreditsHeroService<CreditsHero.Messaging.Dtos.NotificationResults>(results, input,
                "api/services/app/Notification/SendEmailNotification");
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

        public SubscribersValuesDto AddSubscribersValue(CreateSubscribersValuesInput input)
        {
            SubscribersValuesDto results = new SubscribersValuesDto();
            return (SubscribersValuesDto)_creditsHeroConnect.CallCreditsHeroService<SubscribersValuesDto>(results, input,
                "api/services/app/Subscriber/AddSubscribersValue");
        }

        public CriteriaValuesDto AddCriteriaValue(CreateCriteriaValuesInput input)
        {
            CriteriaValuesDto results = new CriteriaValuesDto();
            return (CriteriaValuesDto)_creditsHeroConnect.CallCreditsHeroService<CriteriaValuesDto>(results, input,
                "api/services/app/Criteria/AddCriteriaValue");
        }

        public CriteriaValuesDto UpdateCriteriaValue(CreateCriteriaValuesInput input)
        {
            CriteriaValuesDto results = new CriteriaValuesDto();
            return (CriteriaValuesDto)_creditsHeroConnect.CallCreditsHeroService<CriteriaValuesDto>(results, input,
                "api/services/app/Criteria/UpdateCriteriaValue");
        }

        public CriteriaDto AddCriteria(CreateCriteriaInput input)
        {
            CriteriaDto results = new CriteriaDto();
            return (CriteriaDto)_creditsHeroConnect.CallCreditsHeroService<CriteriaDto>(results, input,
                "api/services/app/Criteria/CreateCriteria");
        }

        public void UpdateSubscriberRequestState(CreateSubscriberRequestStateInput input)
        {
            object results = new object();
            _creditsHeroConnect.CallCreditsHeroService<object>(results, input,
                "api/services/app/Subscriber/UpdateSubscriberRequestState");
        }
    }
}
