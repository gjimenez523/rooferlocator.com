﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace rooferlocator.com.Common.Members.Dtos
{
    public class CreateMemberInput : IInputDto, ICustomValidate
    {
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

        public void AddValidationErrors(List<ValidationResult> results) { }
    }
}
