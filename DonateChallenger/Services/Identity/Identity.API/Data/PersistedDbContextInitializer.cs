using IdentityServer4.EntityFramework.DbContexts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Data;

public class PersistedDbContextInitializer : IDbInitializer<PersistedGrantDbContext>
{
    public async Task Initialize(PersistedGrantDbContext dbContext)
    {
        var dbIsCreated = await dbContext.Database.EnsureCreatedAsync();
        if (dbIsCreated)
        {
            await dbContext.Database.MigrateAsync();
        }
    }
}