using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.API.Services.Abstractions;

namespace Payment.API.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Scope("paypalPayment")]
    [Route(Defaults.DefaultRoute)]
    public class PaypalPaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaypalPaymentController(IPaymentService paymentService) => _paymentService = paymentService;

        [HttpGet]
        public IActionResult PaymentUrl(double unitPrice, string currencyCode, string merchantId)
        {
            var response = _paymentService.CreatePaymentUrl(unitPrice, currencyCode, merchantId);
            return Redirect(response.Response);
        }

        [HttpGet]
        public async Task<IActionResult> Payment(string paymentId, string token, string payerId)
        {
            var response = await _paymentService.ExecutePayment(paymentId, token, payerId);
            if (response.Success)
            {
                return Redirect(response.Response);
            }

            return BadRequest(response.Message);
        }
    }
}
