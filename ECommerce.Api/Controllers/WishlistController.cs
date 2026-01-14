using ECommerce.Application.DTOs.Wishlist;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/user/wishlist")]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not authenticated");

            return Guid.Parse(userIdClaim.Value);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddToWishlist(AddToWishlistDto dto)
        {
            await _wishlistService.AddToWishlist(GetUserId(), dto);
            return Ok("Product added to wishlist");
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetWishlist()
        {
            var result = await _wishlistService.GetWishlist(GetUserId());
            return Ok(result);
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RemoveFromWishlist(int productId)
        {
            await _wishlistService.RemoveFromWishlist(GetUserId(), productId);
            return Ok("Product removed from wishlist");
        }



        [HttpPost("move-to-cart/{productId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> MoveToCart(int productId)
        {
            await _wishlistService.MoveToCart(GetUserId(), productId);
            return Ok("Product moved to cart successfully");
        }

    }
}
