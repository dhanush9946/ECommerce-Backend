using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [ApiController]
    [Route("api/admin/dashboard")]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController:ControllerBase
    {
        private readonly IAdminDashboardService _adminDashboardService;

        public AdminDashboardController(IAdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;
        }
        [HttpGet("analytics")]
        public async Task<IActionResult> GetDashboardAnalytics()
        {
            var result = await _adminDashboardService.GetDashboardAnalyticsAsync();
            return Ok(result);
        }

        [HttpGet("daily-sales")]
        public async Task<IActionResult> GetDailySales([FromQuery] int days = 7)
        {
            return Ok(await _adminDashboardService.GetDailySalesAsync(days));
        }

        [HttpGet("monthly-revenue")]
        public async Task<IActionResult> GetMonthlyRevenue()
        {
            return Ok(await _adminDashboardService.GetMonthlyRevenueAsync());
        }

        [HttpGet("user-growth")]
        public async Task<IActionResult> GetUserGrowth([FromQuery] int days = 30)
        {
            return Ok(await _adminDashboardService.GetUserGrowthAsync(days));
        }
    }
}
