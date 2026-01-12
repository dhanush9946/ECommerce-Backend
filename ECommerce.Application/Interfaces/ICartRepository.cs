

using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartItem(Guid userId, int productId);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task<List<Cart>> GetUserCart(Guid userId);
        Task RemoveAsync(Cart cart);
    }
}
