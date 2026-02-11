

using ECommerce.Application.DTOs.Dashboard;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<List<Order>> GetUserOrders(Guid userId);
        Task<Order?> GetByIdAsync(Guid orderId);
        Task UpdateAsync(Order order);


        Task<List<Order>> GetAllAsync(OrderStatus? status);

        Task<Order?> GetOrderWithItemsAsync(Guid orderId);


        //Dashboard
        Task<int> GetTotalOrdersAsync();
        Task<int> GetOrdersTodayAsync();
        Task<int> GetPendingOrdersAsync();
        Task<int> GetDeliveredOrdersAsync();
        Task<int> GetCancelledOrdersAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<decimal> GetTodayRevenueAsync();

        Task<List<DailySalesDto>> GetDailySalesAsync(DateTime startDate);
        Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync();
    }
}
