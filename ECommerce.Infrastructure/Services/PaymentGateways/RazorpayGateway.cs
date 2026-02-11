

using ECommerce.Application.DTOs.Payment;
using ECommerce.Application.Interfaces;
using ECommerce.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;

namespace ECommerce.Infrastructure.Services.PaymentGateways
{
    public class RazorpayGateway:IPaymentGateway
    {
        private readonly string _key;
        private readonly string _secret;

        public RazorpayGateway(IConfiguration config)
        {
            _key = config["Razorpay:KeyId"]
                   ?? throw new ArgumentNullException("Razorpay:KeyId is missing");

            _secret = config["Razorpay:KeySecret"]
                      ?? throw new ArgumentNullException("Razorpay:KeySecret is missing");
        }



        public Task<RazorpayOrderResponseDto> CreateOrder(decimal amount)
        {
            Console.WriteLine(_key);
            Console.WriteLine(_secret);

            var client = new RazorpayClient(_key, _secret);

            var options = new Dictionary<string, object>
        {
            { "amount", amount * 100 },
            { "currency", "INR" },
            { "receipt", Guid.NewGuid().ToString() }
        };

            var order = client.Order.Create(options);

            return Task.FromResult(new RazorpayOrderResponseDto
            {
                OrderId = order["id"].ToString(),
                Key = _key,
                Amount = amount
            });
        }

        public bool VerifyPayment(RazorpayPaymentDetailsDto dto)
        {
            string payload = dto.RazorpayOrderId + "|" + dto.RazorpayPaymentId;
            var generatedSignature = RazorpaySignatureHelper.GenerateSignature(payload, _secret);
            return generatedSignature == dto.RazorpaySignature;
        }
    }
}
