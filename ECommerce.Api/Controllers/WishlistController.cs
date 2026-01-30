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
            try
            {
                await _wishlistService.AddToWishlist(GetUserId(), dto);
                return Ok("Product added to wishlist");
            }


            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch(ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetWishlist()
        {
            try
            {
                var result = await _wishlistService.GetWishlist(GetUserId());
                return Ok(result);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RemoveFromWishlist(int productId)
        {
            try
            {

                await _wishlistService.RemoveFromWishlist(GetUserId(), productId);
                return Ok("Product removed from wishlist");
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }



        [HttpPost("move-to-cart/{productId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> MoveToCart(int productId)
        {
            try
            {
                await _wishlistService.MoveToCart(GetUserId(), productId);
                return Ok("Product moved to cart successfully");
            }
            catch(ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

    }
}
