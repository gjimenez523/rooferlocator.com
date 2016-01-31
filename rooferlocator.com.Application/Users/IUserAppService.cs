using System.Threading.Tasks;
using Abp.Application.Services;
using rooferlocator.com.Users.Dto;

namespace rooferlocator.com.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task ProhibitPermission(ProhibitPermissionInput input);

        Task RemoveFromRole(long userId, string roleName);
    }
}