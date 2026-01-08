using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController:ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthController(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            if (request.Password != request.ConfirmPassword)
                return BadRequest("Passwords do not match");

            var emailExists = await _userRepository.EmailExistsAsync(request.Email);
            if (emailExists)
                return BadRequest("Email already registered");

            //Creates Entity
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email=request.Email,
                PhoneNumber=request.PhoneNumber,
                PasswordHash=_passwordHasher.Hash(request.Password),
                Role="User"
            };

            //we can save to Database
            await _userRepository.AddAsync(user);

            return Ok("User registered successfully");

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || !user.IsActive)
                return Unauthorized("Invalid email or password");

            var isPasswordValid = _passwordHasher.Verify(
                request.Password, user.PasswordHash);

            if (!isPasswordValid)
                return Unauthorized("Invalid email or password");

            //Generate JWT Token

            var token = GenerateJwtToken(user);

            //response
            var response = new LoginResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Role = user.Role,
                Token = token
            };

            return Ok(response);


        }

        //JWT generation method

        private string GenerateJwtToken(Domain.Entities.User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
                );
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
                );

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(jwtSettings["ExpiryMinutes"]!)
                    ),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
