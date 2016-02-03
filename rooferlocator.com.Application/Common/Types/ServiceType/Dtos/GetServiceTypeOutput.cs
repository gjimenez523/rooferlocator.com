﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace rooferlocator.com.Common.Types.Dtos
{
    public class GetServiceTypeOutput : IOutputDto
    {
        public List<ServiceTypeDto> ServiceTypes { get; set; }
    }
}
