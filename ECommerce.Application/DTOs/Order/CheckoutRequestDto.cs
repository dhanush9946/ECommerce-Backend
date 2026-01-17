

namespace ECommerce.Application.DTOs.Order
{
    public class CheckoutRequestDto
    {
        public string ShippingAddress { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
