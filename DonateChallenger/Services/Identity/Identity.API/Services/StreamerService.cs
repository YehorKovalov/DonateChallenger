using Identity.API.Data;
using Identity.API.Models.Responses;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Services;

public class StreamerService : BaseDataService<AppDbContext>
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

    public async Task<SearchStreamersNicknamesResponse<string>> FindStreamerByNickname(string nickname)
    {
        if (!await _userManager.Users.AnyAsync())
        {
            _userManager.Logger.LogInformation($"{nameof(FindStreamerByNickname)} ---> nicknames list is empty");
            return new SearchStreamersNicknamesResponse<string>
            {
                Data = Enumerable.Empty<string>()
            };
        }
        
        var nicknames = await _userManager
            .Users.Select(s => s.Nickname).Where(w => w.Contains(nickname))
            .ToListAsync();

        _userManager.Logger.LogInformation($"{nameof(FindStreamerByNickname)} ---> nicknames amount: {nicknames.Count}");
        return new SearchStreamersNicknamesResponse<string>
        {
            Data = nicknames
        };
    }
}