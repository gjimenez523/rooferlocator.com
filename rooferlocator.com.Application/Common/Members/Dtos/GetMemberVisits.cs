﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace rooferlocator.com.Common.Members.Dtos
{
    public class GetMemberVisitsOutput : IOutputDto
    {
        public List<MemberVisitDto> MemberVisits { get; set; }
    }
}
