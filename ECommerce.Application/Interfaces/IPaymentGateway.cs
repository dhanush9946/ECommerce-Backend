using ECommerce.Application.DTOs.Payment;

namespace ECommerce.Application.Interfaces
{
    public interface IPaymentGateway
    {
        Task<RazorpayOrderResponseDto> CreateOrder(decimal amount);
        bool VerifyPayment(RazorpayPaymentDetailsDto dto);
    }
}
