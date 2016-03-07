﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Abp.EntityFramework;
using rooferlocator.com.Common;

namespace rooferlocator.com.EntityFramework.Repositories.Common
{
    public class CustomerRepository : comRepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDbContextProvider<comDbContext> dbContextProvider) : base(dbContextProvider) { }

        public List<Customer> GetCustomers(int companyId)
        {
            //var query = Get(agreementId);
            //var query = GetAll();
            //return query.Include(member => member.Company)
            //    .Include(member => member.User)
            //    .ToList<Member>();
            return null;
        }

        public List<Customer> GetCustomersByUserId(int userId)
        {
            //var query = GetAll();
            //return query.Include(member => member.Company)
            //    .Include(member => member.User)
            //    .Where(member => member.UserRefId == memberId)
            //    .ToList<Member>();
            return null;
        }
    }
}
