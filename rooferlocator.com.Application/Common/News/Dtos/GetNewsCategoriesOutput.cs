﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace rooferlocator.com.Common.News.Dtos
{
    public class GetNewsCategoriesOutput : IOutputDto
    {
        public List<NewsCategoriesDto> NewsCategories { get; set; }
    }
}
