using System.Collections.Generic;
using Abp.Domain.Repositories;

namespace rooferlocator.Common.Types
{
    public interface ILocationTypeRepository : IRepository<LocationType>
    {
        List<LocationType> GetStates();
        List<LocationType> GetCities(string state);
    }
}