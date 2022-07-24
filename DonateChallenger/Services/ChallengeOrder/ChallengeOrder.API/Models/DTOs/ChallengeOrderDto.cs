namespace ChallengeOrder.API.Models.DTOs;

public class ChallengeOrderDto
{
    public Guid ChallengeOrderId { get; set; }

    public string PaymentId { get; set; } = null!;

    public int ChallengesAmount { get; set; }

    public double ResultDonationPrice { get; set; }

    public DateTime Date { get; set; }
}