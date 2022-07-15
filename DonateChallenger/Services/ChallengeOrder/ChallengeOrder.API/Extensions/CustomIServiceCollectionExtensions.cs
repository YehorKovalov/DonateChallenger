using ChallengeOrder.API.Consumer;
using ChallengeOrder.API.Data;
using ChallengeOrder.API.Repositories;
using ChallengeOrder.API.Repositories.Abstractions;
using ChallengeOrder.API.Services;
using ChallengeOrder.API.Services.Abstractions;
using Infrastructure.MessageBus.Extensions;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;
using MassTransit;

namespace ChallengeOrder.API.Extensions;

public static class CustomIServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguredMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<MessageAddingChallengesStatusAndPaymentIdConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureRabbitMqConnectionProperties(configuration);
                cfg.ReceiveEndpoint(configuration["RabbitMQ:AddingChallengesStatusAndPaymentIdQueue"], c =>
                {
                    c.ConfigureConsumer<MessageAddingChallengesStatusAndPaymentIdConsumer>(context);
                });
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