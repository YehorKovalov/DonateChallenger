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
[Authorize(Policy = AuthPolicy.ManagerMinimumPolicy)]
[Scope("challenge-catalog.manager")]
[Route(Defaults.DefaultRoute)]
public class ChallengeManagerController : ControllerBase
{
    private readonly IChallengeCatalogService _catalog;

    public ChallengeManagerController(IChallengeCatalogService catalog) => _catalog = catalog;

    [HttpGet]
    [ProducesResponseType(typeof(GetChallengeByIdResponse<ChallengeDto?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(long challengeId)
    {
        var result = await _catalog.GetChallengeByIdAsync(challengeId, false);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetPaginatedChallengesResponse<CurrentChallengeDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Current(GetPaginatedStreamerChallengesRequest<ChallengeFilter, SortChallengeBy> request)
    {
        var result = await _catalog.GetPaginatedCurrentChallengesAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetPaginatedChallengesResponse<SkippedChallengeDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Skipped(GetPaginatedStreamerChallengesRequest<ChallengeFilter, SortChallengeBy> request)
    {
        var result = await _catalog.GetPaginatedSkippedChallengesAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetPaginatedChallengesResponse<CompletedChallengeDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Completed(GetPaginatedStreamerChallengesRequest<ChallengeFilter, SortChallengeBy> request)
    {
        var result = await _catalog.GetPaginatedCompletedChallengesAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UpdateChallengeResponse<long>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(UpdateChallengeRequest request)
    {
        var result = await _catalog.UpdateChallengeAsync(request.ChallengeId, request.Title, request.Description, request.DonatePrice, request.StreamerId, request.DonateFrom);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddChallengeForStreamerResponse<long>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add(AddChallengeForStreamerRequest request)
    {
        var result = await _catalog.AddChallengeForStreamerAsync(request.DonateFrom, request.StreamerId, request.DonatePrice, request.Description, request.Title);
        return Ok(result);
    }
}