using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;
using CreditsHero.Subscribers.Dtos;
using rooferlocator.com.Common;
using System.Collections.Generic;
using CreditsHero.Messaging.Dtos;

namespace rooferlocator.com.Web.Models.Member
{
    public class MemberQuotesViewModel : IInputDto
    {
        public int RequestId { get; set; }
        public int SubscriberId { get; set; }
        public List<CreditsHero.Messaging.Dtos.SubscriberQuoteDto> SubscriberQuotes { get; set; }
        
        public MemberQuotesViewModel()
        {
            SubscriberQuotes = new List<SubscriberQuoteDto>();
        }
    }
}