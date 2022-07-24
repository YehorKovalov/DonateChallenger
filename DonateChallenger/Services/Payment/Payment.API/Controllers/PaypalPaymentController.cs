using System.Net;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Payment.API.Models;
using Payment.API.Services.Abstractions;

namespace Payment.API.Controllers
{
    [ApiController]
    [Scope("paypal-payment.bff")]
    [Route(Defaults.DefaultRoute)]
    public class PaypalPaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaypalPaymentController(IPaymentService paymentService) => _paymentService = paymentService;

        [HttpGet]
        [ProducesResponseType(typeof(GetPayPalPaymentUrlResponse), (int)HttpStatusCode.OK)]
        public IActionResult PaymentUrl(double unitPrice, string currencyCode, string merchantId)
        {
            var response = _paymentService.CreatePaymentUrl(unitPrice, currencyCode, merchantId);
            return Ok(response);
        }
    }
}
