using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Payment
{
    public class RazorpayPaymentDetailsDto
    {
        public string RazorpayOrderId { get; set; } = null!;
        public string RazorpayPaymentId { get; set; } = null!;
        public string RazorpaySignature { get; set; } = null!;
    }
}
