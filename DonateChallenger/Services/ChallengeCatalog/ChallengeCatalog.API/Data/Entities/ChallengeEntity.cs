namespace ChallengeCatalog.API.Data.Entities;

public class ChallengeEntity
{
    public long ChallengeId { get; set; }

    public ChallengeStatusEntity ChallengeStatusEntity { get; set; } = null!;

    public string? Title { get; set; }

    public string Description { get; set; } = null!;

    public double DonatePrice { get; set; }

    public string DonateFrom { get; set; } = null!;

    public string StreamerId { get; set; } = null!;

    public DateTime CreatedTime { get; set; }
}