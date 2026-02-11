using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Payment
{
    public class RazorpayOrderResponseDto
    {
        public string OrderId { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
