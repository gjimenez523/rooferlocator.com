using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;
using CreditsHero.Subscribers.Dtos;
using rooferlocator.com.Common;
using System.Collections.Generic;

namespace rooferlocator.com.Web.Models.Member
{
    public class MemberRequestViewModel : IInputDto
    {
        /// <summary>
        /// Not required for single-tenant applications.
        /// </summary>
        public int SubscriberInquiriesCount { get; set; }
        public int CreditsRemainingCount { get; set; }
        public CreditsHero.Subscribers.Dtos.SubscribersInquiriesDto SubscriberInquiries { get; set; }
        public List<CreditsHero.Subscribers.Dtos.SubscribersRequestDto> SubscriberRequests { get; set; }
        
        public MemberRequestViewModel()
        {
            SubscriberInquiries = new SubscribersInquiriesDto();
            SubscriberRequests = new List<SubscribersRequestDto>();
        }
    }
}