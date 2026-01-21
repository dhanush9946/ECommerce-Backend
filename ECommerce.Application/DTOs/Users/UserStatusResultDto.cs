

namespace ECommerce.Application.DTOs.Users
{
    public class UserStatusResultDto
    {
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
        public string Message { get; set; } = null!;
    }
}
