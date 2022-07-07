namespace Infrastructure.MessageBus.Messages;

public interface PaymentCreatingResponseMessage
{
    public bool Succeeded { get; set; }
    public IEnumerable<string>? ErrorMessages { get; set; }
    public Guid PaymentId { get; set; }
}