using Identity.API.Data.Identities;
using Identity.API.Models.AccountViewModels;
using Identity.API.Models.AccountViewModels.Responses;
using Identity.API.Services.Abstractions;
using IdentityModel;
using IdentityServer4.Services;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Options;

namespace Identity.API.Services;

public class AccountService : IAccountService<ApplicationUser>
{
    private readonly IIdentityServerInteractionService _interactionService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly AppSettings _appSettings;
    private readonly IHttpContextService _httpContextService;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IIdentityServerInteractionService interactionService,
        IOptions<AppSettings> appSettings,
        IHttpContextService httpContextService,
        ILogger<AccountService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interactionService = interactionService;
        _appSettings = appSettings.Value;
        _httpContextService = httpContextService;
        _logger = logger;
    }

    public async Task<ApplicationUser?> FindUserByEmail(string user)
    {
        if (string.IsNullOrWhiteSpace(user))
        {
            var errorMessage = $"{nameof(FindUserByEmail)} ---> Bad arguments. {nameof(user)} is not valid";
            _logger.LogError(errorMessage);
            throw new BusinessException(errorMessage);
        }

        var searchedUser = await _userManager.FindByEmailAsync(user);
        if (searchedUser == null)
        {
            _logger.LogInformation($"{nameof(FindUserByEmail)} ---> Searched User's not found");
        }

        return searchedUser;
    }

    public async Task<SignInResponseViewModel> SignInAsync(string email, string password, string returnUrl, bool rememberMe)
    {
        var response = new SignInResponseViewModel { ReturnUrl = GetValidatedReturnUrl(returnUrl) };

        var user = await FindUserByEmail(email);
        if (user == null)
        {
            return response;
        }

        var passwordIsValid = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordIsValid)
        {
            return response;
        }

        var props = BuildAuthenticationProperties(response.ReturnUrl, rememberMe);

        await _signInManager.SignInAsync(user, props);
        response.SignInIsSuccessful = true;
        return response;
    }

    public async Task<PostLogoutRedirectResponseViewModel> SignOutAsync(string logoutId)
    {
        var httpContext = _httpContextService.GetHttpContext();
        await HandleExternalIdentityProviderSignOut(httpContext, logoutId);

        await httpContext.SignOutAsync();

        await httpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

        var logout = await _interactionService.GetLogoutContextAsync(logoutId);
        return new PostLogoutRedirectResponseViewModel { Uri = logout.PostLogoutRedirectUri };
    }

    public async Task<SignUpResponseViewModel> SignUpAsync(ApplicationUser user, string email, string password, string confirmPassword, string? redirectUrl)
    {
        var user2 = new ApplicationUser
        {
            UserName = email,
            Email = email
        };

        var response = new SignUpResponseViewModel { RedirectUrl = GetValidatedReturnUrl(redirectUrl) };
        var result = await _userManager.CreateAsync(user2, password);

        response.SignUpIsSuccessful = !result.Errors.Any();
        return response;
    }

    public string GetValidatedReturnUrl(string? urlForValidating)
    {
        var alternativeRedirectUrl = _appSettings.ReactClientUrl ?? throw new NullReferenceException(nameof(_appSettings.ReactClientUrl));
        var urlIsValid = urlForValidating is not null && _interactionService.IsValidReturnUrl(urlForValidating);
        return urlIsValid ? urlForValidating! : alternativeRedirectUrl;
    }

    private async Task HandleExternalIdentityProviderSignOut(HttpContext httpContext, string logoutId)
    {
        var identityProvider = httpContext.User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
        if (identityProvider != null && identityProvider != IdentityServerConstants.LocalIdentityProvider)
        {
            if (logoutId == null)
            {
                logoutId = await _interactionService.CreateLogoutContextAsync();
            }

            var url = LogoutRedirectUriBuilder(logoutId);

            await httpContext.SignOutAsync(identityProvider, new AuthenticationProperties
            {
                RedirectUri = url
            });
        }
    }

    private AuthenticationProperties BuildAuthenticationProperties(string returnUrl, bool rememberMe)
    {
        var authProps = new AuthenticationProperties
        {
            AllowRefresh = true,
            RedirectUri = returnUrl
        };

        if (rememberMe)
        {
            var permanentTokenLifetime = _appSettings.PermanentTokenLifetimeDays ?? Defaults.PermanentTokenLifetimeDays;
            authProps.ExpiresUtc = DateTime.UtcNow.AddDays(permanentTokenLifetime);
            authProps.IsPersistent = true;
        }
        else
        {
            var tokenLifetime = _appSettings.TokenLifetimeMinutes ?? Defaults.DefaultTokenLifetimeMinutes;
            authProps.ExpiresUtc = DateTime.UtcNow.AddMinutes(tokenLifetime);
        }

        return authProps;
    }

    private string LogoutRedirectUriBuilder(string logoutId) => $"/Account/Logout?logoutId={logoutId}";
}