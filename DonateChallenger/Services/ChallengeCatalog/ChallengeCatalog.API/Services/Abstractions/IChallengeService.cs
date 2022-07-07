using ChallengeCatalog.API.Models;
using ChallengeCatalog.API.Models.DTOs;
using ChallengeCatalog.API.Models.Requests;
using ChallengeCatalog.API.Models.Response;

namespace ChallengeCatalog.API.Services.Abstractions;

public interface IChallengeService
{
    Task<AddChallengeForStreamerResponse<long?>?> AddChallengeForStreamerAsync(string donateFrom, string streamerId, double donatePrice, string description, string? title);

    Task<GetPaginatedChallengesResponse<CurrentChallengeDto>?> GetPaginatedCurrentChallengesAsync(GetPaginatedStreamerChallengesRequest<ChallengeFilter> request);

    Task<GetPaginatedChallengesResponse<CompletedChallengeDto>?> GetPaginatedCompletedChallengesAsync(GetPaginatedStreamerChallengesRequest<ChallengeFilter> request);

    Task<GetPaginatedChallengesResponse<SkippedChallengeDto>?> GetPaginatedSkippedChallengesAsync(GetPaginatedStreamerChallengesRequest<ChallengeFilter> request);

    Task<bool?> SkipChallengeAsync(long? challengeId);

    Task<bool?> CompleteChallengeAsync(long? challengeId);
}