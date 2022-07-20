using Identity.API.Data;
using Identity.API.Data.Entities;
using Identity.API.Helpers;
using Identity.API.Models.DTOs;
using Identity.API.Models.Responses;
using Identity.API.Services.Abstractions;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;

namespace Identity.API.Services;

public class UserService : BaseDataService<AppDbContext>, IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UserService(
        IDbContextWrapper<AppDbContext> dbContext,
        ILogger<BaseDataService<AppDbContext>> logger,
        UserManager<ApplicationUser> userManager)
        : base(dbContext, logger)
    {
        _userManager = userManager;
    }

    public async Task<GetUsernamesByUsersIdsResponse<IDictionary<string, string>>> GetUsernamesByUsersIdsAsync(IEnumerable<string> usersIds)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _userManager.Users
                .Where(s => usersIds.Contains(s.Id))
                .Select(s => new
                {
                    s.Id,
                    s.Nickname
                })
                .ToListAsync();

            return new GetUsernamesByUsersIdsResponse<IDictionary<string, string>>
            {
                Data = result.ToDictionary(k => k.Id, v => v.Nickname)
            };
        });
    }

    public async Task<GetUserProfileResponse<UserProfileDto>> GetUserProfile(string userId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            Logger.LogInformation($"{nameof(GetUserProfile)} ---> {nameof(userId)}: {userId}");
            var user = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == userId);
            if (user == null)
            {
                Logger.LogWarning($"{nameof(GetUserProfile)} ---> User with id: {user} doesn't exist");
            }

            return new GetUserProfileResponse<UserProfileDto>
            {
                Data = new UserProfileDto
                {
                    UserId = user?.Id,
                    UserNickname = user?.Nickname
                }
            };
        });
    }

    public async Task<ChangeProfileDataResponse<string>> ChangeUserNickname(string userId, string newNickname)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var methodName = nameof(ChangeUserNickname);

            Logger.LogInformation(
                $"{methodName} ---> {nameof(userId)}: {userId};  {nameof(newNickname)}: {newNickname}");
            if (string.IsNullOrWhiteSpace(newNickname))
            {
                var errorMessage = "Nickname can't not be empty";
                Logger.LogInformation($"{methodName} ---> {errorMessage}");
                return new ChangeProfileDataResponse<string>
                {
                    ValidationErrors = new List<string> {errorMessage},
                    ChangedData = string.Empty
                };
            }

            if (newNickname.Length > ValidationConstants.NicknameMaxLength)
            {
                var errorMessage = $"Nickname symbols can not be more than {ValidationConstants.NicknameMaxLength}";
                Logger.LogInformation($"{methodName} ---> {errorMessage}");
                return new ChangeProfileDataResponse<string>
                {
                    ValidationErrors = new List<string> {errorMessage},
                    ChangedData = newNickname
                };
            }

            if (await _userManager.Users.AnyAsync(s => s.Nickname == newNickname))
            {
                var errorMessage = $"Nickname {newNickname} already exists";
                Logger.LogWarning($"{methodName} ---> {errorMessage}");
                return new ChangeProfileDataResponse<string>
                {
                    ValidationErrors = new List<string> {errorMessage},
                    ChangedData = newNickname
                };
            }

            var streamer = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == userId);
            if (streamer == null)
            {
                Logger.LogWarning($"{methodName} ---> Streamer with id: {streamer} doesn't exist");
                return new ChangeProfileDataResponse<string>
                {
                    ValidationErrors = null,
                    ChangedData = string.Empty
                };
            }

            streamer.Nickname = newNickname;
            var result = await _userManager.UpdateAsync(streamer);
            foreach (var error in result.Errors)
            {
                Logger.LogError($"{methodName} ---> Code: {error.Code}. Description: {error.Description}");
            }

            return new ChangeProfileDataResponse<string>
            {
                ValidationErrors = Enumerable.Empty<string>(),
                Succeeded = true,
                ChangedData = newNickname
            };
        });
    }
}