using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Data
{
    public class AppDbInitializer : IDbInitializer<AppDbContext>
    {
        public async Task Initialize(AppDbContext dbContext)
        {
            var dbIsCreated = await dbContext.Database.EnsureCreatedAsync();
            if (dbIsCreated)
            {
                await dbContext.Database.MigrateAsync();
            }
        }
    }
}
