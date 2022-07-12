using ChallengeOrder.API.Data;
using ChallengeOrder.API.Data.Entities;
using ChallengeOrder.API.Repositories.Abstractions;
using Infrastructure.Services.Abstractions;

namespace ChallengeOrder.API.Repositories;

public class ChallengeOrderRepository : IChallengeOrderRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<ChallengeOrderRepository> _logger;

    public ChallengeOrderRepository(
        IDbContextWrapper<AppDbContext> appDbContext,
        ILogger<ChallengeOrderRepository> logger)
    {
        _logger = logger;
        _appDbContext = appDbContext.DbContext;
    }

    public async Task<Guid> Add(string paymentId, int challengesAmount, double resultDonationPrice)
    {
        _logger.LogInformation($"{nameof(Add)} ---> {nameof(paymentId)}: {paymentId};");
        var order = new ChallengeOrderEntity
        {
            ChallengeOrderId = Guid.NewGuid(),
            PaymentId = paymentId,
            ChallengesAmount = challengesAmount,
            ResultDonationPrice = resultDonationPrice
        };

        var result = await _appDbContext.AddAsync(order);
        await _appDbContext.SaveChangesAsync();
        return result.Entity.ChallengeOrderId;
    }
}