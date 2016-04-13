﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Abp.EntityFramework;
using rooferlocator.com.Common;

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

        public List<Member> GetMembersWithCompany(int memberId)
        {
            var query = GetAll();
            return query.Include(member => member.Company)
                .Include(member => member.User)
                .Where(member => member.UserRefId == memberId)
                .ToList<Member>();
        }

        public List<MemberVisits> GetMemberVisits()
        {
            using (var context = new comDbContext())
            {
                try
                {
                    var result = context.Database
                    .SqlQuery<MemberVisits>("GetVisits")
                    .ToList();

                    return result;
                }
                catch (System.Exception exc)
                {
                    throw exc;
                }
            }
        }
    }
}
