

using ECommerce.Application.DTOs.Users;

namespace ECommerce.Application.Interfaces
{
    public interface IAdminUserService
    {
        Task<List<UserListDto>> GetAllUsersAsync();
        Task<UserStatusResultDto> UpdateUserStatusAsync(
        Guid userId,
        bool isActive
        );
    }
}
