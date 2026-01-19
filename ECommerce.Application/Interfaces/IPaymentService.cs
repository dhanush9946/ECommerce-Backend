

using ECommerce.Application.DTOs.Payment;

namespace ECommerce.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ProcessPayment(PaymentRequestDto dto);
    }
}
