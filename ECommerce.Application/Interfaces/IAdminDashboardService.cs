

using ECommerce.Application.DTOs.Dashboard;

namespace ECommerce.Application.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<DashboardAnalyticsDto> GetDashboardAnalyticsAsync();
    }
}
