using Identity.API.Models.DTOs;
using Identity.API.Models.Responses;
using Identity.API.Services.Abstractions;
using Infrastructure;
using Infrastructure.Identity;

namespace Identity.API.Controllers;

[SecurityHeaders]
[ApiController]
[Authorize(Policy = AuthPolicy.AuthorizedWithScope)]
[Scope("user-profile.bff")]
[Route(Defaults.DefaultRoute)]
public class UserProfileBffController : ControllerBase
{
    private readonly IUserService _userService;

    public UserProfileBffController(IUserService userService) =>_userService = userService;
    
    [HttpPost]
    [ProducesResponseType(typeof(ChangeProfileDataResponse<string>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> ChangeNickname(string userId, string newNickname)
    {
        var result = await _userService.ChangeUserNickname(userId, newNickname);
        return Ok(result);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(GetUserProfileResponse<UserProfileDto>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> UserProfile(string userId)
    {
        var result = await _userService.GetUserProfile(userId);
        return Ok(result);
    }
}