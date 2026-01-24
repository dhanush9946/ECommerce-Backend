
using ECommerce.Domain.Entities;
using ECommerce.Application.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class OrderRepository:IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Order>> GetUserOrders(Guid userId)
        {
           return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
        public async Task<Order?> GetByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }




        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderWithItemsAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        //Dashboard

        public async Task<int> GetTotalOrdersAsync()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<int> GetOrdersTodayAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.Orders.CountAsync(o => o.CreatedAt >= today);
        }

        public async Task<int> GetPendingOrdersAsync()
        {
            return await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending);
        }

        public async Task<int> GetDeliveredOrdersAsync()
        {
            return await _context.Orders.CountAsync(o => o.Status == OrderStatus.Delivered);
        }

        public async Task<int> GetCancelledOrdersAsync()
        {
            return await _context.Orders.CountAsync(o => o.Status == OrderStatus.Cancelled);
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Orders
                .Where(o => o.Status == OrderStatus.Confirmed)
                .SumAsync(o => o.TotalAmount);
        }

        public async Task<decimal> GetTodayRevenueAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.Orders
                .Where(o => o.Status == OrderStatus.Confirmed && o.CreatedAt >= today)
                .SumAsync(o => o.TotalAmount);
        }


    }
}
