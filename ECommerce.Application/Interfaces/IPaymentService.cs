

using ECommerce.Application.DTOs.Payment;

namespace ECommerce.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<RazorpayOrderResponseDto> CreateOrder(decimal amount);
        Task<bool> ProcessPayment(PaymentRequestDto dto);
    }
}
