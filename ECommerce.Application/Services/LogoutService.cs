

using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Interfaces;

namespace ECommerce.Application.Services
{
    public class LogoutService:ILogoutService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        public LogoutService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepo = refreshTokenRepository;
        }

        public async Task LogoutAsync(LogoutRequestDto request)
        {
            var token = await _refreshTokenRepo.GetAsync(request.RefreshToken);

            if (token == null)
                return;

            if (token.IsRevoked)
                return;

            token.IsRevoked = true;
            await _refreshTokenRepo.UpdateAsync(token);
        }
    }
}
