using ChallengeOrder.API.Models.Responses;

namespace ChallengeOrder.API.Services.Abstractions;

public interface IChallengeOrderService
{
    Task<AddChallengeOrderResponse<Guid?>> AddChallengeOrderAsync(string paymentId, int challengesAmount, double resultDonationPrice);
}