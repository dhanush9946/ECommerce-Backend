

using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<List<Order>> GetUserOrders(Guid userId);
        Task<Order?> GetByIdAsync(Guid orderId);
        Task UpdateAsync(Order order);


        Task<List<Order>> GetAllAsync();
        Task<Order?> GetOrderWithItemsAsync(Guid orderId);
    }
}
