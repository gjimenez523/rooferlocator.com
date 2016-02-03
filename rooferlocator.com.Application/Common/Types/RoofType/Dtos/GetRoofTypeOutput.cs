﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace rooferlocator.com.Common.Types.Dtos
{
    public class GetRoofTypeOutput : IOutputDto
    {
        public List<RoofTypeDto> RoofTypes { get; set; }
    }
}
