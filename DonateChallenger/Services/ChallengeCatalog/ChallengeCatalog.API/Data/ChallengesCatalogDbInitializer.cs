using ChallengeCatalog.API.Data.Entities;
using Infrastructure;
using Infrastructure.Seeding;
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
            var challenges = GetChallenges(CatalogChallengeSeedingConstants.ChallengesAmount);
            await dbContext.Challenges.AddRangeAsync(challenges);
            await dbContext.SaveChangesAsync();
        }
    }

    private IEnumerable<ChallengeEntity> GetChallenges(long? number = 30)
    {
        var challenges = new List<ChallengeEntity>();
        var response = StreamerIdsProvider.StreamerIds;
        for (var i = 0; i < number; i++)
        {
            challenges.Add(new ChallengeEntity
            {
                Title = $"Title {i}",
                Description = $"It is a long description of challenge for streamer. Has Id = {i}",
                DonateFrom = $"Donater {i}",
                StreamerId = response.Ids[i % response.StreamersAmount],
                DonatePrice = i * new Random().Next(1000),
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