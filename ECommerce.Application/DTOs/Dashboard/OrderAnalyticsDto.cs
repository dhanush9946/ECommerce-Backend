

namespace ECommerce.Application.DTOs.Dashboard
{
    public class OrderAnalyticsDto
    {
        public int TotalOrders { get; set; }
        public int OrdersToday { get; set; }
        public int PendingOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int CancelledOrders { get; set; }
    }
}
