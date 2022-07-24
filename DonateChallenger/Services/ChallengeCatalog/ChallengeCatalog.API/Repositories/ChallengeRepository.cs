using ChallengeCatalog.API.Data;
using ChallengeCatalog.API.Data.Entities;
using ChallengeCatalog.API.Repositories.Abstractions;
using Infrastructure.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ChallengeCatalog.API.Repositories;

public class ChallengeCatalogCatalogRepository : IChallengeCatalogRepository
{
    private readonly ChallengeCatalogDbContext _dbContext;
    private readonly ILogger<ChallengeCatalogCatalogRepository> _logger;

    public ChallengeCatalogCatalogRepository(
        IDbContextWrapper<ChallengeCatalogDbContext> dbContext,
        ILogger<ChallengeCatalogCatalogRepository> logger)
    {
        _logger = logger;
        _dbContext = dbContext.DbContext;
    }

    public async Task<long?> AddChallengeForStreamerAsync(string description, double donatePrice, string donateFrom, string streamerId, string? title)
    {
        var challenge = new ChallengeEntity
        {
            Description = description,
            DonateFrom = donateFrom,
            DonatePrice = donatePrice,
            StreamerId = streamerId,
            Title = title,
            CreatedTime = DateTime.UtcNow,
            ChallengeStatusEntity = new ChallengeStatusEntity
            {
                IsCompleted = false,
                IsSkipped = false
            }
        };

        var result = await _dbContext.AddAsync(challenge);
        await _dbContext.SaveChangesAsync();
        return result.Entity.ChallengeId;
    }

    public async Task AddChallengeRangeForStreamerAsync(IEnumerable<ChallengeEntity> challenges)
    {
        _logger.LogInformation($"{nameof(AddChallengeRangeForStreamerAsync)} ---> Challenges Amount: {challenges.Count()}");
        await _dbContext.AddRangeAsync(challenges);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool?> UpdateChallengeStatusByIdAsync(long challengeId, bool skipped, bool completed)
    {
        if (challengeId <= 0)
        {
            _logger.LogError($"{nameof(UpdateChallengeStatusByIdAsync)} ---> bad argument: {nameof(challengeId)} = {challengeId}");
            return null;
        }

        var challengeStatus = new ChallengeStatusEntity
        {
            StatusId = challengeId,
            IsSkipped = skipped,
            IsCompleted = completed
        };

        var result = _dbContext.Update(challengeStatus);
        await _dbContext.SaveChangesAsync();

        return result?.Entity?.StatusId > 0;
    }

    public async Task<long> UpdateChallengeAsync(long challengeId, string? title, string description, double donatePrice, string donateFrom)
    {
        var challenge = await GetChallengeByIdAsync(challengeId);

        if (challenge == null)
        {
            _logger.LogError($"{nameof(UpdateChallengeAsync)} ---> Challenge's not found. Returned 0");
            return 0;
        }

        challenge.Title = title;
        challenge.Description = description;
        challenge.DonatePrice = donatePrice;
        challenge.DonateFrom = donateFrom;
        var result = _dbContext.Update(challenge);

        await _dbContext.SaveChangesAsync();
        return result.Entity.ChallengeId;
    }

    public async Task<ChallengeEntity?> GetChallengeByIdAsync(long challengeId, bool loadRelatedEntities = false)
    {
        if (challengeId <= 0)
        {
            _logger.LogError($"{nameof(GetChallengeByIdAsync)} ---> bad arguments: {nameof(challengeId)} = {challengeId}");
            return null;
        }

        var challenge = await _dbContext.Challenges.FirstOrDefaultAsync(c => c.ChallengeId == challengeId);

        if (challenge == null)
        {
            _logger.LogError($"{nameof(GetChallengeByIdAsync)} ---> challenge's not found");
            return null;
        }

        if (loadRelatedEntities)
        {
            await _dbContext.Entry(challenge).Reference(c => c.ChallengeStatusEntity).LoadAsync();
        }

        return challenge;
    }

    public async Task<PaginatedChallenges> GetPaginatedCurrentChallengesAsync(int currentPage, int challengesPerPage, string streamerId, int? minPriceFilter = null, bool? sortByCreatedTime = true, bool? sortByMinDonatePrice = true, bool getSkippedChallengesFilter = false, bool getCompletedChallengesFilter = false)
    {
        var query = _dbContext.Challenges.AsQueryable();

        query = query.Where(q => q.StreamerId == streamerId);

        if (minPriceFilter.HasValue)
        {
            query = query.Where(p => p.DonatePrice >= minPriceFilter.Value);
        }

        query = HandleSorting(query, sortByCreatedTime, sortByMinDonatePrice);

        query = query.Include(q => q.ChallengeStatusEntity)
            .Where(q => q.ChallengeStatusEntity.IsCompleted == getCompletedChallengesFilter)
            .Where(q => q.ChallengeStatusEntity.IsSkipped == getSkippedChallengesFilter);

        var totalCount = await query.LongCountAsync();

        if (totalCount == 0)
        {
            return new PaginatedChallenges
            {
                TotalCount = 0,
                Challenges = Enumerable.Empty<ChallengeEntity>()
            };
        }

        var challenges = await query
            .Skip(currentPage * challengesPerPage)
            .Take(challengesPerPage)
            .ToListAsync();

        return new PaginatedChallenges
        {
            Challenges = challenges,
            TotalCount = totalCount
        };
    }

    private IQueryable<ChallengeEntity> HandleSorting(IQueryable<ChallengeEntity> query, bool? sortByCreatedTime = true, bool? sortByMinDonatePrice = true)
    {
        var byCreatedTime = sortByCreatedTime.HasValue && sortByCreatedTime.Value;
        var byMinDonatePrice = sortByMinDonatePrice.HasValue && sortByMinDonatePrice.Value;
        if (byCreatedTime && byMinDonatePrice)
        {
            return query
                .OrderByDescending(o => o.DonatePrice)
                .ThenByDescending(t => t.CreatedTime);
        }

        if (sortByCreatedTime.HasValue && sortByCreatedTime.Value)
        {
            query = query.OrderByDescending(q => q.CreatedTime);
        }

        if (sortByMinDonatePrice.HasValue && sortByMinDonatePrice.Value)
        {
            query = query.OrderByDescending(q => q.DonatePrice);
        }

        return query;
    }
}