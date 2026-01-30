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
            try
            {
                var result = await _adminDashboardService.GetDashboardAnalyticsAsync();
                return Ok(result);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpGet("daily-sales")]
        public async Task<IActionResult> GetDailySales([FromQuery] int days = 7)
        {
            try
            {
                return Ok(await _adminDashboardService.GetDailySalesAsync(days));
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("monthly-revenue")]
        public async Task<IActionResult> GetMonthlyRevenue()
        {
            try
            {
                return Ok(await _adminDashboardService.GetMonthlyRevenueAsync());
            }
            catch(UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpGet("user-growth")]
        public async Task<IActionResult> GetUserGrowth([FromQuery] int days = 30)
        {
            try
            {
                return Ok(await _adminDashboardService.GetUserGrowthAsync(days));
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
