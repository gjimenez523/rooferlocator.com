﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace rooferlocator.com.Common.Types.Dtos
{
    public class GetLocationInput : IInputDto
    {
        public int? Id { get; set; }
    }
}
