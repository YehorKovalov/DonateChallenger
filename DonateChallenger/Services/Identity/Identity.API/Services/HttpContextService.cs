using Identity.API.Services.Abstractions;

namespace Identity.API.Services;

public class HttpContextService : IHttpContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public HttpContext GetHttpContext() => _httpContextAccessor.HttpContext!;
}