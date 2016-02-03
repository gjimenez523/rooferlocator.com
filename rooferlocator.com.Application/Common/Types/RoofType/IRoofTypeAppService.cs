﻿using Abp.Application.Services;
using rooferlocator.com.Common.Types.Dtos;

namespace rooferlocator.com.Common.Types
{
    public interface IRoofTypeAppService : IApplicationService
    {
        //CreditsHero.Common.Dtos.GetCriteriaValuesOutput GetRoofTypes(CreditsHero.Common.Dtos.GetCriteriaInput input);
        Dtos.GetRoofTypeOutput GetRoofTypes(CreditsHero.Common.Dtos.GetCriteriaInput input);
        void UpdateRoofType(UpdateRoofTypeInput input);
        void CreateRoofType(CreateRoofTypeInput input);
    }
}
