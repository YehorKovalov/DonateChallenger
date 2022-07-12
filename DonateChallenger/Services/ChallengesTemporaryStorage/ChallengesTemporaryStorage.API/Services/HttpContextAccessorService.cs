using System.Security.Claims;
using ChallengesTemporaryStorage.API.Configurations;
using ChallengesTemporaryStorage.API.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace ChallengesTemporaryStorage.API.Services;

public class HttpContextAccessorService : IHttpContextAccessorService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly AnonymousUserConfig _anonymousUser;

    public HttpContextAccessorService(
        IHttpContextAccessor contextAccessor,
        IOptions<AnonymousUserConfig> anonymousUser)
    {
        _contextAccessor = contextAccessor;
        _anonymousUser = anonymousUser.Value;
    }

    public ClaimsPrincipal? GetUser() => _contextAccessor.HttpContext?.User;

    public string GetUserId()
    {
        var user = GetUser();
        var userId = user?.Claims.FirstOrDefault(t => t.Type == "sub")?.Value;
        var userIsAuthorized = !string.IsNullOrWhiteSpace(userId);
        return userIsAuthorized ? userId! : _anonymousUser.GuestUserId;
    }
}