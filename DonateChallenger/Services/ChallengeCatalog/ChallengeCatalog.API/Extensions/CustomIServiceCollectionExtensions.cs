using ChallengeCatalog.API.Data;
using ChallengeCatalog.API.Repositories;
using ChallengeCatalog.API.Repositories.Abstractions;
using ChallengeCatalog.API.Services;
using ChallengeCatalog.API.Services.Abstractions;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ChallengeCatalog.API.Extensions;

public static class CustomIServiceCollectionExtensions
{
    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<ChallengeCatalogDbContext>(o =>
        {
            var connectionString = configuration.GetConnectionString("ChallengeConnectionString");
            o.UseNpgsql(connectionString, sql =>
            {
                sql.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(2), errorCodesToAdd: null);
            });
        });
        services.AddScoped<IDbContextWrapper<ChallengeCatalogDbContext>, DbContextWrapper<ChallengeCatalogDbContext>>();

        return services;
    }

    public static IServiceCollection AddAppDependencies(this IServiceCollection services)
    {
        services.AddTransient<IChallengeRepository, ChallengeRepository>();
        services.AddTransient<IChallengeService, ChallengeService>();

        return services;
    }

    public static IServiceCollection AddAppCors(this IServiceCollection services)
    {
        services.AddCors(o =>
        {
            o.AddPolicy("CorsPolicy", policyBuilder =>
            {
                policyBuilder
                    .SetIsOriginAllowed(host => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        return services;
    }
}
