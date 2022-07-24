using ChallengeOrder.API.Models.DTOs;
using ChallengeOrder.API.Models.Responses;

namespace ChallengeOrder.API.Services.Abstractions;

public interface IChallengeOrderService
{
    Task<AddChallengeOrderResponse<Guid?>> AddChallengeOrderAsync(string paymentId, int challengesAmount, double resultDonationPrice);
    Task<UpdateChallengeOrderResponse<Guid>> UpdateChallengeOrderAsync(string challengeOrderId, string paymentId, int challengesAmount, double resultDonationPrice);
    Task<GetChallengeOrderByIdResponse<ChallengeOrderDto?>> GetChallengeOrderByIdAsync(string orderId);
    Task<GetPaginatedChallengeOrdersResponse<ChallengeOrderDto>> GetPaginatedChallengeOrdersAsync(int currentPage, int ordersPerPage);
}