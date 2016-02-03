﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace rooferlocator.com.Common.News.Dtos
{
    public class CreateTipsInput : IInputDto, ICustomValidate
    {
        //[Range(1, long.MaxValue)] //Data annotation attributes work as expected.
        public int? Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public int TipsCategoriesRefId { get; set; }
        public bool IsDeleted { get; set; }

        public void AddValidationErrors(List<ValidationResult> results) { }
    }
}
