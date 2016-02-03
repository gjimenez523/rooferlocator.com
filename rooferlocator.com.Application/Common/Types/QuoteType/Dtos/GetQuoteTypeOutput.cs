﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace rooferlocator.com.Common.Types.Dtos
{
    public class GetQuoteTypeOutput : IOutputDto
    {
        public List<QuoteTypeDto> QuoteTypes { get; set; }
    }
}
