﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Abp.EntityFramework;
using rooferlocator.Sales;

namespace rooferlocator.com.EntityFramework.Repositories.Sales
{
    public class LeadRepository : comRepositoryBase<Lead>, ILeadRepository
    {
        public LeadRepository(IDbContextProvider<comDbContext> dbContextProvider) : base(dbContextProvider) { }
    }
}
