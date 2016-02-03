﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Abp.EntityFramework;
using rooferlocator.com.Common.News;

namespace rooferlocator.com.EntityFramework.Repositories.Common
{
    public class NewsRepository : comRepositoryBase<News>, INewsRepository
    {
        public NewsRepository(IDbContextProvider<comDbContext> dbContextProvider) : base(dbContextProvider) { }
    }
}
