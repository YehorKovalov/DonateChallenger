using System.Security.Claims;

namespace ChallengesTemporaryStorage.API.Services.Abstractions;

public interface IHttpContextAccessorService
{
    ClaimsPrincipal? GetUser();
    string GetUserId();
}