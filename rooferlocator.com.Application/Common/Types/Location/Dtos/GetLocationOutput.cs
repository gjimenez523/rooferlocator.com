﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace rooferlocator.com.Common.Types.Dtos
{
    public class GetLocationOutput : IOutputDto
    {
        public List<LocationDto> Locations { get; set; }
    }
}
