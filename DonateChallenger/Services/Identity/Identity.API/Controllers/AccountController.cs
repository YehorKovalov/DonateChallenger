using Identity.API.Data.Identities;
using Identity.API.Models.AccountViewModels;
using Identity.API.Services.Abstractions;

namespace Identity.API.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly IAccountService<ApplicationUser> _accountService;
    public AccountController(IAccountService<ApplicationUser> accountService) => _accountService = accountService;

    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        var vm = new LoginViewModel { ReturnUrl = _accountService.GetValidatedReturnUrl(returnUrl) };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var response = await _accountService.SignInAsync(model.Email, model.Password, model.ReturnUrl, model.RememberMe);
        if (response.SignInIsSuccessful)
        {
            return Redirect(response.ReturnUrl);
        }

        return View(response.ReturnUrl);
    }

    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return await Logout(new LogoutViewModel { LogoutId = logoutId });
        }

        var viewModel = new LogoutViewModel { LogoutId = logoutId };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(LogoutViewModel model)
    {
        var viewModel = await _accountService.SignOutAsync(model.LogoutId);
        return Redirect(viewModel.Uri);
    }

    [HttpGet]
    public IActionResult Register(string returnUrl)
    {
        var viewModel = new RegisterViewModel { ReturnUrl = _accountService.GetValidatedReturnUrl(returnUrl) };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var responseViewModel = await _accountService.SignUpAsync(model.User, model.Email, model.Password, model.ConfirmPassword, model?.ReturnUrl);
        var redirectTo = responseViewModel.SignUpIsSuccessful ? nameof(Login) : nameof(Register);
        return RedirectToAction(redirectTo, new { returnUrl = responseViewModel.RedirectUrl });
    }
}