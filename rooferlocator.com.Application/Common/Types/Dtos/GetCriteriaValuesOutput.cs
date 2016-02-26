using System.Collections.Generic;
using Abp.Application.Services.Dto;
using CreditsHero.Common.Dtos;

namespace rooferlocator.com.Common.Types.Dtos
{
    public class GetCriteriaValuesOutput : IOutputDto
    {
        public List<CriteriaValuesDto> CriteriaList { get; set; }
    }
}
