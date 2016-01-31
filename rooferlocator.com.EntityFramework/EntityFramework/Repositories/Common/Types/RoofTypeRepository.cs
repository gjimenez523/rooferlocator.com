﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Abp.EntityFramework;
using rooferlocator.Common.Types;

namespace rooferlocator.com.EntityFramework.Repositories.Common.Types
{
    public class RoofTypeRepository : comRepositoryBase<RoofType>, IRoofTypeRepository
    {
        public RoofTypeRepository(IDbContextProvider<comDbContext> dbContextProvider) : base(dbContextProvider) { }
    }
}
