﻿using Abp.Application.Services;
using rooferlocator.com.Common.Types.Dtos;

namespace rooferlocator.com.Common.Types
{
    public interface IQuoteTypeAppService : IApplicationService
    {
        GetQuoteTypeOutput GetQuoteTypes(CreditsHero.Common.Dtos.GetCriteriaInput input);
        void UpdateQuoteType(UpdateQuoteTypeInput input);
        void CreateQuoteType(CreateQuoteTypeInput input);
    }
}
