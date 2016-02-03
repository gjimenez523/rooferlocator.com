﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace rooferlocator.com.Common.News.Dtos
{
    public class GetTipsCategoriesOutput : IOutputDto
    {
        public List<TipsCategoriesDto> TipsCategories { get; set; }
    }
}
