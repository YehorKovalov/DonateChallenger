using Comment.API.Data;
using Comment.API.Repositories;
using Comment.API.Repositories.Abstractions;
using Comment.API.Services;
using Comment.API.Services.Abstractions;
using Infrastructure.MessageBus.Extensions;
using Infrastructure.MessageBus.Messages.Requests;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;
using MassTransit;
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

        return services;
    }

    public static IServiceCollection AddAppDependencies(this IServiceCollection services)
    {
        services.AddTransient<ICommentRepository, CommentRepository>();
        services.AddTransient<ICommentService, CommentService>();
        services.AddScoped<IDbContextWrapper<AppDbContext>, DbContextWrapper<AppDbContext>>();

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

    public static IServiceCollection AddConfiguredMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureRabbitMqConnectionProperties(configuration);
            });
            x.AddRequestClient<MessageFindUsernamesByIdsRequest>();
        });

        services.AddMassTransitHostedService(true);
        return services;
    }
}