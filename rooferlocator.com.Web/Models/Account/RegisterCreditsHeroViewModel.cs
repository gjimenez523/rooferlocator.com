using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;
using CreditsHero.Subscribers.Dtos;

namespace rooferlocator.com.Web.Models.Account
{
    public class RegisterCreditsHeroViewModel : IInputDto
    {
        public int SuscriberId { get; set; }
        public string SubscriberName { get; set; }
        public string SubscriberEmail { get; set; }
        public string SubscriberSms { get; set; }
        public CreditsHero.Common.Dtos.GetCriteriaValuesOutput RoofTypeCriteria { get; set; }
        public CreditsHero.Common.Dtos.GetCriteriaValuesOutput ServiceTypeCriteria { get; set; }
    }
}