namespace ChallengeOrder.API.Repositories.Abstractions;

public interface IChallengeOrderRepository
{
    Task<Guid> Add(Guid paymentId, long catalogChallengeId);
}