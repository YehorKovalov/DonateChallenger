namespace Infrastructure.MessageBus.Messages;

public interface CatalogChallengeCreatingResponseMessage
{
    public bool Succeeded { get; set; }
    public IEnumerable<string>? ErrorMessages { get; set; }
    public long ChallengeId { get; set; }
}