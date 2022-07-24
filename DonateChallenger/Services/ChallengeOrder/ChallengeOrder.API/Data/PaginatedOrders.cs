using ChallengeOrder.API.Data.Entities;

namespace ChallengeOrder.API.Data;

public class PaginatedOrders
{
    public long TotalCount { get; set; }

    public IEnumerable<ChallengeOrderEntity> Challenges { get; set; } = null!;
}