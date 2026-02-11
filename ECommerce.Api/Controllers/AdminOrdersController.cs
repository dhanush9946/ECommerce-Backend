using ECommerce.Application.DTOs.Order;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [ApiController]
    [Route("api/admin/orders")]
    [Authorize(Roles = "Admin")]
    public class AdminOrdersController:ControllerBase
    {
        private readonly IAdminOrderService _adminOrderService;
        public AdminOrdersController(IAdminOrderService adminOrderService)
        {
            _adminOrderService = adminOrderService;
        }
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] string? status)
        {
            try
            {
                var orders = await _adminOrderService.GetAllOrders(status);
                return Ok(orders);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            try
            {
                return Ok(await _adminOrderService.GetOrderById(orderId));
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{orderId}/status")]
        public async Task<IActionResult> UpdateStatus(Guid orderId,UpdateOrderStatusDto dto)
        {
            try
            {
                await _adminOrderService.UpdateOrderStatus(orderId, dto.Status);
                return Ok("Order status updated");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}
