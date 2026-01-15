using ECommerce.Application.DTOs.Order;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> Checkout(Guid userId, CheckoutRequestDto dto);
        Task<List<OrderResponseDto>> GetUserOrders(Guid userId);
    }
}
