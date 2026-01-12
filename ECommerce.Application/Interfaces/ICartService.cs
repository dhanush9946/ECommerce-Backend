

using ECommerce.Application.DTOs.Cart;

namespace ECommerce.Application.Interfaces
{
    public interface ICartService
    {
        Task AddToCart(Guid userId, AddToCartDto dto);
        Task<List<CartResponseDto>> GetCart(Guid userId);
        Task RemoveFromCart(Guid userId, int productId);
        Task UpdateQuantity(Guid userId, UpdateCartQuantityDto dto);
    }
}
