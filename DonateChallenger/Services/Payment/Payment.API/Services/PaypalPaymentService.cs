using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Payment.API.Configurations;
using Payment.API.Models;
using Payment.API.Services.Abstractions;

namespace Payment.API.Services
{
    public class PaypalPaymentService : IPaymentService
    {
        private readonly PaypalConfiguration _paypalConfiguration;
        private readonly ILogger<PaypalPaymentService> _logger;

        public PaypalPaymentService(IOptions<PaypalConfiguration> appSettings, ILogger<PaypalPaymentService> logger)
        {
            _logger = logger;
            _paypalConfiguration = appSettings.Value;
        }

        public PaymentResponse<string> CreatePaymentUrl(double unitPrice, string currencyCode, string returnUrl)
        {
            var serviceResponse = new PaymentResponse<string>();
            try
            {
                var apiContext = GetAPIContext(_paypalConfiguration.ClientId, _paypalConfiguration.ClientSecret);
                var processedPayment = new PayPal.Api.Payment
                {
                    intent = "sale",
                    payer = new Payer { payment_method = "paypal" },
                    transactions = BuildTransactionList(currencyCode, unitPrice),
                    redirect_urls = BuildRedirectUrls(returnUrl)
                };

                var createdPayment = processedPayment.Create(apiContext);
                var link = createdPayment.links.Single(l => l.rel.ToLower().Trim().Equals("approval_url"));

                serviceResponse.Message = "Success";
                serviceResponse.Success = true;
                serviceResponse.Response = link.href;
            }
            catch (Exception error)
            {
                serviceResponse.Message = "Error while generating payment url, please retry.";
                serviceResponse.Error = error;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public PaymentResponse<PayPal.Api.Payment> ExecutePayment(string paymentId, string token, string payerId)
        {
            var serviceResponse = new PaymentResponse<PayPal.Api.Payment>();
            try
            {
                var apiContext = GetAPIContext(_paypalConfiguration.ClientId, _paypalConfiguration.ClientSecret);

                var paymentExecution = new PaymentExecution { payer_id = payerId };
                var payment = new PayPal.Api.Payment { id = paymentId };
                var executedPayment = payment.Execute(apiContext, paymentExecution);

                if (executedPayment != null && executedPayment.state.ToLower().Equals("approved"))
                {
                    serviceResponse.Message = $"Payment {executedPayment.id} approved.";
                    serviceResponse.Success = true;
                    serviceResponse.Response = executedPayment;
                }
                else
                {
                    serviceResponse.Message = $"Payment {executedPayment.state}.";
                    serviceResponse.Success = false;
                    serviceResponse.Response = executedPayment;
                }
            }
            catch (Exception error)
            {
                serviceResponse.Message = "Error while making payment, please retry.";
                serviceResponse.Error = error;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        private List<Transaction> BuildTransactionList(string currencyCode, double unitPrice)
        {
            var paypalAmount = new Amount
            {
                currency = currencyCode,
                total = unitPrice.ToString()
            };

            return new List<Transaction>
            {
                new Transaction
                {
                    invoice_number = $"{Guid.NewGuid()}",
                    amount = paypalAmount
                }
            };
        }

        private RedirectUrls BuildRedirectUrls(string returnUrl) => new RedirectUrls
            {
                cancel_url = returnUrl + "?cancel=true",
                return_url = returnUrl
            };

        private APIContext GetAPIContext(string clientId, string clientSecret)
        {
            var config = ConfigManager.Instance.GetProperties();
            var tokenCredential = new OAuthTokenCredential(clientId, clientSecret, config);
            var token = tokenCredential.GetAccessToken();

            return new APIContext
            {
                AccessToken = token,
                Config = config
            };
        }
    }
}
