namespace ChallengesTemporaryStorage.API.Configurations;

public class RedisConfig
{
    public TimeSpan CacheTimeout { get; set; }

    public string Host { get; set; } = null!;
}