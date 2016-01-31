﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Abp.EntityFramework;
using rooferlocator.Common;

namespace rooferlocator.com.EntityFramework.Repositories.Common
{
    public class MemberRepository : comRepositoryBase<Member>, IMemberRepository
    {
        public MemberRepository(IDbContextProvider<comDbContext> dbContextProvider) : base(dbContextProvider) { }

        public List<Member> GetMembersWithCompany()
        {
            //var query = Get(agreementId);
            var query = GetAll();
            return query.Include(member => member.Company)
                .Include(member => member.User)
                .ToList<Member>();
        }
    }
}
