﻿using Abp.Application.Services;
using rooferlocator.com.Common.Types.Dtos;

namespace rooferlocator.com.Common.Types
{
    public interface IServiceTypeAppService : IApplicationService
    {
        GetServiceTypeOutput GetServiceTypes(CreditsHero.Common.Dtos.GetCriteriaInput input);
        void UpdateServiceType(UpdateServiceTypeInput input);
        void CreateServiceType(CreateServiceTypeInput input);
    }
}
