﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace rooferlocator.com.Common.Companies.Dtos
{
    public class GetCompanyInput : IInputDto
    {
        public int? CompanyId { get; set; }
    }
}
