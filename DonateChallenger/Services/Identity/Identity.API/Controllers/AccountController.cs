using System.Security.Claims;
using Identity.API.Data;
using Identity.API.Data.Entities;
using Identity.API.Helpers;
using Identity.API.Models.Account;
using Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Identity.API.Controllers
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var vm = await BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
                return RedirectToAction("Challenge", "External", new { scheme = vm.ExternalLoginScheme, returnUrl });

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            if (button != "login")
            {
                if (context != null)
                {
                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                    if (context.IsNativeClient())
                        return this.LoadingPage("Redirect", model.ReturnUrl);

                    return Redirect(model.ReturnUrl);
                }

                return Redirect("~/");
            }

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    var roles = await _userManager.GetRolesAsync(user);
                    var roleClaims = roles.Select(role => new Claim(JwtClaimTypes.Role, role)).ToList();
                    await _userManager.AddClaimsAsync(user, roleClaims);
                    
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

                    if (context != null)
                    {
                        if (context.IsNativeClient())
                            return this.LoadingPage("Redirect", model.ReturnUrl);

                        return Redirect(model.ReturnUrl);
                    }

                    if (Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);
                    if (string.IsNullOrEmpty(model.ReturnUrl))
                        return Redirect("~/");

                    throw new Exception("invalid return URL");
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId:context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }

        
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
                return await Logout(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                await _signInManager.SignOutAsync();

                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            if (vm.TriggerExternalSignout)
            {
                var url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var roles = await _roleManager.Roles.ToListAsync();
            var vm = new RegisterViewModel
            {
                Roles = roles
                    .Where(w => w.NormalizedName != "admin" && w.NormalizedName != "manager")
                    .Select(s => new SelectListItem 
                    {
                        Value = s.NormalizedName,
                        Text = s.Name
                    })
            };
            return View(vm);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Errors.Any())
                {
                    AddErrors(result);
                    return View(model);
                }

                var addingToRoleResult = await _userManager.AddToRoleAsync(user, model.Role);
                if (addingToRoleResult.Errors.Any())
                {
                    AddErrors(addingToRoleResult);
                    return View(model);
                }

                var parameters = new
                {
                    returnUrl = returnUrl,
                    userId = user.Id,
                    role = model.Role
                };
                return RedirectToAction("RegisterAdditionalInformation", "account", parameters);
            }

            if (returnUrl != null)
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                    return Redirect(returnUrl);
                return View(model);
            }

            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public IActionResult RegisterAdditionalInformation(string returnUrl, string userId, string role)
        {
            var vm = new AdditionalInformationViewModel
            {
                ReturnUrl = returnUrl,
                UserId = userId,
                Role = role
            };
            return View(vm);
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAdditionalInformation(AdditionalInformationViewModel model)
        {
            if (ModelState.IsValid)
            {
                const string streamerRole = "streamer";
                var errors = IfStreamerHandleStateAndGetErrors(model, streamerRole).ToList();
                if (errors.Any())
                {
                    AddErrors(errors);
                    return View(model);
                }

                var nicknameAlreadyExists = await _userManager.Users
                    .AnyAsync(f => f.Nickname == model.Nickname);

                if (nicknameAlreadyExists)
                {
                    ModelState.AddModelError(string.Empty, "Nickname already exists");
                    return View(model);
                }

                var user = await _userManager.FindByIdAsync(model.UserId);
                user.Nickname = model.Nickname;
                if (model.Role == streamerRole)
                {
                    user.MinDonatePriceInDollars = model.MinDonatePriceInDollars!.Value;
                    user.MerchantId = model.MerchantId;
                }

                var result = await _userManager.UpdateAsync(user);
                if (result.Errors.Any())
                {
                    AddErrors(result);
                    return View(model);
                }
                
                if (model.ReturnUrl != null)
                    return RedirectToAction("login", "account", new { returnUrl = model.ReturnUrl });
            }
            
            if (model.ReturnUrl != null)
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                    return Redirect(model.ReturnUrl);
                return View(model);
            }

            return RedirectToAction("index", "home");
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if (!local)
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }

        private void AddErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
                ModelState.AddModelError(string.Empty, error);
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            return vm;
        }

        private IEnumerable<string> IfStreamerHandleStateAndGetErrors(AdditionalInformationViewModel model, string streamerRole)
        {
            if (model.Role != streamerRole)
            {
                return Enumerable.Empty<string>();
            }

            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.MerchantId))
            {
                errors.Add("The MerchantId field is required.");
            }

            if (model.MinDonatePriceInDollars is < ValidationConstants.LogicalMaximumMinimumDonatePrice
                or > ValidationConstants.LogicalMinimumDonatePrice)
            {
                errors.Add($"The field Min Donate Price must be between {ValidationConstants.LogicalMaximumMinimumDonatePrice} and {ValidationConstants.LogicalMinimumDonatePrice}.");
            }

            return errors;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }
    }
}