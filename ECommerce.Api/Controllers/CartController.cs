using ECommerce.Application.DTOs.Cart;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace ECommerce.Api.Controllers
{
    [ApiController]
    [Route("api/user/cart")]
    [Authorize]
    public class CartController:ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not authenticated");

            return Guid.Parse(userIdClaim.Value);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(AddToCartDto dto)
        {
            await _cartService.AddToCart(GetUserId(), dto);
            return Ok("Product added to cart");
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            return Ok(await _cartService.GetCart(GetUserId()));
        }

        [HttpPut("update-quantity")]
        public async Task<IActionResult> UpdateQuantity(UpdateCartQuantityDto dto)
        {
            await _cartService.UpdateQuantity(GetUserId(), dto);
            return Ok("Quantity updated");
        }

        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> Remove(int productId)
        {
            await _cartService.RemoveFromCart(GetUserId(), productId);
            return Ok("Removed from  cart");
        }
    }
}
