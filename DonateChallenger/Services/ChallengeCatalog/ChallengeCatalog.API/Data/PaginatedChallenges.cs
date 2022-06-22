using ChallengeCatalog.API.Data.Entities;

namespace ChallengeCatalog.API.Data;

public class PaginatedChallenges
{
    public long TotalCount { get; set; }

    public IEnumerable<ChallengeEntity> Challenges { get; set; } = null!;
}