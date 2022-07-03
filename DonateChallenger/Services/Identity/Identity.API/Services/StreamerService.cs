using Identity.API.Data;
using Identity.API.Models.Responses;
using Identity.API.Services.Abstractions;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

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

    public async Task<SearchStreamersNicknamesResponse<string>> FindStreamerByNicknameAsync(string nickname)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var nicknames = await _userManager
                .Users.Select(s => s.Nickname).Where(w => w.Contains(nickname))
                .ToListAsync();

            _userManager.Logger.LogInformation($"{nameof(FindStreamerByNicknameAsync)} ---> nicknames amount: {nicknames.Count}");
            return new SearchStreamersNicknamesResponse<string>
            {
                Data = nicknames
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

            return new GetMinDonatePriceResponse<double?>
            {
                Data = streamer?.MinDonatePriceInDollars
            };
        });
    }

    public async Task<ChangeMinDonatePriceResponse> ChangeMinDonatePriceAsync(string streamerId, double changeOn)
    {
        return await ExecuteSafeAsync(async () =>
        {
            Logger.LogInformation($"{nameof(ChangeMinDonatePriceAsync)} ---> {nameof(streamerId)}: {streamerId};  {nameof(changeOn)}: {changeOn}");
            const double logicalMinimumDonatePrice = 0.1;
            if (changeOn < logicalMinimumDonatePrice)
            {
                Logger.LogInformation($"{nameof(ChangeMinDonatePriceAsync)} ---> Arrived minimum donate price is less than logical minimum price({logicalMinimumDonatePrice}$");
                return new ChangeMinDonatePriceResponse { Succeeded = false };
            }

            var streamer = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == streamerId);
            if (streamer == null)
            {
                Logger.LogWarning($"{nameof(ChangeMinDonatePriceAsync)} ---> Streamer with id: {streamer} doesn't exist");
                return new ChangeMinDonatePriceResponse { Succeeded = false };
            }

            streamer.MinDonatePriceInDollars = changeOn;
            var result = await _userManager.UpdateAsync(streamer);
            foreach (var error in result.Errors)
            {
                Logger.LogError($"{nameof(ChangeMinDonatePriceAsync)} ---> Code: {error.Code}. Description: {error.Description}");
            }

            return new ChangeMinDonatePriceResponse { Succeeded = result.Succeeded };
        });
    }
}