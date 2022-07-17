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
[Authorize(Policy = AuthPolicy.StreamerPolicy)]
[Scope("challenge-catalog.bff")]
[Route(Defaults.DefaultRoute)]
public class ChallengesBoardBffController : ControllerBase
{
    private readonly IChallengeCatalogService _challengeCatalogService;

    public ChallengesBoardBffController(IChallengeCatalogService challengeCatalogService) => _challengeCatalogService = challengeCatalogService;

    [HttpPost]
    [ProducesResponseType(typeof(GetPaginatedChallengesResponse<CurrentChallengeDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Current(GetPaginatedStreamerChallengesRequest<ChallengeFilter, SortChallengeBy> request)
    {
        var result = await _challengeCatalogService.GetPaginatedCurrentChallengesAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(bool?), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Skip(long? challengeId)
    {
        var result = await _challengeCatalogService.SkipChallengeAsync(challengeId);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(bool?), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Complete(long? challengeId)
    {
        var result = await _challengeCatalogService.CompleteChallengeAsync(challengeId);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetPaginatedChallengesResponse<SkippedChallengeDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Skipped(GetPaginatedStreamerChallengesRequest<ChallengeFilter, SortChallengeBy> request)
    {
        var result = await _challengeCatalogService.GetPaginatedSkippedChallengesAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetPaginatedChallengesResponse<CompletedChallengeDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Completed(GetPaginatedStreamerChallengesRequest<ChallengeFilter, SortChallengeBy> request)
    {
        var result = await _challengeCatalogService.GetPaginatedCompletedChallengesAsync(request);
        return Ok(result);
    }
}