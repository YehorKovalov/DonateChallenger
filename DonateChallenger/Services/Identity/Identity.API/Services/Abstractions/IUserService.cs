using Identity.API.Models.DTOs;
using Identity.API.Models.Responses;

namespace Identity.API.Services.Abstractions;

public interface IUserService
{
    Task<GetUsernamesByUsersIdsResponse<IDictionary<string, string>>> GetUsernamesByUsersIdsAsync(IEnumerable<string> usersIds);
    Task<GetUserProfileResponse<UserProfileDto>> GetUserProfile(string userId);
    Task<ChangeProfileDataResponse<string>> ChangeUserNickname(string userId, string newNickname);
}