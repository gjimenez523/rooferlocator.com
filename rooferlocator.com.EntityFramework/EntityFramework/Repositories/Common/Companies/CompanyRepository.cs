﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Abp.EntityFramework;
using rooferlocator.Common;

namespace rooferlocator.com.EntityFramework.Repositories.Common
{   
    public class CompanyRepository : comRepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(IDbContextProvider<comDbContext> dbContextProvider) : base(dbContextProvider) { }

        //public List<Company> GetAllWithLocations(int? companyId)
        //{
        //    //In repository methods, we do not deal with create/dispose DB connections, DbContexes and transactions. ABP handles it.
        //    var query = GetAll(); //GetAll() returns IQueryable<T>, so we can query over it.
        //    //var query = Context.Tasks.AsQueryable(); //Alternatively, we can directly use EF's DbContext object.
        //    //var query = Table.AsQueryable(); //Another alternative: We can directly use 'Table' property instead of 'Context.Tasks', they are identical.

        //    if (companyId.HasValue)
        //    {
        //        query = query.Where(customer => customer.Id == companyId.Value);      
        //    }

        //    return query
        //        .OrderByDescending(company => company.CreationTime)
        //        .Include(company => company.CorporateLocation) //Include Billing Location in a single query
        //        .ToList();
        //}
    }
}
