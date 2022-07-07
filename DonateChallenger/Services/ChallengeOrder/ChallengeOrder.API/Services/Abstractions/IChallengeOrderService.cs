using ChallengeOrder.API.Models.Responses;

namespace ChallengeOrder.API.Services.Abstractions;

public interface IChallengeOrderService
{
    Task<AddChallengeOrderResponse<Guid?>> AddChallengeOrderAsync(string description, decimal donatePrice, string streamerId, string donateFrom, string? title);
}