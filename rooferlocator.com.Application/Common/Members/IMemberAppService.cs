using Abp.Application.Services;
using rooferlocator.com.Common.Members.Dtos;
using CreditsHero.Subscribers.Dtos;
using System.Collections.Generic;
using CreditsHero.Messaging.Dtos;
using CreditsHero.Subscribers.Dtos.PaymentGatewayDtos;
using CreditsHero.Common.Dtos;

namespace rooferlocator.com.Common.Members
{
    public interface IMemberAppService : IApplicationService
    {
        GetMembersOutput GetMembers(GetMemberInput input);
        GetCriteriaOutput GetCriteria(GetSubscribersInput input);
        GetCriteriaValuesOutput GetCriteriaValues(GetCriteriaInput input);
        CreditsHero.Subscribers.Dtos.SubscribersDto GetMember(GetSubscribersInput input);
        SubscribersSkillsDto GetMemberSubscriptions(GetSubscribersInput input);
        SubscribersInquiriesDto GetMemberInquiries(GetSubscribersInput input);
        SubscribersRequestsDto GetMemberRequests(GetSubscribersInput input);
        SubscribersRequestDetailsDto GetMemberRequestDetails(GetSubscribersRequestDetailInput input);
        SubscriberQuotesDto GetMemberQuotes(GetSubscribersInput input);
        SubscribersCreditsDto GetMemberCredits(GetSubscribersInput input);
        GetQuotesResults SendQuote(GetQuotesInput input);
        NotificationResults SendEmail(NotificationInput input);
        GetQuotesResults UpdateQuote(GetQuotesInput input);
        PaymentResponseDto MakePayment(PaymentAuthorizeNetDto input);
        PaymentResponseDto MakePayment(PaymentPaypalDto input);
        void UpdateMember(UpdateMemberInput input);
        void CreateMember(CreateMemberInput input);
        SubscribersValuesDto AddSubscribersValue(CreateSubscribersValuesInput input);
        CriteriaValuesDto AddCriteriaValue(CreateCriteriaValuesInput input);
        CriteriaDto AddCriteria(CreateCriteriaInput input);
    }
}
