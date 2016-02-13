using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;
using CreditsHero.Subscribers.Dtos;
using rooferlocator.com.Common;
using rooferlocator.com.Common.Types.Dtos;
using System;

namespace rooferlocator.com.Web.Models.Member
{
    public class MemberPaymentViewModel : IInputDto
    {
        public Guid CompanyId { get; set; }
        public int SubscriberId { get; set; }
        public int Credits { get; set; }
        public Decimal CostPerCredit { get; set; }

        public MemberPaymentViewModel()
        {
        }
    }
}