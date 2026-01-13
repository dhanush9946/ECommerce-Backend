

using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IWishlistRepository
    {
        Task<bool> Exists(Guid userId, int productId);
        Task Add(Wishlist wishlist);
        Task<List<Wishlist>> GetByUserId(Guid userId);
        Task<Wishlist?> Get(Guid userId, int productId);
        void Remove(Wishlist wishlist);
        Task SaveChanges();
    }
}
