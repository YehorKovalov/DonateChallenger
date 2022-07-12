using Infrastructure.MessageBus.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.API.Configurations;
using Payment.API.Services;
using Payment.API.Services.Abstractions;

namespace Payment.API.Extensions;

public static class CustomIServiceCollectionExtensions
{
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

    public static IServiceCollection RegisterAppConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PaypalConfiguration>(configuration.GetSection("PaypalConfiguration"));
        return services;
    }

    public static IServiceCollection AddConfiguredMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
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
        services.AddTransient<IPaymentService, PaypalPaymentService>();
        return services;
    }

    public static IServiceCollection AddConfiguredMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureRabbitMqConnectionProperties(configuration);
                cfg.ReceiveEndpoint(configuration["RabbitMQ:OrderUserInfoQueue"], c =>
                {
                    // c.ConfigureConsumer<>(context);
                });
            });
        });
        services.AddMassTransitHostedService(true);
        return services;
    }
}