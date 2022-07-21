using Identity.API.Data.Entities;
using Infrastructure;
using Infrastructure.Seeding;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Identity.API.Data
{
    public class AppDbInitializer : IDbInitializer<AppDbContext>
    {
        private const int ManagersAmount = 2;
        private const int AdminsAmount = 2;
        public async Task Initialize(AppDbContext dbContext)
        {
            var dbIsCreated = await dbContext.Database.EnsureCreatedAsync();
            if (dbIsCreated)
            {
                await dbContext.Database.MigrateAsync();
            }

            await EnsureSeedRoles(dbContext);
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
                var testMerchantIds = new string[] { "5WSFVT57ASS6U", "V9YL22Q4UQ3PA", "MCB5L4ZBENTWC" };

                for (var i = 0; i < response.StreamersAmount; i++)
                {
                    var email = $"challenger.test{i}@mail.com";
                    var userName = $"challenger.test{i}@mail.com";
                    var merchantId = testMerchantIds[i % 3];
                    var nickname = $"Streamer {i}";
                    var minDonatePriceInDollars = random.Next(1, 100);
                    
                    var user = new ApplicationUser
                    {
                        Id = response.Ids[i],
                        Email = email,
                        NormalizedEmail = email.Normalize(),
                        UserName = userName,
                        NormalizedUserName = userName.Normalize(),
                        Nickname = nickname,
                        MinDonatePriceInDollars = minDonatePriceInDollars,
                        MerchantId = merchantId,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        EmailConfirmed = true,
                        LockoutEnabled = false
                    };
                    
                    user.PasswordHash = password.HashPassword(user,"secret");
                    await userStore.AddToRoleAsync(user, "streamer");
                    await userStore.CreateAsync(user);
                }

                for (var i = 0; i < AdminsAmount; i++)
                {
                    var email = $"challenger-admin{i}@mail.com";
                    var userName = $"challenger-admin{i}@mail.com";
                    var nickname = $"{Guid.NewGuid()}";
                    
                    var user = new ApplicationUser
                    {
                        Id = $"{Guid.NewGuid()}",
                        Email = email,
                        NormalizedEmail = email.Normalize(),
                        UserName = userName,
                        NormalizedUserName = userName.Normalize(),
                        Nickname = nickname,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        EmailConfirmed = true,
                        LockoutEnabled = false
                    };
                    
                    user.PasswordHash = password.HashPassword(user,"admin");
                    await userStore.AddToRoleAsync(user, "admin");
                    await userStore.CreateAsync(user);
                }

                for (var i = 0; i < ManagersAmount; i++)
                {
                    var email = $"challenger-manager{i}@mail.com";
                    var userName = $"challenger-manager{i}@mail.com";
                    var nickname = $"Manager{i}";
                    
                    var user = new ApplicationUser
                    {
                        Id = $"{Guid.NewGuid()}",
                        Email = email,
                        NormalizedEmail = email.Normalize(),
                        UserName = userName,
                        NormalizedUserName = userName.Normalize(),
                        Nickname = nickname,
                        SecurityStamp = $"{Guid.NewGuid()}",
                        EmailConfirmed = true,
                        LockoutEnabled = false
                    };
                    
                    user.PasswordHash = password.HashPassword(user,"manager");
                    await userStore.AddToRoleAsync(user, "manager");
                    await userStore.CreateAsync(user);
                }

                await dbContext.SaveChangesAsync();
            }
        }

        private async Task EnsureSeedRoles(AppDbContext dbContext)
        {
            if (!await dbContext.Roles.AnyAsync())
            {
                var roles = new List<IdentityRole>
                {
                    new IdentityRole
                    {
                        Name = "admin",
                        NormalizedName = "admin"
                    },
                    new IdentityRole
                    {
                        Name = "manager",
                        NormalizedName = "manager"
                    },
                    new IdentityRole
                    {
                        Name = "streamer",
                        NormalizedName = "streamer"
                    },
                    new IdentityRole
                    {
                        Name = "donater",
                        NormalizedName = "donater"
                    },
                };
                await dbContext.AddRangeAsync(roles);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
