using ChallengesTemporaryStorage.API.Configurations;
using ChallengesTemporaryStorage.API.Consumers;
using ChallengesTemporaryStorage.API.Services;
using ChallengesTemporaryStorage.API.Services.Abstractions;
using Infrastructure.MessageBus.Extensions;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;
using MassTransit;

namespace ChallengesTemporaryStorage.API.Extensions;

public static class CustomIServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguredMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<MessageChallengeOrderStatusConsumer>();
            x.AddConsumer<GetChallengesFromStorageConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureRabbitMqConnectionProperties(configuration);
                cfg.ReceiveEndpoint(configuration["RabbitMQ:ChallengeOrderStatusQueue"], c =>
                {
                     c.ConfigureConsumer<MessageChallengeOrderStatusConsumer>(context);
                });
                cfg.ReceiveEndpoint(configuration["RabbitMQ:GetChallengesFromStorageQueue"], c =>
                {
                     c.ConfigureConsumer<GetChallengesFromStorageConsumer>(context);
                });
            });
        });

        services.AddMassTransitHostedService(true);

        return services;
    }

    public static IServiceCollection AddAppDependencies(this IServiceCollection services)
    {
        services.AddTransient<ICacheService, CacheService>();
        services.AddTransient<IRedisCacheConnectionService, RedisCacheConnectionService>();
        services.AddTransient<IChallengesTemporaryStorageService, ChallengesTemporaryStorageService>();
        services.AddTransient<IJsonSerializerWrapper, JsonSerializerWrapper>();

        services.AddHttpContextAccessor();
        services.AddScoped<IHttpContextAccessorService, HttpContextAccessorService>();

        return services;
    }

    public static IServiceCollection RegisterAppConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AnonymousUserConfig>(configuration.GetSection("AnonymousUserConfig"));
        services.Configure<RedisConfig>(configuration.GetSection("Redis"));
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