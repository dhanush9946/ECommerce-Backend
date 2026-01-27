

using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
