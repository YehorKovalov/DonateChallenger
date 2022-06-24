using Identity.API.Data;
using Identity.API.Data.Identities;
using Identity.API.Models.AccountViewModels;
using Identity.API.Services.Abstractions;
using IdentityServer4.Services;
using Infrastructure.Exceptions;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Authentication;

namespace Identity.API.Services;

public class AccountService : BaseDataService<AppDbContext>, IAccountService<ApplicationUser>
{
    private readonly IIdentityServerInteractionService _interactionService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly AppSettings _appSettings;

    protected AccountService(
        IDbContextWrapper<AppDbContext> dbContext,
        ILogger<BaseDataService<AppDbContext>> logger,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IIdentityServerInteractionService interactionService,
        AppSettings appSettings)
            : base(dbContext, logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interactionService = interactionService;
        _appSettings = appSettings;
    }

    public async Task<ApplicationUser?> FindUserByEmail(string user)
    {
        return await ExecuteSafeAsync(async () =>
        {
            if (string.IsNullOrWhiteSpace(user))
            {
                var errorMessage = $"{nameof(FindUserByEmail)} ---> Bad arguments. {nameof(user)} is not valid";
                Logger.LogError(errorMessage);
                throw new BusinessException(errorMessage);
            }

            var searchedUser = await _userManager.FindByEmailAsync(user);
            if (searchedUser == null)
            {
                Logger.LogInformation($"{nameof(FindUserByEmail)} ---> Searched User's not found");
            }

            return searchedUser;
        });
    }

    public async Task<SignInResponseViewModel> SignInAsync(string email, string password, string returnUrl, bool rememberMe)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var response = new SignInResponseViewModel { ReturnUrl = ValidateReturnUrl(returnUrl) };

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

            response.SignInIsSuccessful = true;
            var props = BuildAuthenticationProperties(response.ReturnUrl, rememberMe);

            await _signInManager.SignInAsync(user, props);
            return response;
        });
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

    private string ValidateReturnUrl(string returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            var errorMessage = $"{nameof(SignInAsync)} ---> Bad arguments. {nameof(returnUrl)} cannot be null or white space";
            Logger.LogError(errorMessage);
            throw new BusinessException(errorMessage);
        }

        return _interactionService.IsValidReturnUrl(returnUrl) ? returnUrl : Defaults.ReturnUrl;
    }
}