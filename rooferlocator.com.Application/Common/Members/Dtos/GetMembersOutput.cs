﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace rooferlocator.com.Common.Members.Dtos
{
    public class GetMembersOutput : IOutputDto
    {
        public List<MemberDto> Members { get; set; }
    }
}
