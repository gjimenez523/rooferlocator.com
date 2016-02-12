using System;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using Abp.AutoMapper;

namespace rooferlocator.com.Common.Companies.Dtos
{
    [AutoMapFrom(typeof(Company))]
    public class CompanyDto : EntityDto
    {
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ServiceArea { get; set; }
        public string YearEstablished { get; set; }
        public string LogoUri { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }

    }
}
