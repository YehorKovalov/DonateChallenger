namespace ChallengeOrder.API.Repositories.Abstractions;

public interface IChallengeOrderRepository
{
    Task<Guid> Add(string paymentId, int challengesAmount, double resultDonationPrice);
}