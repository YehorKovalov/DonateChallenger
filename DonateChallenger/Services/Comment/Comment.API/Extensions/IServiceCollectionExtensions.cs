using Comment.API.Data;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Comment.API.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<AppDbContext>(o =>
        {
            var connectionString = configuration.GetConnectionString("CommentConnectionString");
            o.UseNpgsql(connectionString);
        });
        services.AddScoped<IDbContextWrapper<AppDbContext>, DbContextWrapper<AppDbContext>>();

        return services;
    }
}