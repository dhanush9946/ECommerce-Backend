

using ECommerce.Application.DTOs.Order;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IAdminOrderService
    {
        Task<List<AdminOrderResponseDto>> GetAllOrders();
        Task<AdminOrderResponseDto> GetOrderById(Guid orderId);
        Task UpdateOrderStatus(Guid orderId, string Status);
    }
}
