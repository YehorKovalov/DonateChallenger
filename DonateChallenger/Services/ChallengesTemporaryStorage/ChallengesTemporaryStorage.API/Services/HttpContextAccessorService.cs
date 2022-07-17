using System.Security.Claims;
using ChallengesTemporaryStorage.API.Configurations;
using ChallengesTemporaryStorage.API.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace ChallengesTemporaryStorage.API.Services;

public class HttpContextAccessorService : IHttpContextAccessorService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly AnonymousUserConfig _anonymousUser;
    private readonly ILogger<HttpContextAccessorService> _logger;

    public HttpContextAccessorService(
        IHttpContextAccessor contextAccessor,
        IOptions<AnonymousUserConfig> anonymousUser,
        ILogger<HttpContextAccessorService> logger)
    {
        _contextAccessor = contextAccessor;
        _logger = logger;
        _anonymousUser = anonymousUser.Value;
    }

    public ClaimsPrincipal? GetUser()
    {
        var user = _contextAccessor.HttpContext?.User;
        if (user == null)
        {
            _logger.LogInformation($"{nameof(GetUser)} ---> User is null");
        }

        return user;
    }

    public string GetUserId()
    {
        var userId = GetUser()?.FindFirstValue("sub");
        var userIsAuthorized = !string.IsNullOrWhiteSpace(userId);
        return userIsAuthorized ? userId! : _anonymousUser.GuestUserId;
    }
}