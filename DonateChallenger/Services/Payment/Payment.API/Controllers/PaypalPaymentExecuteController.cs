using System.Net;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.API.Models;
using Payment.API.Services.Abstractions;

namespace Payment.API.Controllers
{
    [ApiController]
    [Route(Defaults.DefaultRoute)]
    public class PaypalPaymentExecuteController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaypalPaymentExecuteController(IPaymentService paymentService) => _paymentService = paymentService;

        [HttpGet]
        [ProducesResponseType(typeof(PaymentResponse<string>), (int)HttpStatusCode.OK)]
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