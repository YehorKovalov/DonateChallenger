using ChallengeOrder.API.Data;
using ChallengeOrder.API.Data.Entities;
using ChallengeOrder.API.Repositories.Abstractions;
using Infrastructure.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

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
        _logger.LogInformation($"{nameof(Add)} ---> {nameof(paymentId)}: {paymentId}; {nameof(challengesAmount)}: {challengesAmount}; {nameof(resultDonationPrice)}: {resultDonationPrice}; ");
        var order = new ChallengeOrderEntity
        {
            ChallengeOrderId = Guid.NewGuid(),
            PaymentId = paymentId,
            ChallengesAmount = challengesAmount,
            ResultDonationPrice = resultDonationPrice,
            Date = DateTime.UtcNow
        };

        var result = await _appDbContext.AddAsync(order);

        await _appDbContext.SaveChangesAsync();

        return result.Entity.ChallengeOrderId;
    }

    public async Task<Guid> Update(string challengeOrderId, string paymentId, int challengesAmount, double resultDonationPrice)
    {
        _logger.LogInformation($"{nameof(Update)} ---> {nameof(challengeOrderId)}: {challengeOrderId}; {nameof(paymentId)}: {paymentId}; {nameof(challengesAmount)}: {challengesAmount}; {nameof(resultDonationPrice)}: {resultDonationPrice}; ");
        var order = new ChallengeOrderEntity
        {
            ChallengeOrderId = Guid.Parse(challengeOrderId),
            PaymentId = paymentId,
            ChallengesAmount = challengesAmount,
            ResultDonationPrice = resultDonationPrice,
        };

        var result = _appDbContext.Update(order);

        await _appDbContext.SaveChangesAsync();

        return result.Entity.ChallengeOrderId;
    }

    public async Task<ChallengeOrderEntity?> GetById(string orderId)
    {
        _logger.LogInformation($"{nameof(GetById)} ---> {nameof(orderId)}: {orderId}");
        var orderIdGuid = Guid.Parse(orderId);
        var order = await _appDbContext.ChallengeOrders.FirstOrDefaultAsync(f => f.ChallengeOrderId == orderIdGuid);

        if (order == null)
        {
            _logger.LogError($"{nameof(GetById)} ---> Order doesn't exist");
        }

        return order;
    }

    public async Task<PaginatedOrders> GetPaginatedOrders(int currentPage, int ordersPerPage)
    {
        _logger.LogInformation($"{nameof(GetPaginatedOrders)} ---> {nameof(currentPage)}: {currentPage}; {nameof(ordersPerPage)}: {ordersPerPage};");

        var query = _appDbContext.ChallengeOrders.AsQueryable();

        var ordersAmount = await query.LongCountAsync();

        if (ordersAmount == 0)
        {
            _logger.LogInformation($"{nameof(GetPaginatedOrders)} ---> List is empty");
            return new PaginatedOrders
            {
                Challenges = Enumerable.Empty<ChallengeOrderEntity>(),
                TotalCount = 0
            };
        }

        var orders = await _appDbContext.ChallengeOrders
            .Skip(currentPage * ordersPerPage)
            .Take(ordersPerPage)
            .ToListAsync();

        return new PaginatedOrders
        {
            Challenges = orders,
            TotalCount = ordersAmount
        };
    }
}