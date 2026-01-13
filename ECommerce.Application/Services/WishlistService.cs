using ECommerce.Application.DTOs.Wishlist;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly ICartRepository _cartRepository;

        public WishlistService(
         IWishlistRepository wishlistRepository,
         ICartRepository cartRepository)
        {
            _wishlistRepository = wishlistRepository;
            _cartRepository = cartRepository;
        }

        public async Task AddToWishlist(Guid userId, AddToWishlistDto dto)
        {
            if (await _wishlistRepository.Exists(userId, dto.ProductId))
                throw new Exception("Product already in wishlist");

            var wishlist = new Wishlist
            {
                UserId = userId,
                ProductId = dto.ProductId
            };

            await _wishlistRepository.Add(wishlist);
            await _wishlistRepository.SaveChanges();
        }

        public async Task<List<WishlistResponseDto>> GetWishlist(Guid userId)
        {
            var items = await _wishlistRepository.GetByUserId(userId);

            return items.Select(w => new WishlistResponseDto
            {
                ProductId = w.ProductId,
                ProductName = w.Product?.Name ?? string.Empty,
                Price = w.Product?.Price ?? 0,
                ImageUrl = w.Product?.ImageUrl
            }).ToList();
        }

        public async Task RemoveFromWishlist(Guid userId, int productId)
        {
            var item = await _wishlistRepository.Get(userId, productId);

            if (item == null)
                throw new Exception("Wishlist item not found");

             _wishlistRepository.Remove(item);
            await _wishlistRepository.SaveChanges();
        }




        public async Task MoveToCart(Guid userId, int productId)
        {
            
            var wishlistItem = await _wishlistRepository.Get(userId, productId);

            if (wishlistItem == null)
                throw new Exception("Wishlist item not found");

            //  Check if product already exists in cart
            var cartItem = await _cartRepository.GetCartItem(userId, productId);

            if (cartItem != null)
            {
                // Increase quantity
                cartItem.Quantity += 1;
                await _cartRepository.UpdateAsync(cartItem);
            }
            else
            {
                // Add new cart item
                var newCartItem = new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = 1
                };

                await _cartRepository.AddAsync(newCartItem);
            }

            //  Remove from wishlist
            _wishlistRepository.Remove(wishlistItem);
            await _wishlistRepository.SaveChanges();
        }

    }
}
