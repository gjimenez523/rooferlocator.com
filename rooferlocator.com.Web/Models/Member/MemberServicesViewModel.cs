using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;
using CreditsHero.Subscribers.Dtos;
using rooferlocator.com.Common;
using rooferlocator.com.Common.Types.Dtos;

namespace rooferlocator.com.Web.Models.Member
{
    public class MemberServicesViewModel : IInputDto
    {
        public GetCriteriaValuesOutput CriteriaValues { get; set; }

        public MemberServicesViewModel()
        {
        }
    }
}