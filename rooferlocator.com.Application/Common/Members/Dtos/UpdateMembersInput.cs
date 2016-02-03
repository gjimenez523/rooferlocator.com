﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace rooferlocator.com.Common.Members.Dtos
{
    public class UpdateMemberInput : IInputDto, ICustomValidate
    {
        public int MemberId { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Credential { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseDescription { get; set; }
        public long UserRefId { get; set; }
        public int CompanyRefId { get; set; }

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
