

using ECommerce.Application.DTOs.Order;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrdersController:ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private Guid UserId
        {
            get
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
                    throw new UnauthorizedAccessException("Invalid or missing user id claim");

                return userId;
            }
        }


        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(CheckoutRequestDto dto)
        {
            try
            {
                var orderId = await _orderService.PlaceOrder(UserId, dto);
                return Ok(new { OrderId = orderId });
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
        public async Task<IActionResult> MyOrders()
        {
            try
            {
                var orders = await _orderService.GetUserOrders(UserId);
                return Ok(orders);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        [HttpPut("{orderId}/cancel")]
        public async Task<IActionResult> Cancel(Guid orderId)
        {
            try
            {
                await _orderService.CancelOrder(UserId, orderId);
                return Ok("Order cancelled successfully");
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(InvalidOperationException ex)
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
