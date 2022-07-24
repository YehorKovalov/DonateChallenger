using ChallengeOrder.API.Data;
using ChallengeOrder.API.Data.Entities;

namespace ChallengeOrder.API.Repositories.Abstractions;

public interface IChallengeOrderRepository
{
    Task<Guid> Add(string paymentId, int challengesAmount, double resultDonationPrice);
    Task<Guid> Update(string challengeOrderId, string paymentId, int challengesAmount, double resultDonationPrice);
    Task<ChallengeOrderEntity?> GetById(string orderId);
    Task<PaginatedOrders> GetPaginatedOrders(int currentPage, int ordersPerPage);
}