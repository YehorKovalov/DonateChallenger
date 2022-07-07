namespace Infrastructure.MessageBus.Messages;

public interface CreateCatalogChallengeRequestMessage
{
    public string Description { get; set; }

    public string StreamerId { get; set; }

    public double DonatePrice { get; set; }

    public string DonateFrom { get; set; }

    public string? Title { get; set; }
}