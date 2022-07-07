using ChallengeOrder.API.Models.Requests;
using ChallengeOrder.API.Models.Responses;
using ChallengeOrder.API.Services.Abstractions;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeOrder.API.Controllers;

[ApiController]
[Route(Defaults.DefaultRoute)]
[Scope("challengeOrder")]
[Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
public class ChallengeOrderBffController : ControllerBase
{
    private readonly IChallengeOrderService _orderService;

    public ChallengeOrderBffController(IChallengeOrderService orderService) => _orderService = orderService;

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AddChallengeOrderResponse<Guid?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> BuildOrder(BuildChallengeOrderRequest request)
    {
        var result = await _orderService
            .AddChallengeOrderAsync(
                request.ChallengeToAdd.Description,
                request.ChallengeToAdd.DonatePrice,
                request.ChallengeToAdd.StreamId,
                request.ChallengeToAdd.DonateFrom,
                request.ChallengeToAdd.Title);
        return Ok(result);
    }
}