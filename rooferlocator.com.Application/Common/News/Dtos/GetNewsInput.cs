﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace rooferlocator.com.Common.News.Dtos
{
    public class GetNewsInput : IInputDto
    {
        public int? Id { get; set; }
    }
}
