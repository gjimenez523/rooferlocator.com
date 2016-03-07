﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace rooferlocator.com.Common.Customers.Dtos
{
    public class GetCustomersOutput : IOutputDto
    {
        public List<CustomerDto> Customers { get; set; }
    }
}
