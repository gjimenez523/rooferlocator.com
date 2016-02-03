﻿using Abp.Application.Services;
using rooferlocator.com.Common.News.Dtos;

namespace rooferlocator.com.Common.News
{
    public interface ITipsAppService : IApplicationService
    {
        GetTipsOutput GetTips(GetTipsInput input);
        GetTipsCategoriesOutput GetTipsCategories(GetTipsCategoriesInput input);
        void UpdateTips(UpdateTipsInput input);
        void UpdateTipsCategories(UpdateTipsCategoriesInput input);
        void CreateTips(CreateTipsInput input);
        void CreateTipsCategories(CreateTipsCategoriesInput input);
    }
}
