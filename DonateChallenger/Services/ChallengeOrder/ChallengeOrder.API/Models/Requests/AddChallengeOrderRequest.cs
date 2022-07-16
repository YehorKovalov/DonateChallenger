namespace ChallengeOrder.API.Models.Requests;

public class AddChallengeOrderRequest
{
    public string PaymentId { get; set; } = null!;
    public int ChallengesAmount { get; set; }
    public double ResultDonationPrice { get; set; }
}