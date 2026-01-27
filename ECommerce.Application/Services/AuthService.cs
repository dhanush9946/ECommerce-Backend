using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;



namespace ECommerce.Application.Services
{
    public class AuthService:IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtService;
        private readonly IRefreshTokenRepository _refreshRepo;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtService,
            IRefreshTokenRepository refreshRepo)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _refreshRepo = refreshRepo;
        }

        public async Task RegisterAsync(RegisterRequestDto request)
        {
            if (request.Password != request.ConfirmPassword)
                throw new ApplicationException("Passwords do not match");

            var emailExists = await _userRepository.EmailExistsAsync(request.Email);
            if (emailExists)
                throw new ApplicationException("Email already registered");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = _passwordHasher.Hash(request.Password),
                Role = "User"
            };
            await _userRepository.AddAsync(user);

        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || !user.IsActive)
                throw new UnauthorizedAccessException("Invalid email or password");

            var isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Invalid email or password");

            var accessToken = _jwtService.GenerateToken(user);

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = Guid.NewGuid().ToString("N"),
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _refreshRepo.AddAsync(refreshToken);

            return new LoginResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Role = user.Role,
                AccessToken=accessToken,
                RefreshToken=refreshToken.Token
            };
        }

       
        
        

    }
}
