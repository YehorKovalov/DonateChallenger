using Identity.API.Data;
using Identity.API.Data.Entities;
using Identity.API.Helpers;
using Identity.API.Models.DTOs;
using Identity.API.Models.Responses;
using Identity.API.Services.Abstractions;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;

namespace Identity.API.Services;

public class StreamerService : BaseDataService<AppDbContext>, IStreamerService
{
    private readonly UserManager<ApplicationUser> _userManager;
    public StreamerService(
        IDbContextWrapper<AppDbContext> dbContext,
        ILogger<BaseDataService<AppDbContext>> logger,
        UserManager<ApplicationUser> userManager)
            : base(dbContext, logger)
    {
        _userManager = userManager;
    }

    public async Task<SearchStreamersByNicknameResponse<SearchedStreamerByNicknameDto>> FindStreamerByNicknameAsync(string nickname)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var nicknameToSearch = nickname.Trim().ToLower();
            Logger.LogInformation($"{nameof(FindStreamerByNicknameAsync)} ---> {nameof(nickname)} to search: {nickname}");

            var users = await _userManager.GetUsersInRoleAsync("streamer");

            var streamers = users
                .Where(w => w.Nickname.ToLower().Contains(nicknameToSearch))
                .Select(s => new SearchedStreamerByNicknameDto
                {
                    StreamerId = s.Id,
                    StreamerNickname = s.Nickname,
                    MerchantId = s.MerchantId,
                    MinDonatePrice = s.MinDonatePriceInDollars
                })
                .ToList();

            _userManager.Logger.LogInformation($"{nameof(FindStreamerByNicknameAsync)} ---> nicknames amount: {streamers.Count}");

            return new SearchStreamersByNicknameResponse<SearchedStreamerByNicknameDto> { Data = streamers };
        });
    }

    public async Task<GetStreamerProfileResponse<StreamerProfileDto>> GetStreamerProfileAsync(string streamerId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            Logger.LogInformation($"{nameof(GetStreamerProfileAsync)} ---> {nameof(streamerId)}: {streamerId}");
            var streamer = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == streamerId);
            if (streamer == null)
            {
                Logger.LogWarning($"{nameof(GetStreamerProfileAsync)} ---> Streamer with id: {streamer} doesn't exist");
            }

            return new GetStreamerProfileResponse<StreamerProfileDto>
            {
                Data = new StreamerProfileDto
                {
                    Email = streamer?.Email,
                    StreamerId = streamer?.Id,
                    StreamerNickname = streamer?.Nickname,
                    MinDonatePrice = streamer.MinDonatePriceInDollars,
                    MerchantId = streamer?.Email
                }
            };
        });
    }
    
    public async Task<GetMinDonatePriceResponse<double?>> GetMinDonatePriceAsync(string streamerId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            Logger.LogInformation($"{nameof(GetMinDonatePriceAsync)} ---> {nameof(streamerId)}: {streamerId}");
            var streamer = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == streamerId);
            if (streamer == null)
            {
                Logger.LogWarning($"{nameof(GetMinDonatePriceAsync)} ---> Streamer with id: {streamer} doesn't exist");
            }

            return new GetMinDonatePriceResponse<double?> { Data = streamer?.MinDonatePriceInDollars };
        });
    }

    public async Task<ChangeProfileDataResponse<double>> ChangeMinDonatePriceAsync(string streamerId, double changeOn)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var methodName = nameof(ChangeMinDonatePriceAsync);

            Logger.LogInformation($"{methodName} ---> {nameof(streamerId)}: {streamerId};  {nameof(changeOn)}: {changeOn}");
            if (changeOn < ValidationConstants.LogicalMinimumDonatePrice)
            {
                var errorMessage = $"Price cannot be less than {ValidationConstants.LogicalMinimumDonatePrice}";
                Logger.LogInformation($"{methodName} ---> {errorMessage}");
                return new ChangeProfileDataResponse<double>
                {
                    ValidationErrors = new List<string> { errorMessage },
                    ChangedData = 0
                };
            }

            if (changeOn > ValidationConstants.LogicalMaximumMinimumDonatePrice)
            {
                var errorMessage = $"Price cannot be more than {ValidationConstants.LogicalMaximumMinimumDonatePrice}";
                Logger.LogInformation($"{methodName} ---> {errorMessage}");
                return new ChangeProfileDataResponse<double>
                {
                    ValidationErrors = new List<string> { errorMessage },
                    ChangedData = 0
                };
            }
            
            var streamer = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == streamerId);
            if (streamer == null)
            {
                Logger.LogWarning($"{methodName} ---> Streamer with id: {streamer} doesn't exist");
                return new ChangeProfileDataResponse<double>
                {
                    ValidationErrors = null,
                    ChangedData = 0
                };
            }

            streamer.MinDonatePriceInDollars = changeOn;
            var result = await _userManager.UpdateAsync(streamer);
            foreach (var error in result.Errors)
            {
                Logger.LogError($"{methodName} ---> Code: {error.Code}. Description: {error.Description}");
            }

            return new ChangeProfileDataResponse<double>
            {
                ValidationErrors = Enumerable.Empty<string>(),
                Succeeded = true,
                ChangedData = changeOn
            };
        });
    }
}