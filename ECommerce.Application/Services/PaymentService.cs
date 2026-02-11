using ECommerce.Application.DTOs.Payment;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepo;
    private readonly IPaymentGateway _gateway;

    public PaymentService(
        IPaymentRepository paymentRepo,
        IPaymentGateway gateway)
    {
        _paymentRepo = paymentRepo;
        _gateway = gateway;
    }

    public async Task<RazorpayOrderResponseDto> CreateOrder(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Invalid amount");

        return await _gateway.CreateOrder(amount);
    }

    public async Task<bool> ProcessPayment(PaymentRequestDto dto)
    {
        if (dto.Method == "Razorpay")
        {
            if (dto.RazorpayDetails == null)
                throw new ArgumentException("Razorpay details are required");


            var isValid = _gateway.VerifyPayment(dto.RazorpayDetails);

            if (!isValid)
                throw new ArgumentException("Payment verification failed");
        }

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = dto.OrderId,
            Amount = dto.Amount,
            Method = dto.Method,
            Status = "Success"
        };

        await _paymentRepo.AddAsync(payment);
        return true;
    }
}
