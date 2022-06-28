using Identity.API.Models.AccountViewModels;
using Identity.API.Models.AccountViewModels.Responses;

namespace Identity.API.Services.Abstractions;

public interface IAccountService<TUser>
    where TUser : class
{
    Task<SignUpResponseViewModel> SignUpAsync(TUser user, string email, string password, string confirmPassword, string? redirectUrl);
    string GetValidatedReturnUrl(string urlForValidating);
    Task<SignInResponseViewModel> SignInAsync(string email, string password, string returnUrl, bool rememberMe);
    Task<PostLogoutRedirectResponseViewModel> SignOutAsync(string logoutId);
}