using System.Collections.Generic;
using Abp.Domain.Repositories;

namespace rooferlocator.com.Common
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        List<Customer> GetCustomers(int companyId);
        List<Customer> GetCustomersByUserId(int userId);
    }
}