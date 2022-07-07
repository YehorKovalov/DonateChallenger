using ChallengeOrder.API.Data;
using ChallengeOrder.API.Models.Responses;
using ChallengeOrder.API.Repositories.Abstractions;
using ChallengeOrder.API.Services.Abstractions;
using Infrastructure.MessageBus.Messages;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;

namespace ChallengeOrder.API.Services;

public class ChallengeOrderService : BaseDataService<AppDbContext>,  IChallengeOrderService
{
    private readonly IChallengeOrderRepository _orderRepository;
    private readonly IRequestClient<CreatePaymentRequestMessage> _paymentCreatingRequestClient;
    private readonly IRequestClient<CreateCatalogChallengeRequestMessage> _catalogChallengeCreatingRequestClient;
    public ChallengeOrderService(
        IDbContextWrapper<AppDbContext> dbContext,
        ILogger<BaseDataService<AppDbContext>> logger,
        IChallengeOrderRepository orderRepository,
        IRequestClient<CreatePaymentRequestMessage> paymentCreatingRequestClient,
        IRequestClient<CreateCatalogChallengeRequestMessage> catalogChallengeCreatingRequestClient)
            : base(dbContext, logger)
    {
        _orderRepository = orderRepository;
        _paymentCreatingRequestClient = paymentCreatingRequestClient;
        _catalogChallengeCreatingRequestClient = catalogChallengeCreatingRequestClient;
    }

    // Will be maintained with payment options
    public async Task<AddChallengeOrderResponse<Guid?>> AddChallengeOrderAsync(string description, decimal donatePrice, string streamerId, string donateFrom, string? title)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var response = new AddChallengeOrderResponse<Guid?>();

            var paymentCreatingResponse = await CreatePayment(); // will be added with Payment.API service
            if (!paymentCreatingResponse.Succeeded)
            {
                response.ErrorMessages = paymentCreatingResponse.ErrorMessages;
                return response;
            }

            var challengeCreatingResponse = await CreateChallenge(description, donatePrice, streamerId, donateFrom, title);
            if (!challengeCreatingResponse.Succeeded)
            {
                response.ErrorMessages = challengeCreatingResponse.ErrorMessages;
                return response;
            }

            var orderId = await _orderRepository.Add(paymentCreatingResponse.PaymentId, challengeCreatingResponse.ChallengeId);

            response.Succeeded = true;
            response.OrderId = orderId;
            return response;
        });
    }

    private async Task<PaymentCreatingResponseMessage> CreatePayment()
    {
        var paymentOptions = new { }; // will be added with Payment.API service
        var response = await _paymentCreatingRequestClient.GetResponse<PaymentCreatingResponseMessage>(paymentOptions);
        return response.Message;
    }

    private async Task<CatalogChallengeCreatingResponseMessage> CreateChallenge(string description, decimal donatePrice, string streamerId, string donateFrom, string? title)
    {
        var challengeOptions = new
        {
            Description = description,
            StreamerId = streamerId,
            DonatePrice = donatePrice,
            DonateFrom = donateFrom,
            Title = title
        };

        var response = await _catalogChallengeCreatingRequestClient.GetResponse<CatalogChallengeCreatingResponseMessage>(challengeOptions);

        return response.Message;
    }
}