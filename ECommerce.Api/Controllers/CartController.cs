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
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddToCart(AddToCartDto dto)
        {
            try
            {
                await _cartService.AddToCart(GetUserId(), dto);
                return Ok("Product added to cart");
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                return Ok(await _cartService.GetCart(GetUserId()));
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPut("update-quantity")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateQuantity(UpdateCartQuantityDto dto)
        {
            try
            {
                await _cartService.UpdateQuantity(GetUserId(), dto);
                return Ok("Quantity updated");
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpDelete("remove/{productId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Remove(int productId)
        {
            try
            {
                await _cartService.RemoveFromCart(GetUserId(), productId);
                return Ok("Removed from  cart");
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
    }
}
