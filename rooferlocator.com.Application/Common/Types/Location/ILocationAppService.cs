﻿using Abp.Application.Services;
using rooferlocator.com.Common.Types.Dtos;

namespace rooferlocator.com.Common.Types
{
    public interface ILocationAppService : IApplicationService
    {
        GetLocationOutput GetLocations(GetLocationInput input);
        GetLocationOutput GetStates();
        GetLocationOutput GetCities(CreditsHero.Common.Dtos.GetCriteriaInput input, string state);
        void UpdateLocation(UpdateLocationInput input);
        void CreateLocation(CreateLocationInput input);
    }
}
