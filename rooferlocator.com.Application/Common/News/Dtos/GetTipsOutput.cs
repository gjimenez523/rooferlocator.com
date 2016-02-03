﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace rooferlocator.com.Common.News.Dtos
{
    public class GetTipsOutput : IOutputDto
    {
        public List<TipsDto> Tips { get; set; }
    }
}
