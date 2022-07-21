using Identity.API.Models.DTOs;
using Identity.API.Models.Responses;
using Identity.API.Services.Abstractions;
using Infrastructure;
using Infrastructure.Identity;

namespace Identity.API.Controllers;

[SecurityHeaders]
[Authorize(Policy = AuthPolicy.StreamerPolicy)]
[ApiController]
[Scope("streamer-profile.bff")]
[Route(Defaults.DefaultRoute)]
public class StreamerBffController : ControllerBase
{
    private readonly IStreamerService _streamerService;

    public StreamerBffController(IStreamerService streamerService) => _streamerService = streamerService;

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SearchStreamersByNicknameResponse<SearchedStreamerByNicknameDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> SearchNicknames(string nicknameAsFilter)
    {
        var result = await _streamerService.FindStreamerByNicknameAsync(nicknameAsFilter);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetMinDonatePriceResponse<double?>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> GetMinDonatePrice(string streamerId)
    {
        var result = await _streamerService.GetMinDonatePriceAsync(streamerId);
        return Ok(result);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ChangeProfileDataResponse<double>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> ChangeMinDonatePrice(string streamerId, double changeOn)
    {
        var result = await _streamerService.ChangeMinDonatePriceAsync(streamerId, changeOn);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetStreamerProfileResponse<StreamerProfileDto>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> UserProfile(string streamerId)
    {
        var result = await _streamerService.GetStreamerProfileAsync(streamerId);
        return Ok(result);
    }
}