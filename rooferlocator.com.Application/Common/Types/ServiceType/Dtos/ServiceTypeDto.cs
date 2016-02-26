using System;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using CreditsHero.Common.Dtos;
using Abp.AutoMapper;

namespace rooferlocator.com.Common.Types.Dtos
{
    [AutoMapFrom(typeof(ServiceType))]
    public class ServiceTypeDto : CriteriaValuesDto //EntityDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }

    }
}
