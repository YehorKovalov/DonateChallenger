using System.Net;
using ChallengeCatalog.API.Models;
using ChallengeCatalog.API.Models.DTOs;
using ChallengeCatalog.API.Models.Requests;
using ChallengeCatalog.API.Models.Response;
using ChallengeCatalog.API.Services.Abstractions;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeCatalog.API.Controllers;

[ApiController]
[Scope("challengeCatalog")]
[Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
[Route(Defaults.DefaultRoute)]
public class ChallengesBoardBffController : ControllerBase
{
    private readonly IChallengeService _challengeService;

    public ChallengesBoardBffController(IChallengeService challengeService) => _challengeService = challengeService;

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AddChallengeForStreamerResponse<long?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add(AddChallengeForStreamerRequest request)
    {
        var result = await _challengeService.AddChallengeForStreamerAsync(request.DonateFrom, request.StreamerId, request.DonatePrice, request.Description, request.Title);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GetPaginatedChallengesResponse<CurrentChallengeDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Current(GetPaginatedStreamerChallengesRequest<ChallengeFilter> request)
    {
        var result = await _challengeService.GetPaginatedCurrentChallengesAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(bool?), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Skip(long? challengeId)
    {
        var result = await _challengeService.SkipChallengeAsync(challengeId);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(bool?), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Complete(long? challengeId)
    {
        var result = await _challengeService.CompleteChallengeAsync(challengeId);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GetPaginatedChallengesResponse<SkippedChallengeDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Skipped(GetPaginatedStreamerChallengesRequest<ChallengeFilter> request)
    {
        var result = await _challengeService.GetPaginatedSkippedChallengesAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GetPaginatedChallengesResponse<CompletedChallengeDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Completed(GetPaginatedStreamerChallengesRequest<ChallengeFilter> request)
    {
        var result = await _challengeService.GetPaginatedCompletedChallengesAsync(request);
        return Ok(result);
    }
}