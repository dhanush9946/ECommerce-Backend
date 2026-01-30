using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Helpers;
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
        private readonly IPasswordResetTokenRepository _resetTokenRepo;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtService,
            IRefreshTokenRepository refreshRepo,
            IPasswordResetTokenRepository tokenRepo,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _refreshRepo = refreshRepo;
            _resetTokenRepo = tokenRepo;
            _unitOfWork = unitOfWork;
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


        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return;

            var rawToken = TokenHelper.GenerateToken();
            var hashedToken = TokenHelper.HashToken(rawToken);

            var resetToken = new PasswordResetToken
            {
                UserId = user.Id,
                TokenHash = hashedToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _resetTokenRepo.AddAsync(resetToken);
                await _unitOfWork.CommitAsync();

                var resetLink =
                $"https://yourfrontend.com/reset-password?token={rawToken}";
                // send email here
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

        }

        public async Task ResetPasswordAsync(string token,string newPassword)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var tokenHash = TokenHelper.HashToken(token);

                var resetToken = await _resetTokenRepo.GetValidTokenAsync(tokenHash);

                if (resetToken == null)
                    throw new Exception("Invalid or Expired token");

                var user = resetToken.User;

                user.PasswordHash = _passwordHasher.Hash(newPassword);

                await _userRepository.UpdateAsync(user);
                await _resetTokenRepo.MarkAsUsedAsync(resetToken);
                await _resetTokenRepo.InvalidateAllForUserAsync(user.Id);


                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

       
        
        

    }
}
