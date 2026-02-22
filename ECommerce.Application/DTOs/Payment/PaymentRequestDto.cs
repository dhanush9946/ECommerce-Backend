using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Payment
{
    public class PaymentRequestDto
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = null!;

        /// <summary>"success" or "failed" — sent by frontend after Razorpay resolves.</summary>
        public string PaymentStatus { get; set; } = "success";

        public RazorpayPaymentDetailsDto? RazorpayDetails { get; set; }
    }
}
