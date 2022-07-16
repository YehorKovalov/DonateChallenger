using ChallengeOrder.API.Data.Entities;
using Infrastructure;
using Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;

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

    private IEnumerable<ChallengeOrderEntity> GetChallengeOrders(long? challengesAmount = 30)
    {
        var random = new Random();
        var merchantIds = new string[] { "V9YL22Q4UQ3PA", "5WSFVT57ASS6U", "MCB5L4ZBENTWC" };
        var orders = new List<ChallengeOrderEntity>();
        do
        {
            var orderChallengesAmount = random.Next(1, 4);
            orders.Add(new ChallengeOrderEntity
            {
                ChallengeOrderId = Guid.NewGuid(),
                PaymentId = merchantIds[random.Next(merchantIds.Length - 1)],
                ChallengesAmount = orderChallengesAmount,
                ResultDonationPrice = random.Next(20, 100)
            });
            challengesAmount -= orderChallengesAmount;
        }
        while (challengesAmount > 0);

        return orders;
    }
}