namespace ChallengeCatalog.API.Models.Requests;

public class AddChallengeForStreamerRequest
{
    public string? Title { get; set; }

    public string Description { get; set; } = null!;

    public decimal DonatePrice { get; set; }

    public string DonateFrom { get; set; } = null!;
}