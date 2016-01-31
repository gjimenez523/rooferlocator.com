using System.Collections.Generic;
using Abp.Domain.Repositories;

namespace rooferlocator.Common
{
    public interface IMemberRepository : IRepository<Member>
    {
        List<Member> GetMembersWithCompany();
    }
}