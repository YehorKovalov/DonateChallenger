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
                var response = StreamerIdsProvider.StreamerIds;
                var password = new PasswordHasher<ApplicationUser>();
                var userStore = new UserStore<ApplicationUser>(dbContext);
                var email = string.Empty;
                var userName = string.Empty;
                var id = string.Empty;
                var hashed = string.Empty;
                var merchantId = string.Empty;
                var testMerchantIds = new string[] { "5WSFVT57ASS6U", "V9YL22Q4UQ3PA", "MCB5L4ZBENTWC" };

                for (var i = 0; i < response.StreamersAmount; i++)
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
                        MinDonatePriceInDollars = random.Next(1, 100),
                        MerchantId = testMerchantIds[i % 3],
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
