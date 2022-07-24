namespace ChallengeOrder.API.Data.Entities;

public class ChallengeOrderEntity
{
    public Guid ChallengeOrderId { get; set; }

    public string PaymentId { get; set; } = null!;

    public int ChallengesAmount { get; set; }

    public double ResultDonationPrice { get; set; }

    public DateTime Date { get; set; }
}