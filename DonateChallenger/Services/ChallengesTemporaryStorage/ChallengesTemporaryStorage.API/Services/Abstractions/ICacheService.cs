using StackExchange.Redis;

namespace ChallengesTemporaryStorage.API.Services.Abstractions;

public interface ICacheService
{
    Task<bool> UpdateAsync<T>(string key, T value, IDatabase? redis = null, TimeSpan? expiry = null);
    Task<TResult?> GetAsync<TResult>(string key);
    Task<bool> DeleteAsync(string key);
}