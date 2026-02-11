
using ECommerce.Domain.Entities;
using ECommerce.Application.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ECommerce.Application.DTOs.Dashboard;

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




        public async Task<List<Order>> GetAllAsync(OrderStatus? status)
        {
            var query = _context.Orders
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            return await query
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

        public async Task<List<DailySalesDto>> GetDailySalesAsync(DateTime startDate)
        {
            var successStatuses = new[]
            {
        OrderStatus.Confirmed,
        OrderStatus.Delivered,
        OrderStatus.Shipped
    };

            return await _context.Orders
                .Where(o =>
                    successStatuses.Contains(o.Status) &&
                    o.CreatedAt >= startDate)
                .GroupBy(o => o.CreatedAt.Date)
                .Select(g => new DailySalesDto
                {
                    Date = g.Key,
                    TotalSales = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(x => x.Date)
                .ToListAsync();
        }


        public async Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync()
        {
            var successStatuses = new[]
           {
        OrderStatus.Confirmed,
        OrderStatus.Delivered,
        OrderStatus.Shipped
    };
            return await _context.Orders
                //.Where(o => o.Status == OrderStatus.Confirmed)
                .Where(o=>successStatuses.Contains(o.Status))
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .Select(g => new MonthlyRevenueDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();
        }


    }
}
