using System.Collections.Generic;
using Abp.Application.Services.Dto;
using System;

namespace rooferlocator.com.Common.Customers.Dtos
{
    public class GetCustomerInput : IInputDto
    {
        public int? CustomerId { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
