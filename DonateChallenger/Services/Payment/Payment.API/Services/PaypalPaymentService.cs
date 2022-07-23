using Infrastructure.MessageBus.Messages;
using Microsoft.Extensions.Options;
using Payment.API.Configurations;
using Payment.API.Models;
using Payment.API.Services.Abstractions;

namespace Payment.API.Services
{
    public class PaypalPaymentService : IPaymentService
    {
        private const string PaypalPaymentEndpoint = "http://donate-challenger.com:4003/api/v1/paypalpaymentexecute/payment";
        private const string GlobalUrl = "http://donate-challenger.com";
        private readonly PaypalConfiguration _paypalConfiguration;
        private readonly ILogger<PaypalPaymentService> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public PaypalPaymentService(
            IOptions<PaypalConfiguration> appSettings,
            ILogger<PaypalPaymentService> logger,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _paypalConfiguration = appSettings.Value;
        }

        public GetPayPalPaymentUrlResponse CreatePaymentUrl(double unitPrice, string currencyCode, string merchantId, string? returnUrl = null)
        {
            var response = new GetPayPalPaymentUrlResponse();
            try
            {
                _logger.LogInformation($"{nameof(CreatePaymentUrl)} ---> {nameof(unitPrice)}: {unitPrice}; {nameof(currencyCode)}: {currencyCode}; {nameof(merchantId)}: {merchantId}; {nameof(returnUrl)}: {returnUrl}; ");
                var apiContext = GetAPIContext(_paypalConfiguration.ClientId, _paypalConfiguration.ClientSecret);
                var processedPayment = new PayPal.Api.Payment
                {
                    intent = "sale",
                    payer = new Payer { payment_method = "paypal" },
                    transactions = BuildTransactionList(currencyCode, merchantId, unitPrice),
                    redirect_urls = BuildRedirectUrls(returnUrl ?? PaypalPaymentEndpoint)
                };

                var createdPayment = processedPayment.Create(apiContext);
                var link = createdPayment.links.Single(l => l.rel.ToLower().Trim().Equals("approval_url"));

                response.Url = link.href;
            }
            catch
            {
                _logger.LogError($"{nameof(CreatePaymentUrl)} ---> Error while generating payment url, please retry.");
            }

            _logger.LogInformation($"{nameof(CreatePaymentUrl)} ---> Url = {response.Url}");
            return response;
        }

        public async Task<PaymentResponse<string>> ExecutePayment(string paymentId, string token, string payerId)
        {
            var serviceResponse = new PaymentResponse<string>();
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
                    serviceResponse.Response = GlobalUrl;
                }
                else
                {
                    serviceResponse.Message = $"Payment {executedPayment!.state}.";
                    serviceResponse.Success = false;
                }
            }
            catch (Exception error)
            {
                serviceResponse.Message = "Error while making payment, please retry.";
                serviceResponse.Error = error;
                serviceResponse.Success = false;
            }

            await PublishPaymentStatus(serviceResponse.Success, paymentId);
            return serviceResponse;
        }

        private async Task PublishPaymentStatus(bool succeeded, string paymentId)
        {
            _logger.LogInformation($"{nameof(PublishPaymentStatus)} ---> PaymentStatus published with {succeeded}");
            await _publishEndpoint.Publish<MessagePaymentStatus>(new
            {
                Succeeded = succeeded,
                PaymentId = paymentId
            });
        }

        private List<Transaction> BuildTransactionList(string currencyCode, string merchantId, double unitPrice)
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
                    amount = paypalAmount,
                    payee = new Payee { merchant_id = merchantId }
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
