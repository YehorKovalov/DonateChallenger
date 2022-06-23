namespace ChallengeCatalog.API.Models.Requests;

public class AddChallengeForStreamerRequest<TStreamerId>
{
    public string? Title { get; set; }

    public string Description { get; set; } = null!;

    public decimal DonatePrice { get; set; }

    public string DonateFrom { get; set; } = null!;

    public TStreamerId StreamerId { get; set; } = default !;
}