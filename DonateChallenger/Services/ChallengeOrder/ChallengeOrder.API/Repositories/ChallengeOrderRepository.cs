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

    public async Task<Guid> Add(Guid paymentId, long catalogChallengeId)
    {
        _logger.LogInformation($"{nameof(Add)} ---> {nameof(paymentId)}: {paymentId}; {nameof(catalogChallengeId)}: {catalogChallengeId}");
        var order = new ChallengeOrderEntity
        {
            ChallengeOrderId = Guid.NewGuid(),
            PaymentId = paymentId,
            CatalogChallengeId = catalogChallengeId
        };

        var result = await _appDbContext.AddAsync(order);
        await _appDbContext.SaveChangesAsync();
        return result.Entity.ChallengeOrderId;
    }
}