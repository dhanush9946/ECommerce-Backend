using ECommerce.Application.DTOs.Payment;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> Pay(PaymentRequestDto dto)
    {
        try
        {
            var success = await _paymentService.ProcessPayment(dto);

            if (!success)
                return BadRequest("Payment failed");

            return Ok("Payment successful");
        }
        catch(UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);

        }
        catch(ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
