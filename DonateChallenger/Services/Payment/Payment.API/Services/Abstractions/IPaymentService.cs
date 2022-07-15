using Payment.API.Models;

namespace Payment.API.Services.Abstractions
{
    public interface IPaymentService
    {
        GetPayPalPaymentUrlResponse CreatePaymentUrl(double unitPrice, string currencyCode, string merchantId, string? returnUrl = null);
        Task<PaymentResponse<string>> ExecutePayment(string paymentId, string token, string payerId);
    }
}
