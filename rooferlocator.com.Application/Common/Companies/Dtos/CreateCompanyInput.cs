﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace rooferlocator.com.Common.Companies.Dtos
{
    public class CreateCompanyInput : IInputDto, ICustomValidate
    {
        //[Range(1, long.MaxValue)] //Data annotation attributes work as expected.
        public int? CompanyId { get; set; }

        [Required]
        public string Name { get; set; }

        public string LogoUri { get; set; }

        public void AddValidationErrors(List<ValidationResult> results) { }
    }
}
