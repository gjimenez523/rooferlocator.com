﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace rooferlocator.com.Common.Types.Dtos
{
    public class CreateQuoteTypeInput : IInputDto, ICustomValidate
    {
        //[Range(1, long.MaxValue)] //Data annotation attributes work as expected.
        public int? Id { get; set; }

        [Required]
        public string Quote { get; set; }

        public string DisplayName { get; set; }

        public void AddValidationErrors(List<ValidationResult> results) { }
    }
}
