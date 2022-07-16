using ChallengeCatalog.API.Consumers;
using ChallengeCatalog.API.Data;
using ChallengeCatalog.API.Repositories;
using ChallengeCatalog.API.Repositories.Abstractions;
using ChallengeCatalog.API.Services;
using ChallengeCatalog.API.Services.Abstractions;
using Infrastructure.MessageBus.Extensions;
using Infrastructure.MessageBus.Messages.Requests;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ChallengeCatalog.API.Extensions;

public static class CustomIServiceCollectionExtensions
{
    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<ChallengeCatalogDbContext>(o =>
        {
            var connectionString = configuration.GetConnectionString("ChallengeConnectionString");
            o.UseNpgsql(connectionString);
        });
        services.AddScoped<IDbContextWrapper<ChallengeCatalogDbContext>, DbContextWrapper<ChallengeCatalogDbContext>>();

        return services;
    }

    public static IServiceCollection AddAppDependencies(this IServiceCollection services)
    {
        services.AddTransient<IChallengeCatalogRepository, ChallengeCatalogCatalogRepository>();
        services.AddTransient<IChallengeCatalogService, ChallengeCatalogCatalogService>();
        services.AddTransient<IJsonSerializerWrapper, JsonSerializerWrapper>();

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
            x.AddConsumer<MessagePaymentStatusConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureRabbitMqConnectionProperties(configuration);
                cfg.ReceiveEndpoint(configuration["RabbitMQ:PaymentStatusQueue"], c =>
                {
                    c.ConfigureConsumer<MessagePaymentStatusConsumer>(context);
                });
            });
            x.AddRequestClient<MessageGetChallengesFromStorageRequest>();
        });

        services.AddMassTransitHostedService(true);
        return services;
    }
}
