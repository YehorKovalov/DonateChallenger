using AutoMapper;
using ChallengeCatalog.API.Data;
using ChallengeCatalog.API.Models;
using ChallengeCatalog.API.Models.DTOs;
using ChallengeCatalog.API.Models.Requests;
using ChallengeCatalog.API.Models.Response;
using ChallengeCatalog.API.Repositories.Abstractions;
using ChallengeCatalog.API.Services.Abstractions;
using Infrastructure.Helpers;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;

namespace ChallengeCatalog.API.Services;

public class ChallengeService : BaseDataService<ChallengeCatalogDbContext>, IChallengeService
{
    private readonly IChallengeRepository _challengeRepository;
    private readonly IMapper _mapper;
    public ChallengeService(
        IDbContextWrapper<ChallengeCatalogDbContext> dbContext,
        ILogger<BaseDataService<ChallengeCatalogDbContext>> logger,
        IChallengeRepository challengeRepository,
        IMapper mapper)
            : base(dbContext, logger)
    {
        _challengeRepository = challengeRepository;
        _mapper = mapper;
    }

    public async Task<AddChallengeForStreamerResponse<long?>?> AddChallengeForStreamerAsync(string donateFrom, string streamerId, double donatePrice, string description, string? title)
    {
        return await ExecuteSafeAsync(async () =>
        {
            if (!AddChallengeRequestStateIsValid(donateFrom, streamerId, donatePrice, description))
            {
                Logger.LogError($"{nameof(AddChallengeForStreamerAsync)} ---> Bad request");
                return null!;
            }

            var result = await _challengeRepository.AddChallengeForStreamerAsync(description, donatePrice, donateFrom, streamerId, title);

            return new AddChallengeForStreamerResponse<long?> { ChallengeId = result };
        });
    }

    public async Task<GetPaginatedChallengesResponse<CurrentChallengeDto>?> GetPaginatedCurrentChallengesAsync(GetPaginatedStreamerChallengesRequest<ChallengeFilter> request)
    {
        var result = await GetPaginatedChallengesAsyncInternal(request);

        return result is not null ? _mapper.Map<GetPaginatedChallengesResponse<CurrentChallengeDto>>(result) : null;
    }

    public async Task<GetPaginatedChallengesResponse<CompletedChallengeDto>?> GetPaginatedCompletedChallengesAsync(GetPaginatedStreamerChallengesRequest<ChallengeFilter> request)
    {
        var result = await GetPaginatedChallengesAsyncInternal(request, getCompletedChallenges: true);

        return result is not null ? _mapper.Map<GetPaginatedChallengesResponse<CompletedChallengeDto>>(result) : null;
    }

    public async Task<GetPaginatedChallengesResponse<SkippedChallengeDto>?> GetPaginatedSkippedChallengesAsync(GetPaginatedStreamerChallengesRequest<ChallengeFilter> request)
    {
        var result = await GetPaginatedChallengesAsyncInternal(request, getSkippedChallenges: true);

        return result is not null ? _mapper.Map<GetPaginatedChallengesResponse<SkippedChallengeDto>>(result) : null;
    }

    public async Task<bool?> SkipChallengeAsync(long? challengeId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            if (challengeId is null or <= 0)
            {
                Logger.LogError($"{nameof(SkipChallengeAsync)} ---> Bad arguments {nameof(challengeId)} ---> {challengeId}");
                return null;
            }

            var result = await _challengeRepository.UpdateChallengeStatusByIdAsync(challengeId!.Value, true, false);
            Logger.LogInformation($"{nameof(SkipChallengeAsync)} ---> {nameof(result)} = {result}");

            return result;
        });
    }

    public async Task<bool?> CompleteChallengeAsync(long? challengeId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            if (challengeId is null or <= 0)
            {
                Logger.LogError($"{nameof(CompleteChallengeAsync)} ---> Bad arguments {nameof(challengeId)} ---> {challengeId}");
                return null;
            }

            var result = await _challengeRepository.UpdateChallengeStatusByIdAsync(challengeId!.Value, false, true);
            Logger.LogInformation($"{nameof(CompleteChallengeAsync)} ---> {nameof(result)} = {result}");

            return result;
        });
    }

    private async Task<GetPaginatedChallengesResponse<ChallengeDto>?> GetPaginatedChallengesAsyncInternal(
        GetPaginatedStreamerChallengesRequest<ChallengeFilter> request, bool getSkippedChallenges = false, bool getCompletedChallenges = false)
    {
        return await ExecuteSafeAsync(async () =>
        {
            if (getCompletedChallenges && getSkippedChallenges)
            {
                throw new Exception($"{nameof(GetPaginatedChallengesAsyncInternal)} ---> Logic's been broken. {nameof(getCompletedChallenges)} and {nameof(getSkippedChallenges)} can not be true both at the same time");
            }

            if (!GetPaginatedChallengesRequestStateIsValid(request))
            {
                return null;
            }

            HandleFilters(request.Filters, out var minPriceFilter, out var sortByCreatedTimeFilter);

            var result = await _challengeRepository.GetPaginatedCurrentChallengesAsync(request.CurrentPage, request.ChallengesPerPage, request.StreamerId, minPriceFilter, sortByCreatedTimeFilter, getSkippedChallenges, getCompletedChallenges);

            return new GetPaginatedChallengesResponse<ChallengeDto>
            {
                TotalCount = result.TotalCount,
                TotalPages = PageCounter.Count(result.TotalCount, request.ChallengesPerPage),
                ChallengesPerPage = request.ChallengesPerPage,
                CurrentPage = request.CurrentPage,
                Data = result.Challenges.Select(s => _mapper.Map<ChallengeDto>(s)).ToList()
            };
        });
    }

    private void HandleFilters(IDictionary<ChallengeFilter, int>? filters, out int? minPriceFilter, out int? sortByCreatedTimeFilter)
    {
        minPriceFilter = null;
        sortByCreatedTimeFilter = null;
        if (filters == null)
        {
            return;
        }

        if (filters!.TryGetValue(ChallengeFilter.MinPriceFilter, out var minPrice))
        {
            minPriceFilter = minPrice;
        }

        if (filters!.TryGetValue(ChallengeFilter.SortByCreatedTime, out var sortByCreatedTime))
        {
            sortByCreatedTimeFilter = sortByCreatedTime;
        }
    }

    private bool AddChallengeRequestStateIsValid(string donateFrom, string streamerId, double donatePrice, string description)
    {
        return !string.IsNullOrWhiteSpace(description)
               && !string.IsNullOrWhiteSpace(donateFrom)
               && !string.IsNullOrWhiteSpace(streamerId)
               && donatePrice > 0;
    }

    private bool GetPaginatedChallengesRequestStateIsValid(GetPaginatedStreamerChallengesRequest<ChallengeFilter> request)
    {
        return request.CurrentPage >= 0
               && request.ChallengesPerPage > 0;
    }
}