using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Data
{
    public class ConfigurationDbContextInitializer : IDbInitializer<ConfigurationDbContext>
    {
        public async Task Initialize(ConfigurationDbContext context)
        {
            var dbIsCreated = await context.Database.EnsureCreatedAsync();
            if (dbIsCreated)
            {
                await context.Database.MigrateAsync();
            }

            await EnsureSeedAsync(context);
        }

        private async Task EnsureSeedAsync(ConfigurationDbContext context)
        {
            if (!await context.Clients.AnyAsync())
            {
                var clients = Config.GetClients().Select(s => s.ToEntity());
                await context.Clients.AddRangeAsync(clients);
                await context.SaveChangesAsync();
            }

            if (!await context.ApiResources.AnyAsync())
            {
                var apiResources = Config.ApiResources.Select(s => s.ToEntity());
                await context.ApiResources.AddRangeAsync(apiResources);
                await context.SaveChangesAsync();
            }

            if (!await context.IdentityResources.AnyAsync())
            {
                var identityResources = Config.IdentityResources.Select(s => s.ToEntity());
                await context.IdentityResources.AddRangeAsync(identityResources);
                await context.SaveChangesAsync();
            }

            if (!await context.ApiScopes.AnyAsync())
            {
                var apiScopes = Config.Scopes.Select(s => s.ToEntity());
                await context.ApiScopes.AddRangeAsync(apiScopes);
                await context.SaveChangesAsync();
            }
        }
    }
}