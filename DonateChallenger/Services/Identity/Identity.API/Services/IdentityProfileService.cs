using System.Security.Claims;
using Identity.API.Data.Entities;

namespace Identity.API.Services;

public class IdentityProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityProfileService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context) {
        var user = await _userManager.GetUserAsync(context.Subject);

        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = roles.Select(role => new Claim(JwtClaimTypes.Role, role)).ToList();

        context.IssuedClaims.AddRange(roleClaims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        context.IsActive = user != null;
    }
}