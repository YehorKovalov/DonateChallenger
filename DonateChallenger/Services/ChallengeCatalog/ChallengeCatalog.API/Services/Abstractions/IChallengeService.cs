using ChallengeCatalog.API.Models;
using ChallengeCatalog.API.Models.DTOs;
using ChallengeCatalog.API.Models.Requests;
using ChallengeCatalog.API.Models.Response;

namespace ChallengeCatalog.API.Services.Abstractions;

public interface IChallengeCatalogService
{
    Task<AddChallengeForStreamerResponse<long?>?> AddChallengeForStreamerAsync(string donateFrom, string streamerId, double donatePrice, string description, string? title);
    Task<AddChallengeRangeForStreamerResponse> AddChallengeRangeForStreamerAsync(IEnumerable<ChallengeForAddingDto> challengeDtos);

    Task<GetPaginatedChallengesResponse<CurrentChallengeDto>?> GetPaginatedCurrentChallengesAsync(GetPaginatedStreamerChallengesRequest<ChallengeFilter, SortChallengeBy> request);

    Task<GetPaginatedChallengesResponse<CompletedChallengeDto>?> GetPaginatedCompletedChallengesAsync(GetPaginatedStreamerChallengesRequest<ChallengeFilter, SortChallengeBy> request);

    Task<GetPaginatedChallengesResponse<SkippedChallengeDto>?> GetPaginatedSkippedChallengesAsync(GetPaginatedStreamerChallengesRequest<ChallengeFilter, SortChallengeBy> request);

    Task<bool?> SkipChallengeAsync(long? challengeId);

    Task<bool?> CompleteChallengeAsync(long? challengeId);

    Task<UpdateChallengeResponse<long>> UpdateChallengeAsync(long challengeId, string? title, string description, double donatePrice, string streamerId, string donateFrom);
    Task<GetChallengeByIdResponse<ChallengeDto?>> GetChallengeByIdAsync(long challengeId, bool loadRelatedEntities = false);
}