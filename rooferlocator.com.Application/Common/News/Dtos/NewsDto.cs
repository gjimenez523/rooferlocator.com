using System;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using Abp.AutoMapper;

namespace rooferlocator.com.Common.News.Dtos
{
    [AutoMapFrom(typeof(News))]
    public class NewsDto : EntityDto
    {
        public string Quote { get; set; }
        public string DisplayName{ get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }

    }
}
