using System;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using rooferlocator.com.Users.Dto;
using rooferlocator.com.Users;
using rooferlocator.com.Common.Companies.Dtos;
using rooferlocator.com.Sessions.Dto;
using Abp.AutoMapper;

namespace rooferlocator.com.Common.Members.Dtos
{
    [AutoMapFrom(typeof(Member))]
    public class MemberDto : EntityDto
    {
        public virtual CompanyDto Company { get; set; }
        public virtual UserLoginInfoDto User {get; set;}

        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Credential { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseDescription { get; set; }
        public long UserRefId { get; set; }
        public int CompanyRefId { get; set; }

    }
}
