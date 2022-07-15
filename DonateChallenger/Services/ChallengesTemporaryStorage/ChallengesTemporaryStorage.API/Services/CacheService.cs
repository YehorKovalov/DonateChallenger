using ChallengesTemporaryStorage.API.Configurations;
using ChallengesTemporaryStorage.API.Services.Abstractions;
using Infrastructure.Services.Abstractions;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace ChallengesTemporaryStorage.API.Services;

public class CacheService : ICacheService
{
    private readonly RedisConfig _config;
    private readonly IRedisCacheConnectionService _redisCacheConnectionService;
    private readonly IJsonSerializerWrapper _jsonSerializer;
    private readonly ILogger<CacheService> _logger;

    public CacheService(
        IRedisCacheConnectionService redisCacheConnectionService,
        IOptions<RedisConfig> config,
        IJsonSerializerWrapper jsonSerializer,
        ILogger<CacheService> logger)
    {
        _redisCacheConnectionService = redisCacheConnectionService;
        _jsonSerializer = jsonSerializer;
        _logger = logger;
        _config = config.Value;
    }

    public async Task<bool> UpdateAsync<T>(string key, T value, IDatabase? redis = null, TimeSpan? expiry = null)
    {
        redis ??= GetRedisDatabase();
        expiry ??= _config.CacheTimeout;

        var serialized = _jsonSerializer.Serialize(value);
        var result = await redis.StringSetAsync(key, serialized, expiry);
        if (result)
        {
            _logger.LogInformation($"Cached value for key {key} cached");
        }
        else
        {
            _logger.LogInformation($"Cached value for key {key} updated");
        }

        return result;
    }

    public async Task<TResult?> GetAsync<TResult>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return default(TResult);
        }

        var redis = GetRedisDatabase();

        var serialized = await redis.StringGetAsync(key);

        return serialized.HasValue ?
            _jsonSerializer.Deserialize<TResult>(serialized.ToString())
            : default!;
    }

    public async Task<bool> DeleteAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        var redis = GetRedisDatabase();
        return await redis.KeyDeleteAsync(key);
    }

    private IDatabase GetRedisDatabase() => _redisCacheConnectionService.Connection.GetDatabase();
}