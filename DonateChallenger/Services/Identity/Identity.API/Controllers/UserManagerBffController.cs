using Identity.API.Models.DTOs;
using Identity.API.Models.Requests;
using Identity.API.Models.Responses;
using Identity.API.Services.Abstractions;
using Infrastructure;
using Infrastructure.Identity;

namespace Identity.API.Controllers;

[SecurityHeaders]
[ApiController]
[Route(Defaults.DefaultRoute)]
[Scope("user-manager.bff")]
[Authorize(Policy = AuthPolicy.AdminOnlyPolicy)]
public class UserManagerBffController : ControllerBase
{
    private readonly IAccountManagerService _accountManager;

    public UserManagerBffController(IAccountManagerService accountManager) => _accountManager = accountManager;
    
    [HttpPost]
    [ProducesResponseType(typeof(ManagerUpdateProfileResponse), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateStreamerProfile(ManagerUpdateStreamerProfileRequest request)
    {
        var result = await _accountManager.UpdateStreamerProfileAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ManagerUpdateProfileResponse), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateUserProfile(ManagerUpdateUserProfileRequest request)
    {
        var result = await _accountManager.UpdateUserProfileAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DeleteUserByIdResponse<bool>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(string userId)
    {
        var result = await _accountManager.DeleteUserByIdAsync(userId);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ManagerGetPortionedUsersResponse<StreamerProfileDto>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> Streamers(ManagerGetPortionedUsersRequest request)
    {
        var result = await _accountManager.GetPortionedStreamersAsync(request);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ManagerGetPortionedUsersResponse<UserProfileDto>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> All(ManagerGetPortionedUsersRequest request)
    {
        var result = await _accountManager.GetPortionedUsersAsync(request);
        return Ok(result);
    }
}