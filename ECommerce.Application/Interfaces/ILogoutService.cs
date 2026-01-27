

using ECommerce.Application.DTOs.Auth;

namespace ECommerce.Application.Interfaces
{
    public interface ILogoutService
    {
        Task LogoutAsync(LogoutRequestDto request);
    }
}
