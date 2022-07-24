using ChallengeCatalog.API.Data;
using ChallengeCatalog.API.Data.Entities;

namespace ChallengeCatalog.API.Repositories.Abstractions;

public interface IChallengeCatalogRepository
{
    Task<long?> AddChallengeForStreamerAsync(string description, double donatePrice, string donateFrom, string streamerId, string? title);
    Task<PaginatedChallenges> GetPaginatedCurrentChallengesAsync(int currentPage, int challengesPerPage, string streamerId, int? minPriceFilter = null, bool? sortByCreatedTime = false, bool? sortByMinDonatePrice = false, bool getSkippedChallengesFilter = false, bool getCompletedChallengesFilter = false);
    Task<bool?> UpdateChallengeStatusByIdAsync(long challengeId, bool skipped, bool completed);
    Task<long> UpdateChallengeAsync(long challengeId, string? title, string description, double donatePrice, string donateFrom);
    Task<ChallengeEntity?> GetChallengeByIdAsync(long challengeId, bool loadRelatedEntities = false);
    Task AddChallengeRangeForStreamerAsync(IEnumerable<ChallengeEntity> challenges);
}