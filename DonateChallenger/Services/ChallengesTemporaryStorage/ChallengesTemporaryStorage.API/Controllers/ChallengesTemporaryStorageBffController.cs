using System.Net;
using ChallengesTemporaryStorage.API.Models;
using ChallengesTemporaryStorage.API.Services.Abstractions;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChallengesTemporaryStorage.API.Controllers;

[ApiController]
[Scope("challenges-temporary-storage.bff")]
[Route(Defaults.DefaultRoute)]
public class ChallengesTemporaryStorageBffController : ControllerBase
{
    private readonly IChallengesTemporaryStorageService _temporaryStorage;

    public ChallengesTemporaryStorageBffController(IChallengesTemporaryStorageService temporaryStorage) => _temporaryStorage = temporaryStorage;

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GetChallengesTemporaryStorageResponse<string>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get()
    {
        var response = await _temporaryStorage.GetAsync<string>();

        return Ok(response);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(UpdateChallengesTemporaryStorageRequest<string> request)
    {
        var isUpdated = await _temporaryStorage.UpdateAsync(request.Data);

        return Ok(isUpdated);
    }
}