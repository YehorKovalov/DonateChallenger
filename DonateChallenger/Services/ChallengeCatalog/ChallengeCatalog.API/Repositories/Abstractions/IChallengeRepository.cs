using ChallengeCatalog.API.Data;
using ChallengeCatalog.API.Data.Entities;

namespace ChallengeCatalog.API.Repositories.Abstractions;

public interface IChallengeCatalogRepository
{
    Task<long?> AddChallengeForStreamerAsync(string description, double donatePrice, string donateFrom, string streamerId, string? title);
    Task<PaginatedChallenges> GetPaginatedCurrentChallengesAsync(int currentPage, int challengesPerPage, string streamerId, int? minPriceFilter = null, int? sortByCreatedTime = 0, bool sortBySkipped = false, bool sortByCompleted = false);
    Task<bool?> UpdateChallengeStatusByIdAsync(long challengeId, bool skipped, bool completed);
    Task<ChallengeEntity?> GetChallengeByIdAsync(long challengeId, bool loadRelatedEntities = false);
    Task<bool?> UpdateChallengeAsync(ChallengeEntity challenge);
    Task AddChallengeRangeForStreamerAsync(IEnumerable<ChallengeEntity> challenges);
}