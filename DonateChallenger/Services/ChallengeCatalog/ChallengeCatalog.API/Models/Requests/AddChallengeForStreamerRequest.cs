namespace ChallengeCatalog.API.Models.Requests;

public class AddChallengeForStreamerRequest
{
    public string? Title { get; set; }

    public string Description { get; set; } = null!;

    public double DonatePrice { get; set; }

    public string StreamerId { get; set; } = null!;

    public string DonateFrom { get; set; } = null!;
}