

using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token);
        Task<RefreshToken?> GetAsync(string token);
        Task UpdateAsync(RefreshToken token);
    }
}
