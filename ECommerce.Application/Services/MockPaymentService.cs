using ECommerce.Application.DTOs.Payment;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

public class MockPaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepo;

    public MockPaymentService(IPaymentRepository paymentRepo)
    {
        _paymentRepo = paymentRepo;
    }

    public async Task<bool> ProcessPayment(PaymentRequestDto dto)
    {
        
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = dto.OrderId,
            Amount = dto.Amount,
            Method = dto.Method,
            Status = "Success" // Always success in mock
        };

        await _paymentRepo.AddAsync(payment);

        return true;
    }
}
