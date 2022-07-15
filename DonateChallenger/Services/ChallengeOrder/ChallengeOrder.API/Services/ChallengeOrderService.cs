using ChallengeOrder.API.Data;
using ChallengeOrder.API.Models.Responses;
using ChallengeOrder.API.Repositories.Abstractions;
using ChallengeOrder.API.Services.Abstractions;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;

namespace ChallengeOrder.API.Services;

public class ChallengeOrderService : BaseDataService<AppDbContext>,  IChallengeOrderService
{
    private readonly IChallengeOrderRepository _orderRepository;
    public ChallengeOrderService(
        IDbContextWrapper<AppDbContext> dbContext,
        ILogger<BaseDataService<AppDbContext>> logger,
        IChallengeOrderRepository orderRepository)
            : base(dbContext, logger)
    {
        _orderRepository = orderRepository;
    }

    public async Task<AddChallengeOrderResponse<Guid?>> AddChallengeOrderAsync(string paymentId, int challengesAmount, double resultDonationPrice)
    {
        return await ExecuteSafeAsync(async () =>
        {
            if (!AddChallengeOrderStateIsValid(paymentId, challengesAmount, resultDonationPrice))
            {
                var errorMessage = "Add Challenge Order State Is Valid";
                Logger.LogError($"{nameof(AddChallengeOrderAsync)} ---> {errorMessage}");
                return new AddChallengeOrderResponse<Guid?>
                {
                    ErrorMessage = errorMessage,
                    Succeeded = false,
                    OrderId = null
                };
            }

            var orderId = await _orderRepository.Add(paymentId, challengesAmount, resultDonationPrice);
            Logger.LogInformation($"{nameof(AddChallengeOrderAsync)} ---> {nameof(orderId)}: {orderId}");
            return new AddChallengeOrderResponse<Guid?>
            {
                OrderId = orderId,
                Succeeded = true
            };
        });
    }

    private bool AddChallengeOrderStateIsValid(string paymentId, int challengesAmount, double resultDonationPrice)
    {
        const double logicalMinimumDonationPriceValue = 0.1;
        return !string.IsNullOrWhiteSpace(paymentId)
               && challengesAmount > 0
               && resultDonationPrice > logicalMinimumDonationPriceValue;
    }
}