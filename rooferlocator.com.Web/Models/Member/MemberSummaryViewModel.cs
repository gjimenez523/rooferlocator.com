using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;
using CreditsHero.Subscribers.Dtos;
using rooferlocator.com.Common;
using System.Collections.Generic;
using System;

namespace rooferlocator.com.Web.Models.Member
{
    public class MemberSummaryViewModel : IInputDto
    {
        /// <summary>
        /// Not required for single-tenant applications.
        /// </summary>
        public int SubscriberInquiriesCount { get; set; }
        public Decimal CreditsRemainingCount { get; set; }
        public CreditsHero.Subscribers.Dtos.SubscribersInquiriesDto SubscriberInquiries { get; set; }
        public List<CreditsHero.Subscribers.Dtos.SubscribersRequestDto> SubscriberRequests { get; set; }
        public MemberQuotesViewModel SubscriberQuotes { get; set; }
        
        public MemberSummaryViewModel()
        {
            SubscriberInquiries = new SubscribersInquiriesDto();
            SubscriberRequests = new List<SubscribersRequestDto>();
            SubscriberQuotes = new MemberQuotesViewModel();
        }
    }
}