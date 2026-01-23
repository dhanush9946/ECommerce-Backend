

namespace ECommerce.Application.DTOs.Dashboard
{
    public class ProductAnalyticsDto
    {
        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public int LowStockProducts { get; set; }
    }
}
