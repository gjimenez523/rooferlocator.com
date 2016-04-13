using System.Collections.Generic;
using Abp.Domain.Repositories;

namespace rooferlocator.com.Common
{
    public interface IMemberRepository : IRepository<Member>
    {
        List<Member> GetMembersWithCompany();
        List<Member> GetMembersWithCompany(int memberId);
        List<MemberVisits> GetMemberVisits();
    }
}