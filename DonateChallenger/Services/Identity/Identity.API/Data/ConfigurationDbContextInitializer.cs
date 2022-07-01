using System.Threading.Tasks;
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
                var clients = Config.GetClients();
                foreach (var client in clients)
                {
                    await context.Clients.AddAsync(client.ToEntity());
                }

                await context.SaveChangesAsync();
            }

            if (!await context.IdentityResources.AnyAsync())
            {
                foreach (var resource in Config.Resources)
                {
                    await context.IdentityResources.AddAsync(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }

            if (!await context.ApiScopes.AnyAsync())
            {
                foreach (var api in Config.Scopes)
                {
                    await context.ApiScopes.AddAsync(api.ToEntity());
                }

                await context.SaveChangesAsync();
            }
        }
    }
}