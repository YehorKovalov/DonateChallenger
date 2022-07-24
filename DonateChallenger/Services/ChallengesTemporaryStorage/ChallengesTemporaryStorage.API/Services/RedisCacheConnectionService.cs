using ChallengesTemporaryStorage.API.Configurations;
using ChallengesTemporaryStorage.API.Services.Abstractions;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace ChallengesTemporaryStorage.API.Services;

public class RedisCacheConnectionService : IRedisCacheConnectionService
{
    private readonly Lazy<ConnectionMultiplexer> _connectionLazy;

    private bool _disposed;

    public RedisCacheConnectionService(
        IOptions<RedisConfig> config)
    {
        var redisConfigurationOptions = ConfigurationOptions.Parse(config.Value.Host);
        _connectionLazy = new Lazy<ConnectionMultiplexer>(
            () => ConnectionMultiplexer.Connect(redisConfigurationOptions));
    }

    public IConnectionMultiplexer Connection => _connectionLazy.Value;

    public void Dispose()
    {
        if (!_disposed)
        {
            Connection.Dispose();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }
}