

using ECommerce.Application.DTOs.Wishlist;

namespace ECommerce.Application.Interfaces
{
    public interface IWishlistService
    {
        Task AddToWishlist(Guid userId, AddToWishlistDto dto);
        Task<List<WishlistResponseDto>> GetWishlist(Guid userId);
        Task RemoveFromWishlist(Guid userId, int productId);
        Task MoveToCart(Guid userId, int productId);

    }
}
