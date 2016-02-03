﻿using Abp.Application.Services;
using rooferlocator.com.Common.News.Dtos;

namespace rooferlocator.com.Common.News
{
    public interface INewsAppService : IApplicationService
    {
        GetNewsOutput GetNews(GetNewsInput input);
        GetNewsCategoriesOutput GetNewsCategories(GetNewsCategoriesInput input);
        void UpdateNews(UpdateNewsInput input);
        void UpdateNewsCategories(UpdateNewsCategoriesInput input);
        void CreateNews(CreateNewsInput input);
        void CreateNewsCategories(CreateNewsCategoriesInput input);
    }
}
