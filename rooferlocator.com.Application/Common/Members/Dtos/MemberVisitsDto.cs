using System;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using rooferlocator.com.Users.Dto;
using rooferlocator.com.Users;
using rooferlocator.com.Common.Companies.Dtos;
using rooferlocator.com.Sessions.Dto;
using Abp.AutoMapper;
using CreditsHero.Subscribers.Dtos;

namespace rooferlocator.com.Common.Members.Dtos
{
    [AutoMapFrom(typeof(MemberVisits))]
    public class MemberVisitDto : EntityDto
    {
        public int VisitMonth { get; set; }
        public int VisitDay { get; set; }
        public int VisitYear { get; set; }
        public int ReturnVisits { get; set; }
        public int NewVisits { get; set; }
    }
}
