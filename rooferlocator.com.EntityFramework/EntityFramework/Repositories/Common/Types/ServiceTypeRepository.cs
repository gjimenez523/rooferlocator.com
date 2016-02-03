﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Abp.EntityFramework;
using rooferlocator.com.Common.Types;

namespace rooferlocator.com.EntityFramework.Repositories.Common.Types
{
    public class ServiceTypeRepository : comRepositoryBase<ServiceType>, IServiceTypeRepository
    {
        public ServiceTypeRepository(IDbContextProvider<comDbContext> dbContextProvider) : base(dbContextProvider) { }
    }
}
