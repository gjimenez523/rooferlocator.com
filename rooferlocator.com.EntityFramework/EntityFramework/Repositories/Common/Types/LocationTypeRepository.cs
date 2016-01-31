﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Abp.EntityFramework;
using rooferlocator.Common.Types;

namespace rooferlocator.com.EntityFramework.Repositories.Common.Types
{
    public class LocationTypeRepository : comRepositoryBase<LocationType>, ILocationTypeRepository
    {
        public LocationTypeRepository(IDbContextProvider<comDbContext> dbContextProvider) : base(dbContextProvider) { }

        public List<LocationType> GetStates()
        {
            var query = GetAll();

            return query
                .GroupBy(g => g.State)
                .Select(l => l.FirstOrDefault()).ToList();
        }

        public List<LocationType> GetCities(string state)
        {
            var query = GetAll();

            return query.Where(l => l.State == state).ToList();
        }
    }
}
