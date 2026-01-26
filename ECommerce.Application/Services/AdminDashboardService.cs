

using ECommerce.Application.DTOs.Dashboard;
using ECommerce.Application.Interfaces;

namespace ECommerce.Application.Services
{
    public class AdminDashboardService:IAdminDashboardService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public AdminDashboardService(IUserRepository userRepository,
                                     IProductRepository productRepository,
                                     IOrderRepository orderRepository)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public async Task<DashboardAnalyticsDto> GetDashboardAnalyticsAsync()
        {
            var dashboard = new DashboardAnalyticsDto();

            dashboard.Users = new UserAnalyticsDto
            {
                TotalUsers = await _userRepository.GetTotalUsersAsync(),
                ActiveUsers = await _userRepository.GetActiveUsersAsync(),
                BlockedUsers = await _userRepository.GetBlockedUsersAsync(),
                NewUsersToday = await _userRepository.GetNewUsersTodayAsync()
            };

            dashboard.Products = new ProductAnalyticsDto
            {
                TotalProducts = await _productRepository.GetTotalProductsAsync(),
                ActiveProducts = await _productRepository.GetActiveProductsAsync(),
                OutOfStockProducts = await _productRepository.GetOutOfStockProductsAsync(),
                LowStockProducts = await _productRepository.GetLowStockProductsAsync()
            };

            dashboard.Orders = new OrderAnalyticsDto
            {
                TotalOrders = await _orderRepository.GetTotalOrdersAsync(),
                OrdersToday = await _orderRepository.GetOrdersTodayAsync(),
                PendingOrders = await _orderRepository.GetPendingOrdersAsync(),
                DeliveredOrders = await _orderRepository.GetDeliveredOrdersAsync(),
                CancelledOrders = await _orderRepository.GetCancelledOrdersAsync()
            };

            dashboard.Revenue = new RevenueAnalyticsDto
            {
                TotalRevenue = await _orderRepository.GetTotalRevenueAsync(),
                TodayRevenue = await _orderRepository.GetTodayRevenueAsync()
            };

            return dashboard;

        }

        public async Task<List<DailySalesDto>> GetDailySalesAsync(int days = 7)
        {
            var startDate = DateTime.UtcNow.Date.AddDays(-days + 1);
            return await _orderRepository.GetDailySalesAsync(startDate);
        }

        public async Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync()
        {
            return await _orderRepository.GetMonthlyRevenueAsync();
        }

        public async Task<List<UserGrowthDto>> GetUserGrowthAsync(int days = 30)
        {
            var startDate = DateTime.UtcNow.Date.AddDays(-days + 1);
            return await _userRepository.GetUserGrowthAsync(startDate);
        }

    }
}
