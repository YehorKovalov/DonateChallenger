using ChallengeOrder.API.Consumer;
using ChallengeOrder.API.Data;
using ChallengeOrder.API.Repositories;
using ChallengeOrder.API.Repositories.Abstractions;
using ChallengeOrder.API.Services;
using ChallengeOrder.API.Services.Abstractions;
using Infrastructure.MessageBus.Extensions;
using Infrastructure.MessageBus.Messages;
using Infrastructure.MessageBus.Messages.Requests;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;

namespace ChallengeOrder.API.Extensions;

public static class CustomIServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguredMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<MessageAddingChallengesAndPaymentIdConsumer>();
            x.UsingRabbitMq((context, config) =>
            {
                config.ConfigureRabbitMqConnectionProperties(configuration);
            });
        });

        services.AddMassTransitHostedService(true);
        return services;
    }

    public static IServiceCollection AddAppDependencies(this IServiceCollection services)
    {
        services.AddScoped<IDbContextWrapper<AppDbContext>, DbContextWrapper<AppDbContext>>();
        services.AddTransient<IChallengeOrderRepository, ChallengeOrderRepository>();
        services.AddTransient<IChallengeOrderService, ChallengeOrderService>();
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