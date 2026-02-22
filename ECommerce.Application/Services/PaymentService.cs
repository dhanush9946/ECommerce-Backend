using ECommerce.Application.DTOs.Payment;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepo;
    private readonly IPaymentGateway _gateway;
    private readonly IOrderRepository _orderRepo;
    private readonly ICartRepository _cartRepo;

    public PaymentService(
        IPaymentRepository paymentRepo,
        IPaymentGateway gateway,
        IOrderRepository orderRepo,
        ICartRepository cartRepo)
    {
        _paymentRepo = paymentRepo;
        _gateway = gateway;
        _orderRepo = orderRepo;
        _cartRepo = cartRepo;
    }

    public async Task<RazorpayOrderResponseDto> CreateOrder(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Invalid amount");

        return await _gateway.CreateOrder(amount);
    }

    public async Task<bool> ProcessPayment(PaymentRequestDto dto)
    {
        // --- 1. For Razorpay: verify signature only on success ---
        if (dto.Method == "Razorpay" && dto.PaymentStatus == "success")
        {
            if (dto.RazorpayDetails == null)
                throw new ArgumentException("Razorpay details are required");

            var isValid = _gateway.VerifyPayment(dto.RazorpayDetails);
            if (!isValid)
                throw new ArgumentException("Payment signature verification failed");
        }

        // --- 2. Record payment with whatever status came from frontend ---
        var normalizedStatus = dto.PaymentStatus?.ToLower() == "success" ? "Success" : "Failed";

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = dto.OrderId,
            Amount = dto.Amount,
            Method = dto.Method,
            Status = normalizedStatus
        };

        await _paymentRepo.AddAsync(payment);

        // --- 3. On success: update order to Placed and clear cart ---
        if (normalizedStatus == "Success")
        {
            var order = await _orderRepo.GetOrderWithItemsAsync(dto.OrderId);

            if (order != null)
            {
                // Mark order as Placed
                order.Status = OrderStatus.Placed;
                await _orderRepo.UpdateAsync(order);

                // Clear the user's cart
                var cartItems = await _cartRepo.GetUserCart(order.UserId);
                foreach (var item in cartItems)
                {
                    await _cartRepo.RemoveAsync(item);
                }
            }
        }

        return normalizedStatus == "Success";
    }
}
