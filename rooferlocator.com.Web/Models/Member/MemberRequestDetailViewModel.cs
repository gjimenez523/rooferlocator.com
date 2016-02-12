using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;
using CreditsHero.Subscribers.Dtos;
using rooferlocator.com.Common;
using System.Collections.Generic;

namespace rooferlocator.com.Web.Models.Member
{
    public class MemberRequestDetailViewModel : IInputDto
    {
        public int RequestId { get; set; }
        public int SubscriberId { get; set; }
        public List<CreditsHero.Subscribers.Dtos.SubscribersRequestDetailDto> SubscriberRequestDetails { get; set; }
        
        public MemberRequestDetailViewModel()
        {
            SubscriberRequestDetails = new List<SubscribersRequestDetailDto>();
        }
    }
}