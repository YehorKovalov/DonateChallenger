using System.Net;
using ChallengeOrder.API.Models.Responses;
using ChallengeOrder.API.Services.Abstractions;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeOrder.API.Controllers;

[Authorize(Policy = AuthPolicy.AdminOnlyPolicy)]
[Scope("challenge-order.bff")]
[ApiController]
[Route(Defaults.DefaultRoute)]
public class ChallengeOrderBffController : ControllerBase
{
    private readonly IChallengeOrderService _challengeOrderService;

    public ChallengeOrderBffController(IChallengeOrderService challengeOrderService) => _challengeOrderService = challengeOrderService;

    [HttpPost]
    [ProducesResponseType(typeof(AddChallengeOrderResponse<Guid>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddOrder(string paymentId, int challengesAmount, double resultDonationPrice)
    {
        var result = await _challengeOrderService.AddChallengeOrderAsync(paymentId, challengesAmount, resultDonationPrice);
        return Ok(result);
    }
}