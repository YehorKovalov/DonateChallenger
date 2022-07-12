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
            var result = await _orderRepository.Add(paymentId, challengesAmount, resultDonationPrice);
            return new AddChallengeOrderResponse<Guid?>
            {
                OrderId = result,
                Succeeded = true
            };
        });
    }
}