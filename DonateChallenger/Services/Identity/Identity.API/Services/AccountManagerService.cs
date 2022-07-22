using Identity.API.Data;
using Identity.API.Data.Entities;
using Identity.API.Models.DTOs;
using Identity.API.Models.Requests;
using Identity.API.Models.Responses;
using Identity.API.Services.Abstractions;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;

namespace Identity.API.Services;

public class AccountManagerService : BaseDataService<AppDbContext>, IAccountManagerService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly IStreamerService _streamerService;
    public AccountManagerService(
        IDbContextWrapper<AppDbContext> dbContext,
        ILogger<BaseDataService<AppDbContext>> logger,
        UserManager<ApplicationUser> userManager,
        IUserService userService,
        IStreamerService streamerService)
        : base(dbContext, logger)
    {
        _userManager = userManager;
        _userService = userService;
        _streamerService = streamerService;
        _dbContext = dbContext.DbContext;
    }

    public async Task<UserProfileDto> GetUserProfileById(string userId)
    {
        var response = await _userService.GetUserProfileById(userId);
        return response.Data;
    }

    public async Task<StreamerProfileDto> GetStreamerProfileById(string userId)
    {
        var response = await _streamerService.GetStreamerProfileAsync(userId);
        return response.Data;
    }

    public async Task<DeleteUserByIdResponse<bool>> DeleteUserByIdAsync(string userId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new DeleteUserByIdResponse<bool> { Data = false };
            }
            
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                Logger.LogError($"{nameof(DeleteUserByIdAsync)} ---> User doesn't exist");
                return new DeleteUserByIdResponse<bool> { Data = false };
            }

            var result = await _userManager.DeleteAsync(user);
            return new DeleteUserByIdResponse<bool> { Data = result.Succeeded };
        });
    }

    public async Task<ManagerUpdateProfileResponse> UpdateUserProfileAsync(ManagerUpdateUserProfileRequest request)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var methodName = nameof(UpdateUserProfileAsync);
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                var errorMessage = "User doesn't exist";
                Logger.LogError($"{methodName} ---> {errorMessage}");
                return new ManagerUpdateProfileResponse
                {
                    Succeeded = false,
                    Errors = new[] {errorMessage}
                };
            }

            var errorWhileUpdatingEmail = await TryUpdateUserEmail(user, request.Email);

            if (!string.IsNullOrWhiteSpace(errorWhileUpdatingEmail))
            {
                return new ManagerUpdateProfileResponse
                {
                    Succeeded = false,
                    Errors = new[] {errorWhileUpdatingEmail}
                };
            }

            user.Nickname = request.Nickname;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                LogErrorsIfNotSucceeded(result, methodName);
                return new ManagerUpdateProfileResponse
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(s => s.Description).ToList()
                };
            }

            Logger.LogInformation($"{methodName} ---> User profile updated");
            return new ManagerUpdateProfileResponse
            {
                Succeeded = true,
                Errors = Enumerable.Empty<string>()
            };
        });
    }

    public async Task<ManagerUpdateProfileResponse> UpdateStreamerProfileAsync(ManagerUpdateStreamerProfileRequest request)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var methodName = nameof(UpdateStreamerProfileAsync);
            var streamer = await _userManager.FindByIdAsync(request.UserId);
            if (streamer == null)
            {
                var errorMessage = "Streamer doesn't exist";
                Logger.LogError($"{methodName} ---> {errorMessage}");
                return new ManagerUpdateProfileResponse
                {
                    Succeeded = false,
                    Errors = new List<string> { errorMessage }
                };
            }

            var errorWhileUpdatingEmail = await TryUpdateUserEmail(streamer, request.Email);

            if (!string.IsNullOrWhiteSpace(errorWhileUpdatingEmail))
            {
                return new ManagerUpdateProfileResponse
                {
                    Succeeded = false,
                    Errors = new List<string> { errorWhileUpdatingEmail }
                };
            }
            
            streamer.Nickname = request.Nickname;
            streamer.MinDonatePriceInDollars = request.MinDonatePrice;
            streamer.MerchantId = request.MerchantId;
            var result = await _userManager.UpdateAsync(streamer);

            if (!result.Succeeded)
            {
                LogErrorsIfNotSucceeded(result, methodName);
                return new ManagerUpdateProfileResponse
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(s => s.Description).ToList()
                };
            }

            Logger.LogInformation($"{methodName} ---> Streamer profile updated");
            return new ManagerUpdateProfileResponse
            {
                Succeeded = true,
                Errors = Enumerable.Empty<string>()
            };
        });
    }

    public async Task<ManagerGetPortionedUsersResponse<UserProfileDto>> GetPortionedUsersAsync(ManagerGetPortionedUsersRequest request)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var takeCount = request.CurrentPortion * request.UsersPerPortion;
            var morePortionsExist = false;

            var userAmount = await _userManager.Users.LongCountAsync();

            if (userAmount == 0)
            {
                return new ManagerGetPortionedUsersResponse<UserProfileDto>
                {
                    CurrentPortion = 0,
                    MorePortionsExist = false,
                    UsersPerPortion = request.UsersPerPortion,
                    Users = Enumerable.Empty<UserProfileDto>()
                };
            }

            var users = await _userManager.Users
                .Take(takeCount)
                .Select(s => new UserProfileDto
                {
                    UserId = s.Id,
                    UserNickname = s.Nickname,
                    Email = s.Email
                })
                .ToListAsync();

            if (userAmount > request.CurrentPortion * request.UsersPerPortion)
            {
                morePortionsExist = true;
            }

            Logger.LogInformation($"{nameof(GetPortionedUsersAsync)} ---> Users amount: {users.Count()}");
            return new ManagerGetPortionedUsersResponse<UserProfileDto>
            {
                CurrentPortion = request.CurrentPortion,
                UsersPerPortion = request.UsersPerPortion,
                Users = users,
                MorePortionsExist = morePortionsExist
            };
        });
    }

    public async  Task<ManagerGetPortionedUsersResponse<StreamerProfileDto>> GetPortionedStreamersAsync(ManagerGetPortionedUsersRequest request)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var streamers = await _userManager.GetUsersInRoleAsync("streamer");
            var morePortionsExist = false;

            if (streamers.Count == 0)
            {
                return new ManagerGetPortionedUsersResponse<StreamerProfileDto>
                {
                    CurrentPortion = 0,
                    MorePortionsExist = false,
                    UsersPerPortion = request.UsersPerPortion,
                    Users = Enumerable.Empty<StreamerProfileDto>()
                };
            }
            
            var portionedStreamers = streamers
                .Take(request.CurrentPortion * request.UsersPerPortion)
                .Select(s => new StreamerProfileDto
                {
                    Email = s.Email,
                    StreamerId = s.Id,
                    MerchantId = s.MerchantId,
                    StreamerNickname = s.Nickname,
                    MinDonatePrice = s.MinDonatePriceInDollars
                }).ToList();

            if (streamers.Count > request.CurrentPortion * request.UsersPerPortion)
            {
                morePortionsExist = true;
            }
            
            Logger.LogInformation($"{nameof(GetPortionedStreamersAsync)} ---> Users amount: {portionedStreamers.Count()}");
            return new ManagerGetPortionedUsersResponse<StreamerProfileDto>
            {
                CurrentPortion = request.CurrentPortion + 1,
                UsersPerPortion = request.UsersPerPortion,
                Users = portionedStreamers,
                MorePortionsExist = morePortionsExist
            };
        });
    }

    private async Task<string> TryUpdateUserEmail(ApplicationUser user, string newEmail)
    {
        if (!user.Email.Equals(newEmail))
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == newEmail))
            {
                var errorMessage = "Current email actually exists";
                Logger.LogError($"{nameof(TryUpdateUserEmail)} ---> {errorMessage}");
                return errorMessage;
            }

            var settingResult = await _userManager.SetEmailAsync(user, user.Email);
            LogErrorsIfNotSucceeded(settingResult, $"{nameof(TryUpdateUserEmail)}");
            if (settingResult.Succeeded)
            {
                return string.Empty;
            }
        }

        return string.Empty;
    }

    private void LogErrorsIfNotSucceeded(IdentityResult result, string methodName)
    {
        if (result.Succeeded)
        {
            return;
        }

        Logger.LogError($"{methodName} ---> Errors:");
        foreach (var error in result.Errors)
        {
            Logger.LogError($"{error.Code}: {error.Description}");
        }
    }
}