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
            try
            {
                await _authService.RegisterAsync(request);
                return Ok("User registered successfully");
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            try
            {
                var result = await _authService.LoginAsync(request);

                SetRefreshTokenCookie(result.RefreshToken);

                return Ok(new
                {
                    result.UserId,
                    result.Name,
                    result.Role,
                    AccessToken = result.AccessToken
                });
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> refresh()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(refreshToken))
                    return Unauthorized();

                var result = await _refreshTokenService.RefreshAsync(
                    new RefreshTokenRequestDto { RefreshToken = refreshToken }
                    );


                SetRefreshTokenCookie(result.RefreshToken);

                return Ok(new
                {
                    AccessToken = result.AccessToken
                });
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    await _logoutService.LogoutAsync(
                        new LogoutRequestDto { RefreshToken = refreshToken }
                        );
                }

                Response.Cookies.Delete("refreshToken");

                return Ok("Logged out successfully");
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            [FromBody] ForgotPasswordRequestDto dto)
        {
            try
            {
                await _authService.ForgotPasswordAsync(dto.Email);
            }
            catch (ArgumentException)
            {
                // swallow – do NOT reveal anything
            }
            catch (InvalidOperationException)
            {
                // swallow – user may not exist or be inactive
            }


            // Do NOT reveal whether email exists or not
            return Ok(new
            {
                message = "If the email exists, a password reset link has been sent."
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
           [FromBody] ResetPasswordRequestDto dto)
        {
            try
            {
                await _authService.ResetPasswordAsync(dto.Token, dto.NewPassword);

                return Ok(new
                {
                    message = "Password has been reset successfully."
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Password reset is not allowed for this token.");
            }
        }



        private void SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
