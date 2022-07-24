using System.Net;
using ChallengeOrder.API.Models.DTOs;
using ChallengeOrder.API.Models.Requests;
using ChallengeOrder.API.Models.Responses;
using ChallengeOrder.API.Services.Abstractions;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeOrder.API.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.ManagerMinimumPolicy)]
[Route(Defaults.DefaultRoute)]
[Scope("challenge-order.manager")]
public class ChallengeOrderManagerController : ControllerBase
{
    private readonly IChallengeOrderService _orderService;

    public ChallengeOrderManagerController(IChallengeOrderService orderService) => _orderService = orderService;

    [HttpGet]
    [ProducesResponseType(typeof(GetChallengeOrderByIdResponse<ChallengeOrderDto?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(string orderId)
    {
        var result = await _orderService.GetChallengeOrderByIdAsync(orderId);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetPaginatedChallengeOrdersResponse<ChallengeOrderDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(GetPaginatedChallengeOrdersRequest request)
    {
        var result = await _orderService.GetPaginatedChallengeOrdersAsync(request.CurrentPage, request.OrdersPerPage);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UpdateChallengeOrderResponse<Guid>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(UpdateChallengeOrderRequest request)
    {
        var result = await _orderService.UpdateChallengeOrderAsync(request.ChallengeOrderId, request.PaymentId, request.ChallengesAmount, request.ResultDonationPrice);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddChallengeOrderResponse<Guid?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add(AddChallengeOrderRequest request)
    {
        var result = await _orderService.AddChallengeOrderAsync(request.PaymentId, request.ChallengesAmount, request.ResultDonationPrice);
        return Ok(result);
    }
}