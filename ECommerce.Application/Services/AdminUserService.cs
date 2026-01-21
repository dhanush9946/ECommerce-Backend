

using ECommerce.Application.DTOs.Users;
using ECommerce.Application.Interfaces;

namespace ECommerce.Application.Services
{
    public class AdminUserService:IAdminUserService
    {
        private readonly IUserRepository _userRepository;

        public AdminUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserListDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            var normalUsers = users
                              .Where(u => u.Role != "Admin")
                              .ToList();

            return normalUsers.Select(u => new UserListDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            }).ToList();
        }



        public async Task<UserStatusResultDto> UpdateUserStatusAsync(Guid userId,bool isActive)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            user.IsActive = isActive;
            await _userRepository.UpdateAsync(user);

            return new UserStatusResultDto {
                UserId = user.Id,
                IsActive = user.IsActive,
                Message = isActive ?
                    "User Unblocked Successfully":"User Blocked Successfully"
            };
        }
    }
}
