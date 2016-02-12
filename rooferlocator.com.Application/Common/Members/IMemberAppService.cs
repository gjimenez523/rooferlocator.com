using Abp.Application.Services;
using rooferlocator.com.Common.Members.Dtos;
using CreditsHero.Subscribers.Dtos;
using System.Collections.Generic;
using CreditsHero.Messaging.Dtos;

namespace rooferlocator.com.Common.Members
{
    public interface IMemberAppService : IApplicationService
    {
        GetMembersOutput GetMembers(GetMemberInput input);
        CreditsHero.Subscribers.Dtos.SubscribersDto GetMember(GetSubscribersInput input);
        SubscribersSkillsDto GetMemberSubscriptions(GetSubscribersInput input);
        SubscribersInquiriesDto GetMemberInquiries(GetSubscribersInput input);
        SubscribersRequestsDto GetMemberRequests(GetSubscribersInput input);
        SubscribersRequestDetailsDto GetMemberRequestDetails(GetSubscribersRequestDetailInput input);
        SubscriberQuotesDto GetMemberQuotes(GetSubscribersInput input);
        GetQuotesResults SendQuote(GetQuotesInput input);
        void UpdateMember(UpdateMemberInput input);
        void CreateMember(CreateMemberInput input);
    }
}
