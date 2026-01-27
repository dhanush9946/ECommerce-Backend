using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;


namespace ECommerce.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController:ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ILogoutService _logoutService;

        public AuthController(IAuthService authService,
                              IRefreshTokenService refreshTokenService,
                              ILogoutService logoutService)
        {
            _authService = authService;
            _refreshTokenService = refreshTokenService;
            _logoutService = logoutService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            await _authService.RegisterAsync(request);
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> refresh(RefreshTokenRequestDto request)
        {
            var result = await _refreshTokenService.RefreshAsync(request);
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutRequestDto request)
        {
            await _logoutService.LogoutAsync(request);
            return Ok("Logged out successfully");
        }
    }
}
