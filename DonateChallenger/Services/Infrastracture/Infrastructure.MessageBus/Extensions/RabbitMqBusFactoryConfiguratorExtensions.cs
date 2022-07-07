using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.MessageBus.Extensions;

public static class RabbitMqBusFactoryConfiguratorExtensions
{
    public static void ConfigureRabbitMqConnectionProperties(this IRabbitMqBusFactoryConfigurator rabbitMqBusConfigurator, IConfiguration configuration)
    {
        var uri = new Uri(configuration["RabbitMQ:Uri"]);
        var username = configuration["RabbitMQ:Username"] ?? throw new ArgumentNullException("username");
        var password = configuration["RabbitMQ:Password"] ?? throw new ArgumentNullException("password");
        rabbitMqBusConfigurator.Host(uri, host =>
        {
            host.Username(username);
            host.Password(password);
        });
    }
}