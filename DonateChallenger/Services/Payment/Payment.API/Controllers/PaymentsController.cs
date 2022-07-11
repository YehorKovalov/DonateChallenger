using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Payment.API.Services.Abstractions;

namespace Payment.API.Controllers
{
    [Route(Defaults.DefaultRoute)]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private const string PaymentReturnUrl = "http://donate-challenger.com:4003/api/v1/payments/payment";
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService) => _paymentService = paymentService;

        [HttpGet]
        public IActionResult PaymentUrl(double unitPrice, string currencyCode)
        {
            var response = _paymentService.CreatePaymentUrl(unitPrice, currencyCode, PaymentReturnUrl);
            return Redirect(response.Response);
        }

        [HttpGet]
        public IActionResult Payment(string paymentId, string token, string payerId)
        {
            var response = _paymentService.ExecutePayment(paymentId, token, payerId);
            return Ok(response);
        }
    }
}
