﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace rooferlocator.com.Common.Members.Dtos
{
    public class GetMemberInput : IInputDto
    {
        public int? MemberId { get; set; }
    }
}
