﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace rooferlocator.com.Common.Companies.Dtos
{
    public class GetCompaniesOutput : IOutputDto
    {
        public List<CompanyDto> Companies { get; set; }
    }
}
