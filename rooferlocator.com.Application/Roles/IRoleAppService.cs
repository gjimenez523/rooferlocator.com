using System.Threading.Tasks;
using Abp.Application.Services;
using rooferlocator.com.Roles.Dto;

namespace rooferlocator.com.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);
    }
}
