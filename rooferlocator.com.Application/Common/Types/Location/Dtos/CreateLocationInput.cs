﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace rooferlocator.com.Common.Types.Dtos
{
    public class CreateLocationInput : IInputDto, ICustomValidate
    {
        //[Range(1, long.MaxValue)] //Data annotation attributes work as expected.
        public int? Id { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public void AddValidationErrors(List<ValidationResult> results) { }
    }
}
