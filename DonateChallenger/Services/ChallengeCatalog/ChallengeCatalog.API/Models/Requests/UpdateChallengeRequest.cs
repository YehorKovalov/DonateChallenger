namespace ChallengeCatalog.API.Models.Requests;

public class UpdateChallengeRequest
{
    public long ChallengeId { get; set; }

    public string? Title { get; set; }

    public string Description { get; set; } = null!;

    public double DonatePrice { get; set; }

    public string DonateFrom { get; set; } = null!;
}