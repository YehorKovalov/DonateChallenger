using ChallengesTemporaryStorage.API.Models;
using ChallengesTemporaryStorage.API.Services.Abstractions;

namespace ChallengesTemporaryStorage.API.Services;

public class ChallengesTemporaryStorageService : IChallengesTemporaryStorageService
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<ChallengesTemporaryStorageService> _logger;
    private readonly IHttpContextAccessorService _httpContextAccessor;
    public ChallengesTemporaryStorageService(
        ICacheService cacheService,
        ILogger<ChallengesTemporaryStorageService> logger,
        IHttpContextAccessorService httpContextAccessor)
    {
        _cacheService = cacheService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetChallengesTemporaryStorageResponse<TResult?>> GetAsync<TResult>(string? userId = null)
    {
        var key = _httpContextAccessor.GetUserId();
        _logger.LogWarning($"{nameof(GetAsync)} ---> key is : {key}");

        var result = await _cacheService.GetAsync<TResult>(key);
        _logger.LogWarning($"{nameof(GetAsync)} ---> result: {result}");

        return new GetChallengesTemporaryStorageResponse<TResult?> { Data = result };
    }

    public async Task<bool> UpdateAsync<T>(T? data, string? userId = null)
    {
        if (data == null)
        {
            _logger.LogWarning($"{nameof(UpdateAsync)} ---> data is null");
        }

        var key = _httpContextAccessor.GetUserId();
        return await _cacheService.UpdateAsync(key, data);
    }

    public async Task<bool> DeleteAsync(string? userId = null)
    {
        var key = _httpContextAccessor.GetUserId();
        var result = await _cacheService.DeleteAsync(key);
        if (!result)
        {
            _logger.LogError($"{nameof(DeleteAsync)} ---> Basket is not deleted!");
        }

        return result;
    }
}