using Identity.API.Data;
using Identity.API.Data.Entities;
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
    public AccountManagerService(
        IDbContextWrapper<AppDbContext> dbContext,
        ILogger<BaseDataService<AppDbContext>> logger,
        UserManager<ApplicationUser> userManager)
        : base(dbContext, logger)
    {
        _userManager = userManager;
        _dbContext = dbContext.DbContext;
    }

    public async Task<DeleteUserByIdResponse<bool>> DeleteUserByIdAsync(string userId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new DeleteUserByIdResponse<bool> { Data = false };
            }

            var user = new ApplicationUser { Id = userId };
            _dbContext.Entry(user).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
            return new DeleteUserByIdResponse<bool> { Data = true };
        });
    }

    public async Task<ManagerUpdateProfileResponse> UpdateUserProfileAsync(ManagerUpdateUserProfileRequest request)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var methodName = nameof(UpdateUserProfileAsync);
            var user = await _dbContext.Users.FirstOrDefaultAsync(f => f.Id == request.UserId);
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
            var user = await _dbContext.Users.FirstOrDefaultAsync(f => f.Id == request.UserId);
            if (user == null)
            {
                var errorMessage = "Streamer doesn't exist";
                Logger.LogError($"{methodName} ---> {errorMessage}");
                return new ManagerUpdateProfileResponse
                {
                    Succeeded = false,
                    Errors = new List<string> { errorMessage }
                };
            }

            var errorWhileUpdatingEmail = await TryUpdateUserEmail(user, request.Email);

            if (!string.IsNullOrWhiteSpace(errorWhileUpdatingEmail))
            {
                return new ManagerUpdateProfileResponse
                {
                    Succeeded = false,
                    Errors = new List<string> { errorWhileUpdatingEmail }
                };
            }
            
            user.Nickname = request.Nickname;
            user.MinDonatePriceInDollars = request.MinDonatePrice;
            user.MerchantId = request.MerchantId;
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

            Logger.LogInformation($"{methodName} ---> Streamer profile updated");
            return new ManagerUpdateProfileResponse
            {
                Succeeded = true,
                Errors = Enumerable.Empty<string>()
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