

using ECommerce.Application.DTOs.Cart;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class CartService:ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        public CartService(ICartRepository cartRepository,
                           IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task AddToCart(Guid userId, AddToCartDto dto)
        {

            var product = await _productRepository.GetByIdAsync(dto.ProductId);

            if (product == null)
                throw new Exception("Product not found");

            if (dto.Quantity > product.MaxOrderQuantity)
                throw new Exception(
                    $"Maximum {product.MaxOrderQuantity} units allowed per order");

            if (dto.Quantity > product.Stock)
                throw new Exception("Insufficient stock");

            var cartItem = await _cartRepository.GetCartItem(userId, dto.ProductId);


            if (cartItem != null)
            {
                cartItem.Quantity += dto.Quantity;
                await _cartRepository.UpdateAsync(cartItem);
            }

            else
            {
                var cart = new Cart
                {
                    UserId = userId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };
                await _cartRepository.AddAsync(cart);
            }
        }


        public async Task UpdateQuantity(Guid userId, UpdateCartQuantityDto dto)
        {
            var cartItem = await _cartRepository.GetCartItem(userId, dto.ProductId);

            if (cartItem == null)
                throw new Exception("Cart item not found");

            if (dto.Action == "increase")
            {
                cartItem.Quantity += 1;
                await _cartRepository.UpdateAsync(cartItem);
            }
            else if (dto.Action == "decrease")
            {
                cartItem.Quantity -= 1;

                if (cartItem.Quantity <= 0)
                {
                    await _cartRepository.RemoveAsync(cartItem);
                }
                else
                {
                    await _cartRepository.UpdateAsync(cartItem);
                }
            }
            else
            {
                throw new Exception("Invalid action");
            }
        }

        public async Task<List<CartResponseDto>> GetCart(Guid userId)
        {
            var cartItems = await _cartRepository.GetUserCart(userId);

            return cartItems.Select(c => new CartResponseDto
            {
                CartId = c.Id,
                ProductId = c.ProductId,
                ProductName = c.Product?.Name ?? string.Empty,
                Price = c.Product?.Price ?? 0,
                Quantity = c.Quantity
            }).ToList();
        }
        public async Task RemoveFromCart(Guid userId,int productId)
        {
            var cartItem = await _cartRepository.GetCartItem(userId, productId);
            if (cartItem != null)
            {
                await _cartRepository.RemoveAsync(cartItem);
            }
        }
    }
}
