namespace ChallengeCatalog.API.Data.Entities;

public class ChallengeEntity
{
    public long ChallengeId { get; set; }

    public ChallengeStatusEntity ChallengeStatusEntity { get; set; } = null!;

    public string? Title { get; set; }

    public string Description { get; set; } = null!;

    public decimal DonatePrice { get; set; }

    public string DonateFrom { get; set; } = null!;

    public int StreamerId { get; set; }

    public DateTime CreatedTime { get; set; }
}