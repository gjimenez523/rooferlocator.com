using System.Collections.Generic;
using Abp.Application.Services.Dto;
using System;

namespace rooferlocator.com.Common.Members.Dtos
{
    public class GetMemberInput : IInputDto
    {
        public int? MemberId { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
