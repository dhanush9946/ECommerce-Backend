using ECommerce.Application.DTOs.Order;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> PlaceOrder(Guid userId, CheckoutRequestDto dto);
        Task<List<OrderResponseDto>> GetUserOrders(Guid userId);
        Task CancelOrder(Guid userId, Guid orderId);
    }
}
