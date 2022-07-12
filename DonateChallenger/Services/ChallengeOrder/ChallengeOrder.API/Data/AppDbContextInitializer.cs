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
        var merchantIds = new string[] { "V9YL22Q4UQ3PA", "5WSFVT57ASS6U", "MCB5L4ZBENTWC" };
        var orders = new List<ChallengeOrderEntity>();
        for (var i = 0; i < amount; i++)
        {
            orders.Add(new ChallengeOrderEntity
            {
                ChallengeOrderId = Guid.NewGuid(),
                PaymentId = merchantIds[i % 3],
                ChallengesAmount = (i % 3) + 1,
                ResultDonationPrice = 12342
            });
        }

        return orders;
    }
}