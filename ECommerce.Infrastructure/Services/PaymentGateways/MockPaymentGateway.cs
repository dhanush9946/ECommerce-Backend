using ECommerce.Application.DTOs.Payment;
using ECommerce.Application.Interfaces;

public class MockPaymentGateway : IPaymentGateway
{
    public Task<RazorpayOrderResponseDto> CreateOrder(decimal amount)
    {
        // Fake order for frontend testing
        return Task.FromResult(new RazorpayOrderResponseDto
        {
            OrderId = "mock_order_123",
            Key = "mock_key",
            Amount = amount
        });
    }

    public bool VerifyPayment(RazorpayPaymentDetailsDto dto)
    {
        // Always success
        return true;
    }
}
