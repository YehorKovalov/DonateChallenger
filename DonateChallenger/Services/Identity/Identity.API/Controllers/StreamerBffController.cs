using System.Net;
using Identity.API.Models.Requests;
using Identity.API.Models.Responses;
using Identity.API.Services.Abstractions;
using Infrastructure;
using Microsoft.AspNetCore.Routing;

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
    public async Task<IActionResult> SearchNicknames(SearchStreamersNicknamesRequest request)
    {
        var result = await _streamerService.FindStreamerByNicknameAsync(request.NicknameAsFilter);
        return Ok(result);
    }

    [HttpGet]
    [Route("mindonateprice/{streamerId:string}")]
    [ProducesResponseType(typeof(GetMinDonatePriceResponse<double?>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> GetMinDonatePrice(string streamerId)
    {
        var result = await _streamerService.GetMinDonatePriceAsync(streamerId);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ChangeMinDonatePriceResponse), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> ChangeMinDonatePrice(string streamerId, double changeOn)
    {
        var result = await _streamerService.ChangeMinDonatePriceAsync(streamerId, changeOn);
        return Ok(result);
    }
}