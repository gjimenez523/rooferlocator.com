﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace rooferlocator.com.Common.Types.Dtos
{
    public class UpdateQuoteTypeInput : IInputDto, ICustomValidate
    {
        //[Range(1, long.MaxValue)] //Data annotation attributes work as expected.
        public int? Id { get; set; }
        public string Quote { get; set; }
        public string DisplayName { get; set; }
        public bool IsDeleted { get; set; }

        //Custom validation method. It's called by ABP after data annotation validations.
        public void AddValidationErrors(List<ValidationResult> results)
        {
            //if (AssignedPersonId == null && State == null)
            //{
            //    results.Add(new ValidationResult("Both of AssignedPersonId and State can not be null in order to update a Task!", new[] { "AssignedPersonId", "State" }));
            //}
        }

    }
}
