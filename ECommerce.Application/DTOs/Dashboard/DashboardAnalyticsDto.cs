using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Dashboard
{
    public class DashboardAnalyticsDto
    {
        public UserAnalyticsDto Users { get; set; } = new();
        public ProductAnalyticsDto Products { get; set; } = new();
        public OrderAnalyticsDto Orders { get; set; } = new();
        public RevenueAnalyticsDto Revenue { get; set; } = new();
    }
}
