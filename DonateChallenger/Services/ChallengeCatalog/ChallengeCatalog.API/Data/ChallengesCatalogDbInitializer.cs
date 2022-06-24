using ChallengeCatalog.API.Data.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ChallengeCatalog.API.Data;

public class ChallengesCatalogDbInitializer : IDbInitializer<ChallengeCatalogDbContext>
{
    public async Task Initialize(ChallengeCatalogDbContext dbContext)
    {
        var dbIsCreated = await dbContext.Database.EnsureCreatedAsync();
        if (dbIsCreated)
        {
            await dbContext.Database.MigrateAsync();
        }

        if (!await dbContext.Challenges.AnyAsync())
        {
            var challenges = GetChallenges(100);
            await dbContext.Challenges.AddRangeAsync(challenges);
            await dbContext.SaveChangesAsync();
        }
    }

    private IEnumerable<ChallengeEntity> GetChallenges(int? number = 30)
    {
        var challenges = new List<ChallengeEntity>();

        for (var i = 1; i < number; i++)
        {
            challenges.Add(new ChallengeEntity
            {
                Title = $"Title {i}",
                Description = $"It is a long description of challenge for streamer. Has Id = {i}",
                DonateFrom = $"Donater {i}",
                DonatePrice = i * new Random().Next(1000),
                StreamerId = (i % 3) + 1,
                CreatedTime = DateTime.UtcNow,
                ChallengeStatusEntity = new ChallengeStatusEntity
                {
                    IsCompleted = i % 2 == 0,
                    IsSkipped = false
                }
            });
        }

        return challenges;
    }
}