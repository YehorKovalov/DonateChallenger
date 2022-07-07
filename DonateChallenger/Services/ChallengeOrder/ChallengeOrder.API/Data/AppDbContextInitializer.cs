using ChallengeOrder.API.Data.Entities;
using Infrastructure;
using Infrastructure.Seeding;

namespace ChallengeOrder.API.Data;

public class AppDbContextInitializer : IDbInitializer<AppDbContext>
{
    public async Task Initialize(AppDbContext dbContext)
    {
        var dbIsCreated = await dbContext.Database.EnsureCreatedAsync();
        if (dbIsCreated)
        {
            await dbContext.Database.MigrateAsync();
        }

        if (!await dbContext.ChallengeOrders.AnyAsync())
        {
            var orders = GetChallengeOrders(CatalogChallengeSeedingConstants.ChallengesAmount);
            await dbContext.AddRangeAsync(orders);
            await dbContext.SaveChangesAsync();
        }
    }

    private IEnumerable<ChallengeOrderEntity> GetChallengeOrders(long? amount = 30)
    {
        var orders = new List<ChallengeOrderEntity>();
        for (var i = 0; i < amount; i++)
        {
            orders.Add(new ChallengeOrderEntity
            {
                ChallengeOrderId = Guid.NewGuid(),
                CatalogChallengeId = i,
                PaymentId = Guid.NewGuid() // will be changed after implementing Payment.API
            });
        }

        return orders;
    }
}