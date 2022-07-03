using System.Net;
using Identity.API.Models.Requests;
using Identity.API.Models.Responses;
using Identity.API.Services.Abstractions;
using Infrastructure;

namespace Identity.API.Controllers;

[SecurityHeaders]
[ApiController]
[Route(Defaults.DefaultRoute)]
public class StreamerBffController : ControllerBase
{
    private readonly IStreamerService _streamerService;

    public StreamerBffController(IStreamerService streamerService) =>_streamerService = streamerService;

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SearchStreamersNicknamesResponse<string>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> SearchNicknamesAsync(SearchStreamersNicknamesRequest request)
    {
        var result = await _streamerService.FindStreamerByNickname(request.NicknameAsFilter);
        return Ok(result);
    }
}