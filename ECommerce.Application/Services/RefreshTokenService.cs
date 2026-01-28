

using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class RefreshTokenService:IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly IUserRepository _userRepo;
        private readonly IJwtTokenService _jwtTokenService;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository,
                                   IUserRepository userRepository,
                                   IJwtTokenService jwtTokenService
                                  )
        {
            _refreshRepo = refreshTokenRepository;
            _userRepo = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<RefreshTokenResponseDto> RefreshAsync(RefreshTokenRequestDto request)
        {
            var refreshToken = await _refreshRepo.GetAsync(request.RefreshToken);

            if (refreshToken == null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            if (refreshToken.IsRevoked)
                throw new UnauthorizedAccessException("Refresh token revoked");

            if (refreshToken.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token expired");

            var user = await _userRepo.GetByIdAsync(refreshToken.UserId);

            if (user == null || !user.IsActive)
                throw new UnauthorizedAccessException("User is not found or user is inactive");

            refreshToken.IsRevoked = true;

            var newRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = Guid.NewGuid().ToString("N"),
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            refreshToken.ReplacedByToken = newRefreshToken.Token;

            await _refreshRepo.UpdateAsync(refreshToken);
            await _refreshRepo.AddAsync(newRefreshToken);



            var newAccessToken = _jwtTokenService.GenerateToken(user);

            return new RefreshTokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken=newRefreshToken.Token
            };
        }
    }
}
