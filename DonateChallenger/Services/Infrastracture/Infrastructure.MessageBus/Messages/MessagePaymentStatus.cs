namespace Infrastructure.MessageBus.Messages;

public interface MessagePaymentStatus
{
    public bool Succeeded { get; set; }
    public string PaymentId { get; set; }
}