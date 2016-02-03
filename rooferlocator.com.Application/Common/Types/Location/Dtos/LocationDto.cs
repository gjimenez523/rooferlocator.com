using System;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using CreditsHero.Common.Dtos;
using Abp.AutoMapper;

namespace rooferlocator.com.Common.Types.Dtos
{
    [AutoMapFrom(typeof(LocationType))]
    public class LocationDto : CriteriaValuesDto
    {
        public string State { get; set; }
        public string City { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }

    }
}
