

namespace ECommerce.Application.DTOs.Order
{
    public class CheckoutRequestDto
    {
        public string Address { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
