using ChallengesTemporaryStorage.API.Models;

namespace ChallengesTemporaryStorage.API.Services.Abstractions;

public interface IChallengesTemporaryStorageService
{
    Task<GetChallengesTemporaryStorageResponse<TResult?>> GetAsync<TResult>(string? userId = null);

    Task<bool> UpdateAsync<T>(T data, string? userId = null);

    Task<bool> DeleteAsync(string? userId = null);
}