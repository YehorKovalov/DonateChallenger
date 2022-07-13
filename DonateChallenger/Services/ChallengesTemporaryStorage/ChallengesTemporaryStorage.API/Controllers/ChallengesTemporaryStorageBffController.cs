using System.Net;
using ChallengesTemporaryStorage.API.Models;
using ChallengesTemporaryStorage.API.Services.Abstractions;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ChallengesTemporaryStorage.API.Controllers;

[ApiController]
[Route(Defaults.DefaultRoute)]
public class ChallengesTemporaryStorageBffController : ControllerBase
{
    private readonly IChallengesTemporaryStorageService _temporaryStorage;

    public ChallengesTemporaryStorageBffController(IChallengesTemporaryStorageService temporaryStorage) => _temporaryStorage = temporaryStorage;

    [HttpPost]
    [ProducesResponseType(typeof(GetChallengesTemporaryStorageResponse<string>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get()
    {
        var response = await _temporaryStorage.GetAsync<string>();

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(UpdateChallengesTemporaryStorageRequest<string> request)
    {
        var isUpdated = await _temporaryStorage.UpdateAsync(request.Data);

        return Ok(isUpdated);
    }
}