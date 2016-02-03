﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using CreditsHero.Common.Dtos;

namespace rooferlocator.com.Common.Types.Dtos
{
    public class CreateRoofTypeInput : CreateCriteriaValuesInput, IInputDto, ICustomValidate
    {
        //[Range(1, long.MaxValue)] //Data annotation attributes work as expected.
        public int? Id { get; set; }

        [Required]
        public string Code { get; set; }

        public string Description { get; set; }

        public void AddValidationErrors(List<ValidationResult> results) { }
    }
}
