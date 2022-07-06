using Infrastructure;
using Infrastructure.Seeding;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

            await EnsureSeedUsers(dbContext);
        }

        private async Task EnsureSeedUsers(AppDbContext dbContext)
        {
            if (!await dbContext.Users.AnyAsync())
            {
                var random = new Random();
                var response = StreamerIdsProvider.Response;
                var password = new PasswordHasher<ApplicationUser>();
                var userStore = new UserStore<ApplicationUser>(dbContext);
                var email = string.Empty;
                var userName = string.Empty;
                var id = string.Empty;
                var hashed = string.Empty;
                
                for (var i = 0; i < response.IdsAmount; i++)
                {
                    email = $"challenger.test{i}@mail.com";
                    userName = $"challenger.test{i}@mail.com";
                    id = response.Ids[i];

                    var user = new ApplicationUser
                    {
                        Id = id,
                        Email = email,
                        NormalizedEmail = email.Normalize(),
                        UserName = userName,
                        NormalizedUserName = userName.Normalize(),
                        Nickname = $"Donater {i}",
                        MinDonatePriceInDollars = random.Next(100000),
                        SecurityStamp = Guid.NewGuid().ToString(),
                        EmailConfirmed = true,
                        LockoutEnabled = false
                    };
                    
                    hashed = password.HashPassword(user,"secret");
                    user.PasswordHash = hashed;

                    await userStore.CreateAsync(user);
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
