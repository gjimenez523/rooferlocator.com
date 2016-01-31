﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Abp.EntityFramework;
using rooferlocator.Common.News;

namespace rooferlocator.com.EntityFramework.Repositories.Common
{
    public class TipsRepository : comRepositoryBase<Tips>, ITipsRepository
    {
        public TipsRepository(IDbContextProvider<comDbContext> dbContextProvider) : base(dbContextProvider) { }
    }
}
