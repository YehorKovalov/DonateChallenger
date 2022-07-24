namespace ChallengeOrder.API.Models.Requests;

public class UpdateChallengeOrderRequest
{
    public string ChallengeOrderId { get; set; } = null!;
    public string PaymentId { get; set; } = null!;
    public int ChallengesAmount { get; set; }
    public double ResultDonationPrice { get; set; }
}