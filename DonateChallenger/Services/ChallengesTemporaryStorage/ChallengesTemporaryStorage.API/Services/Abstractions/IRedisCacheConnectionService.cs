using StackExchange.Redis;

namespace ChallengesTemporaryStorage.API.Services.Abstractions;

public interface IRedisCacheConnectionService
{
    IConnectionMultiplexer Connection { get; }   
}