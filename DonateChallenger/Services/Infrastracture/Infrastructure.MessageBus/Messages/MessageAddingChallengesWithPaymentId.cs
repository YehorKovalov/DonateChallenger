namespace Infrastructure.MessageBus.Messages;

public interface MessageAddingChallengesAndPaymentId
{
    public bool AddingIsSucceeded { get; set; }
    public string PaymentId { get; set; }
    public int ChallengesAmount { get; set; }
    public double ResultDonationPrice { get; set; }
}