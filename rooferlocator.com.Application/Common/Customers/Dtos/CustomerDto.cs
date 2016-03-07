using System;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using rooferlocator.com.Users.Dto;
using rooferlocator.com.Users;
using rooferlocator.com.Common.Companies.Dtos;
using rooferlocator.com.Sessions.Dto;
using Abp.AutoMapper;
using CreditsHero.Subscribers.Dtos;

namespace rooferlocator.com.Common.Customers.Dtos
{
    public class CustomerDto : EntityDto
    {
        public virtual SubscribersInquiryDto Inquiry { get; set; }
        public virtual UserLoginInfoDto User {get; set;}
        
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CellPhone { get; set; }
        public string Comment { get; set; }
    }
}
