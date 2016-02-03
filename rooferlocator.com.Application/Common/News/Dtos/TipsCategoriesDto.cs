using System;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using Abp.AutoMapper;

namespace rooferlocator.com.Common.News.Dtos
{
    [AutoMapFrom(typeof(TipsCategories))]
    public class TipsCategoriesDto : EntityDto
    {
        public string Title { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }

    }
}
