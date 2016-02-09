using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;
using CreditsHero.Subscribers.Dtos;
using rooferlocator.com.Common;

namespace rooferlocator.com.Web.Models.Member
{
    public class MemberSummaryViewModel : IInputDto
    {
        /// <summary>
        /// Not required for single-tenant applications.
        /// </summary>
        public int SubscriberInquiriesCount { get; set; }
        public int CreditsRemainingCount { get; set; }
        
        public MemberSummaryViewModel()
        {
            //Company = new Company();
        }
    }
}