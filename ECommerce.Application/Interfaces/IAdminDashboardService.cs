

using ECommerce.Application.DTOs.Dashboard;

namespace ECommerce.Application.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<DashboardAnalyticsDto> GetDashboardAnalyticsAsync();
        Task<List<DailySalesDto>> GetDailySalesAsync(int days = 7);
        Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync();

        Task<List<UserGrowthDto>> GetUserGrowthAsync(int days = 30);
    }
}
