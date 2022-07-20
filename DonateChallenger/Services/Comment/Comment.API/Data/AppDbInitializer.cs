using Comment.API.Data.Entities;
using Infrastructure;
using Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;

namespace Comment.API.Data;

public class AppDbInitializer : IDbInitializer<AppDbContext>
{
    public async Task Initialize(AppDbContext dbContext)
    {
        var dbIsCreated = await dbContext.Database.EnsureCreatedAsync();
        if (dbIsCreated)
        {
            await dbContext.Database.MigrateAsync();
        }

        await EnsureSeedComments(dbContext);
    }

    private async Task EnsureSeedComments(AppDbContext dbContext)
    {
        if (!await dbContext.Comments.AnyAsync())
        {
            var response = StreamerIdsProvider.StreamerIds;
            var comments = new List<CommentEntity>();
            for (var i = 0; i < 1000; i++)
            {
                comments.Add(new CommentEntity
                {
                    ChallengeId = (i % CatalogChallengeSeedingConstants.ChallengesAmount) + 1,
                    Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s,",
                    UserId = response.Ids[i % response.StreamersAmount],
                    Date = DateTime.UtcNow
                });
            }

            await dbContext.Comments.AddRangeAsync(comments);
            await dbContext.SaveChangesAsync();
        }
    }
}