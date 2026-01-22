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
        public async Task<IActionResult> GetAllOrders()
        {
            return Ok(await _adminOrderService.GetAllOrders());
        }
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            return Ok(await _adminOrderService.GetOrderById(orderId));
        }

        [HttpPatch("{orderId}/status")]
        public async Task<IActionResult> UpdateStatus(Guid orderId,UpdateOrderStatusDto dto)
        {
            await _adminOrderService.UpdateOrderStatus(orderId, dto.Status);
            return Ok("Order status updated");
        }
    }
}
