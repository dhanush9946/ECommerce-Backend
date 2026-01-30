

using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IPasswordResetTokenRepository
    {
        Task AddAsync(PasswordResetToken token);
        Task<PasswordResetToken?> GetValidTokenAsync(string tokenHash);
        Task MarkAsUsedAsync(PasswordResetToken token);
        Task InvalidateAllForUserAsync(Guid userId);
    }
}
