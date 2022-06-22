using ChallengeCatalog.API.Data.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ChallengeCatalog.API.Data;

public class ChallengesCatalogDbInitializer : IDbInitializer<ChallengeCatalogDbContext>
{
    public void Initialize(ChallengeCatalogDbContext dbContext)
    {
        var dbIsCreated = dbContext.Database.EnsureCreated();
        if (dbIsCreated)
        {
            dbContext.Database.Migrate();
        }

        if (!dbContext.Challenges.Any())
        {
            var challenges = GetChallenges(100);
            dbContext.Challenges.AddRange(challenges);
            dbContext.SaveChanges();
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
                CreatedTime = i % 2 == 0 ? DateTime.UtcNow : DateTime.Now,
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