using Payment.API.Models;

namespace Payment.API.Services.Abstractions
{
    public interface IPaymentService
    {
        PaymentResponse<string> CreatePaymentUrl(double unitPrice, string currencyCode, string returnUrl);
        PaymentResponse<PayPal.Api.Payment> ExecutePayment(string paymentId, string token, string payerId);
    }
}
