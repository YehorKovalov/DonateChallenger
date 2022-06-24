using Identity.API.Models.AccountViewModels;
using Microsoft.AspNetCore.Authentication;

namespace Identity.API.Services.Abstractions;

public interface IAccountService<TUser>
    where TUser : class
{
    Task<TUser?> FindUserByEmail(string user);

    Task<SignInResponseViewModel> SignInAsync(string email, string password, string returnUrl, bool rememberMe);
}