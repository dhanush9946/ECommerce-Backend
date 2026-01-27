

using ECommerce.Application.DTOs.Auth;

namespace ECommerce.Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenResponseDto> RefreshAsync(
        RefreshTokenRequestDto request
    );
    }
}
