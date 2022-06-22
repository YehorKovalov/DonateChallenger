namespace ChallengeCatalog.API.Data.Entities;

public class ChallengeStatusEntity
{
    public long StatusId { get; set; }

    public bool IsCompleted { get; set; }

    public bool IsSkipped { get; set; }

    public ChallengeEntity Challenge { get; set; } = null!;
}