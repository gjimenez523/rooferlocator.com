using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;
using CreditsHero.Subscribers.Dtos;
using rooferlocator.com.Common;
using System.Collections.Generic;
using CreditsHero.Messaging.Dtos;
using rooferlocator.com.Common.Types.Dtos;

namespace rooferlocator.com.Web.Models.Member
{
    public class MemberLocationsViewModel : IInputDto
    {
        public int RequestId { get; set; }
        public int SubscriberId { get; set; }
        public GetLocationOutput Locations { get; set; }
        public GetLocationOutput States { get; set; }
        public GetLocationOutput Cities { get; set; }
        
        public MemberLocationsViewModel()
        {
            Locations = new GetLocationOutput();
            States = new GetLocationOutput();
            Cities = new GetLocationOutput();
        }
    }
}